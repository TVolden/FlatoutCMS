namespace ContentManagement.Content
{
    public interface IYamlParser
    {
        TPageModel Parse<TPageModel>(string data) where TPageModel : IPageModel;
    }
}