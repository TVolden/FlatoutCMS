using System.IO;
using System.Text;

namespace ContentManagement.Content
{
    class DataReader
    {
        private enum ReaderStates { skip, yaml, markdown }

        private const string seperator = "---";
        private StringBuilder yaml;
        private ReaderStates state;

        public string Markdown { get; private set; }
        public string Yaml => yaml.ToString();

        public DataReader()
        {
            yaml = new StringBuilder();
            state = ReaderStates.skip;
        }

        public void Read(string data)
        {
            var reader = new StringReader(data);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                switch (state)
                {
                    case ReaderStates.skip:
                        if (line.StartsWith(seperator))
                        {
                            state = ReaderStates.yaml;
                        }
                        break;
                    case ReaderStates.yaml:
                        if (!line.StartsWith(seperator))
                        {
                            yaml.AppendLine(line);
                        }
                        else
                        {
                            state = ReaderStates.markdown;
                        }
                        break;
                }

                if (state == ReaderStates.markdown)
                    break;
            }
            Markdown = reader.ReadToEnd();
        }
    }
}
