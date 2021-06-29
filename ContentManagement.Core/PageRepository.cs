using FlatoutCMS.ConntentManagement.Content;
using FlatoutCMS.ConntentManagement.Context;
using System.Collections.Generic;

namespace FlatoutCMS.ConntentManagement
{
    public class PageRepository
    {
        private readonly DataParserFactory parserFactory;
        private readonly DirectorySearcher directorySearcher;

        public PageRepository(DataParserFactory parserFactory, DirectorySearcher directorySearcher)
        {
            this.parserFactory = parserFactory;
            this.directorySearcher = directorySearcher;
        }

        public IEnumerable<ReferencePage<TPageModel>> GetPagesIn<TPageModel>(string uri) where TPageModel : IPageModel
        {
            foreach (var fileUri in directorySearcher.GetFiles(uri))
            {
                var data = FileReader.Read(directorySearcher.GetFile(fileUri));
                var parser = parserFactory.CreateParser(data);
                var model = parser.Parse<TPageModel>();
                yield return new ReferencePage<TPageModel>(model, fileUri);
            } 
        }
    }
}
