using System;

namespace FlatoutCMS.Core.Content
{
    public interface IParser
    {
        TPageModel Parse<TPageModel>() where TPageModel : IPageModel;
        IPageModel Parse(Type modelName);
    }
}
