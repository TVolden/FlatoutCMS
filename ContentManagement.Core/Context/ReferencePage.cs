using ContentManagement.Content;

namespace ContentManagement
{
    public class ReferencePage<TPageModel>
    {
        public TPageModel Model { get; private set; }
        public string Uri { get; private set; }

        public ReferencePage(TPageModel model, string uri)
        {
            Model = model;
            Uri = uri;
        }
    }
}