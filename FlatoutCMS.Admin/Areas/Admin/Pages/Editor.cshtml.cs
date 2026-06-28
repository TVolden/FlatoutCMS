using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace FlatoutCMS.Admin.Areas.Admin.Pages
{
    [Authorize(AuthenticationSchemes = "FlatoutAdmin")]
    public class EditorModel : PageModel
    {
        private static readonly string[] AllowedExtensions = { ".md", ".cshtml" };

        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        public List<FolderRoot> Roots { get; private set; } = new();

        public EditorModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public void OnGet()
        {
            Roots = BuildRoots();
        }

        public IActionResult OnGetFileContent(string path)
        {
            if (!TryDecodePath(path, out var fullPath) || !IsPathAllowed(fullPath))
                return BadRequest();
            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var content = System.IO.File.ReadAllText(fullPath, Encoding.UTF8);
            var ext = Path.GetExtension(fullPath).ToLowerInvariant();
            var role = GetRole(fullPath);
            return new JsonResult(new { content, extension = ext, role });
        }

        public IActionResult OnPostSaveFile([FromBody] SaveRequest request)
        {
            if (request == null || !TryDecodePath(request.Path, out var fullPath) || !IsPathAllowed(fullPath))
                return BadRequest();

            System.IO.File.WriteAllText(fullPath, request.Content ?? "", Encoding.UTF8);
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostCreateFile([FromBody] CreateFileRequest request)
        {
            if (request == null || !TryDecodePath(request.DirectoryPath, out var dirPath))
                return BadRequest();

            var normalizedDir = Path.GetFullPath(dirPath);
            if (!GetAllowedRoots().Any(root => normalizedDir.StartsWith(root, StringComparison.OrdinalIgnoreCase)))
                return BadRequest();

            var fileName = request.FileName?.Trim();
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return BadRequest("Invalid file name.");

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest($"Extension '{ext}' is not allowed. Use .md or .cshtml.");

            var fullPath = Path.Combine(normalizedDir, fileName);
            if (System.IO.File.Exists(fullPath))
                return BadRequest("A file with that name already exists.");

            System.IO.File.WriteAllText(fullPath, "", Encoding.UTF8);
            return new JsonResult(new { success = true, encodedPath = EncodePath(fullPath) });
        }

        public IActionResult OnPostPromoteFile([FromBody] PromoteRequest request)
        {
            if (request == null || !TryDecodePath(request.Path, out var fullPath))
                return BadRequest();

            var draftsRoot = GetDraftsRoot();
            var publishedRoot = GetPublishedRoot();

            if (draftsRoot == null || publishedRoot == null)
                return StatusCode(500, "Folders:Drafts or Folders:Published not configured.");

            if (!fullPath.StartsWith(draftsRoot, StringComparison.OrdinalIgnoreCase))
                return BadRequest("File is not in the Drafts folder.");

            var relativePart = fullPath[draftsRoot.Length..];
            var target = Path.Combine(publishedRoot, relativePart.TrimStart(Path.DirectorySeparatorChar));
            var targetDir = Path.GetDirectoryName(target);
            if (!string.IsNullOrEmpty(targetDir))
                Directory.CreateDirectory(targetDir);

            System.IO.File.Copy(fullPath, target, overwrite: true);
            return new JsonResult(new { success = true, targetPath = EncodePath(target) });
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private List<FolderRoot> BuildRoots()
        {
            var roots = new List<FolderRoot>();
            void AddRoot(string label, string role, string rootPath)
            {
                if (string.IsNullOrEmpty(rootPath)) return;
                var fullRoot = Path.GetFullPath(rootPath);
                if (!Directory.Exists(fullRoot)) return;
                roots.Add(new FolderRoot
                {
                    Label = label,
                    Role = role,
                    RootPath = fullRoot,
                    EncodedRootPath = EncodePath(fullRoot),
                    Children = BuildTree(fullRoot, role)
                });
            }
            AddRoot("Published", "published", GetPublishedRoot());
            AddRoot("Drafts", "drafts", GetDraftsRoot());
            AddRoot("Views", "views", GetViewsRoot());
            AddRoot("Pages", "pages", GetPagesRoot());
            return roots;
        }

        private List<FileNode> BuildTree(string dir, string role)
        {
            var nodes = new List<FileNode>();
            foreach (var sub in Directory.GetDirectories(dir).OrderBy(d => d))
            {
                var children = BuildTree(sub, role);
                nodes.Add(new FileNode { Name = Path.GetFileName(sub), IsDirectory = true, EncodedPath = EncodePath(sub), Children = children, Role = role });
            }
            foreach (var file in Directory.GetFiles(dir).OrderBy(f => f))
            {
                var ext = Path.GetExtension(file).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext)) continue;
                nodes.Add(new FileNode
                {
                    Name = Path.GetFileName(file),
                    IsDirectory = false,
                    EncodedPath = EncodePath(file),
                    Role = role,
                    Children = new()
                });
            }
            return nodes;
        }

        private bool IsPathAllowed(string fullPath)
        {
            var normalized = Path.GetFullPath(fullPath);
            return GetAllowedRoots().Any(root => normalized.StartsWith(root, StringComparison.OrdinalIgnoreCase));
        }

        private string GetRole(string fullPath)
        {
            var published = GetPublishedRoot();
            var drafts = GetDraftsRoot();
            var views = GetViewsRoot();
            var pages = GetPagesRoot();
            if (published != null && fullPath.StartsWith(published, StringComparison.OrdinalIgnoreCase)) return "published";
            if (drafts != null && fullPath.StartsWith(drafts, StringComparison.OrdinalIgnoreCase)) return "drafts";
            if (views != null && fullPath.StartsWith(views, StringComparison.OrdinalIgnoreCase)) return "views";
            if (pages != null && fullPath.StartsWith(pages, StringComparison.OrdinalIgnoreCase)) return "pages";
            return "unknown";
        }

        private IEnumerable<string> GetAllowedRoots() =>
            new[] { GetPublishedRoot(), GetDraftsRoot(), GetViewsRoot(), GetPagesRoot() }
                .Where(r => r != null)
                .Select(r => Path.GetFullPath(r));

        private string GetPublishedRoot() => ResolveFolderConfig("Folders:Published");
        private string GetDraftsRoot() => ResolveFolderConfig("Folders:Drafts");
        private string GetViewsRoot() => ResolveFolderConfig("Folders:Views");
        private string GetPagesRoot() => ResolveFolderConfig("Folders:Pages");

        private string ResolveFolderConfig(string key)
        {
            var val = configuration[key];
            if (string.IsNullOrEmpty(val)) return null;
            return Path.IsPathRooted(val) ? val : Path.Combine(env.ContentRootPath, val);
        }

        private static string EncodePath(string path) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(path))
                .Replace('+', '-').Replace('/', '_').TrimEnd('=');

        private static bool TryDecodePath(string encoded, out string path)
        {
            path = null;
            if (string.IsNullOrEmpty(encoded)) return false;
            try
            {
                var padded = encoded.Replace('-', '+').Replace('_', '/');
                padded += (padded.Length % 4) switch { 2 => "==", 3 => "=", _ => "" };
                path = Encoding.UTF8.GetString(Convert.FromBase64String(padded));
                return true;
            }
            catch { return false; }
        }
    }

    public class FolderRoot
    {
        public string Label { get; set; }
        public string Role { get; set; }
        public string RootPath { get; set; }
        public string EncodedRootPath { get; set; }
        public List<FileNode> Children { get; set; } = new();
    }

    public class FileNode
    {
        public string Name { get; set; }
        public string EncodedPath { get; set; }
        public string Role { get; set; }
        public bool IsDirectory { get; set; }
        public List<FileNode> Children { get; set; } = new();
    }

    public class SaveRequest
    {
        public string Path { get; set; }
        public string Content { get; set; }
    }

    public class PromoteRequest
    {
        public string Path { get; set; }
    }

    public class CreateFileRequest
    {
        public string DirectoryPath { get; set; }
        public string FileName { get; set; }
    }
}
