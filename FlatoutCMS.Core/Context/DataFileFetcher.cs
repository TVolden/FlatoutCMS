using System.IO;

namespace FlatoutCMS.Core.Context
{
    public class DataFileFetcher
    {
        private readonly DirectorySearcher directorySearcher;

        public DataFileFetcher(DirectorySearcher directorySearcher)
        {
            this.directorySearcher = directorySearcher;
        }

        public Maybe<string> Find(string uri)
        {
            var file = directorySearcher.GetFile(uri);
            System.Console.WriteLine(file);
            if (!File.Exists(file))
                return new Maybe<string>();

            return new Maybe<string>(FileReader.Read(file));
        }
    }
}
