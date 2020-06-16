using System;

namespace ContentManagement.Content
{
    public interface IParser
    {
        TPageModel Parse<TPageModel>() where TPageModel : IPageModel;
        IPageModel Parse(Type modelName);
    }
}
