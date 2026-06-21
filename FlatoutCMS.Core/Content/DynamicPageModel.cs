using System.Collections.Generic;

namespace FlatoutCMS.Core.Content
{
    public class DynamicPageModel : IPageModel, IDynamicFieldsModel, IWritableDynamicFields
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string View { get; set; }

        private readonly Dictionary<string, string> fields = new();
        public IReadOnlyDictionary<string, string> Fields => fields;
        void IWritableDynamicFields.AddField(string key, string value) => fields[key] = value;
    }
}
