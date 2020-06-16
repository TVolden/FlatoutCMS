using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ContentManagement.Context
{
    public class ViewModelFinder
    {
        private const string dataFileExtension = ".cshtml";
        private readonly string viewPath;

        public ViewModelFinder(string viewPath)
        {
            this.viewPath = viewPath;
        }

        public Maybe<string> FindModel(string view)
        {
            var file = $"{viewPath}{view}{dataFileExtension}";

            if (!File.Exists(file))
                return new Maybe<string>();

            var data = GetData(file);

            var match = Regex.Match(data, "@model (.+)\r");
            if (match.Success && match.Groups[1].Success)
                return new Maybe<string>(match.Groups[1].Value);

            return new Maybe<string>();
        }

        private string GetData(string file)
        {
            var data = string.Empty;
            using (var stream = File.OpenRead(file))
            {
                data = new StreamReader(stream).ReadToEnd();
            }
            return data;
        }
    }
}
