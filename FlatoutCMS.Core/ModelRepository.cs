using FlatoutCMS.Core.Content;
using FlatoutCMS.Core.Context;

namespace FlatoutCMS.Core
{
    public class ModelRepository
    {
        private readonly DataFileFetcher dataFileFetcher;
        private readonly DataParserFactory parserFactory;
        private readonly ModelTypeFetcher typeFetcher;

        public ModelRepository(DataFileFetcher dataFileFetcher, DataParserFactory parserFactory, ModelTypeFetcher typeFetcher)
        {
            this.dataFileFetcher = dataFileFetcher;
            this.parserFactory = parserFactory;
            this.typeFetcher = typeFetcher;
        }

        public Maybe<IPageModel> GetModel(string uri)
        {
            IPageModel model = null;

            dataFileFetcher.Find(uri).Apply(page =>
            {
                var parser = parserFactory.CreateParser(page);
                var baseModel = parser.Parse<PageModel>();
                var modelType = typeFetcher.GetModelType(baseModel.View);
                model = parser.Parse(modelType);
            });

            return model == null ? new Maybe<IPageModel>() : new Maybe<IPageModel>(model);
        }
    }
}
