namespace FlatoutCMS.ConntentManagement.Content
{
    public class DataParserFactory
    {
        private readonly IMarkdownParser markdownParser;
        private readonly IYamlParser yamlParser;

        public DataParserFactory(IMarkdownParser markdownParser, IYamlParser yamlParser)
        {
            this.markdownParser = markdownParser;
            this.yamlParser = yamlParser;
        }

        public IParser CreateParser(string data)
        {
            var reader = new DataReader();
            reader.Read(data);
            return new DataParser(markdownParser, yamlParser, reader);
        }
    }
}
