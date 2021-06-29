namespace FlatoutCMS.Core.Content
{
    public interface IMarkdownParser
    {
        string Parse(string markdown);
    }
}