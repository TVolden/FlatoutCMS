using System.Collections.Generic;
using System.IO;

namespace ContentManagement.Context
{
    public class DirectorySearcher
    {
        private const string dataFileExtension = ".md";
        private readonly string dataPath;

        public DirectorySearcher(string dataPath)
        {
            this.dataPath = dataPath;
        }

        public string GetFile(string uri)
        {
            return $"{dataPath}{uri}{dataFileExtension}";
        }

        public IEnumerable<string> GetFiles(string uri)
        {
            var directory = new DirectoryInfo($"{dataPath}{uri}");
            foreach (var item in directory.GetFiles($"*{dataFileExtension}"))
            {
                int index = item.Name.LastIndexOf('.');
                yield return $"{uri}/{item.Name.Substring(0, index)}";
            }
        }

    }
}
