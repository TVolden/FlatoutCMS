using ContentManagement.Content;
using Markdig;

namespace ContentParsers
{
    public class MarkdownParser : IMarkdownParser
    {
        public string Parse(string data)
        {
            return Markdown.ToHtml(data);
        }
    }
}
