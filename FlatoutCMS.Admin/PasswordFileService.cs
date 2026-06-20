using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace FlatoutCMS.Admin
{
    public class PasswordFileService
    {
        private readonly string passwdFilePath;

        public PasswordFileService(IWebHostEnvironment env, IConfiguration configuration)
        {
            passwdFilePath = configuration["Admin:PasswordFile"]
                ?? Path.Combine(env.ContentRootPath, ".passwd");
        }

        public bool AnyUserExists()
        {
            if (!File.Exists(passwdFilePath)) return false;
            return File.ReadLines(passwdFilePath)
                .Any(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith('#'));
        }

        public bool ValidateUser(string username, string password)
        {
            if (!File.Exists(passwdFilePath)) return false;
            foreach (var line in File.ReadLines(passwdFilePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith('#')) continue;
                var parts = line.Split(':');
                if (parts.Length != 4) continue;
                if (!string.Equals(parts[0], username, StringComparison.OrdinalIgnoreCase)) continue;
                if (!int.TryParse(parts[1], out int iterations)) continue;
                var salt = Convert.FromBase64String(parts[2]);
                var storedHash = parts[3];
                return storedHash == ComputeHash(password, salt, iterations);
            }
            return false;
        }

        public void SetPassword(string username, string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            const int iterations = 100000;
            var newLine = $"{username}:{iterations}:{Convert.ToBase64String(salt)}:{ComputeHash(password, salt, iterations)}";

            var lines = File.Exists(passwdFilePath)
                ? new List<string>(File.ReadAllLines(passwdFilePath))
                : new List<string> { "# FlatoutCMS Admin Password File" };

            bool replaced = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].TrimStart().StartsWith('#')) continue;
                var parts = lines[i].Split(':');
                if (parts.Length >= 1 && string.Equals(parts[0], username, StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = newLine;
                    replaced = true;
                    break;
                }
            }
            if (!replaced) lines.Add(newLine);
            File.WriteAllLines(passwdFilePath, lines, Encoding.UTF8);
        }

        private static string ComputeHash(string password, byte[] salt, int iterations)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(hash);
        }
    }
}
