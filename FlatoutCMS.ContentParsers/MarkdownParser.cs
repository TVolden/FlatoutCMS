using FlatoutCMS.Core.Content;
using Markdig;

namespace FlatoutCMS.ContentParsers
{
    public class MarkdownParser : IMarkdownParser
    {
        public string Parse(string data)
        {
            return Markdown.ToHtml(data);
        }
    }
}
