using System;
using System.Linq;
using System.Reflection;

namespace ContentManagement.Content
{
    class DataParser : IParser
    {
        private readonly IMarkdownParser markdownParser;
        private readonly IYamlParser yamlParser;
        private readonly DataReader reader;
        private string markdownCache;
        private string Markdown
        {
            get
            {
                if (markdownCache == null)
                    markdownCache = markdownParser.Parse(reader.Markdown);
                return markdownCache;
            }
        }

        public DataParser(IMarkdownParser markdownParser, IYamlParser yamlParser, DataReader dataReader)
        {
            this.markdownParser = markdownParser;
            this.yamlParser = yamlParser;
            reader = dataReader;
        }

        public TPageModel Parse<TPageModel>() where TPageModel : IPageModel
        {
            var model = yamlParser.Parse<TPageModel>(reader.Yaml);
            model.Content = Markdown;
            return model;
        }

        public IPageModel Parse(Type modelType)
        {
            if (!typeof(IPageModel).IsAssignableFrom(modelType))
                throw new ArgumentException("Not a valid model.", nameof(modelType));

            MethodInfo method = typeof(IYamlParser).GetMethod("Parse");
            var model = (IPageModel) method.MakeGenericMethod(modelType).Invoke(yamlParser, new[] { reader.Yaml });
            model.Content = Markdown;
            return model;
        }
    }
}
