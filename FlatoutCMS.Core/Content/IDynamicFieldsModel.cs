using System.Collections.Generic;

namespace FlatoutCMS.Core.Content
{
    public interface IDynamicFieldsModel
    {
        IReadOnlyDictionary<string, string> Fields { get; }
    }
}
