﻿using FlatoutCMS.Core.Content;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FlatoutCMS.ContentParsers
{
    public class YamlParser : IYamlParser
    {
        private IDeserializer deserializer;

        public YamlParser()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();
        }

        public TPageModel Parse<TPageModel>(string data) where TPageModel : IPageModel
        {
            return deserializer.Deserialize<TPageModel>(data);
        }
    }
}
