using FlatoutCMS.Core.Content;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FlatoutCMS.ContentParsers
{
    public class YamlParser : IYamlParser
    {
        private readonly IDeserializer deserializer;
        private readonly IDeserializer rawDeserializer;

        public YamlParser()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            rawDeserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();
        }

        public TPageModel Parse<TPageModel>(string data) where TPageModel : IPageModel
        {
            return deserializer.Deserialize<TPageModel>(data);
        }

        public Dictionary<string, object> ParseRaw(string data)
        {
            return rawDeserializer.Deserialize<Dictionary<string, object>>(data) ?? new();
        }
    }
}
