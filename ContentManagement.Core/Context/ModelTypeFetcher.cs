using ContentManagement.Content;
using System;
using System.Linq;
using System.Reflection;

namespace ContentManagement.Context
{
    public class ModelTypeFetcher
    {
        private readonly ViewModelFinder modelFinder;
        private readonly IAssemblyEmbryo assemblyEmbryo;

        public ModelTypeFetcher(ViewModelFinder modelFinder, IAssemblyEmbryo assemblyEmbryo)
        {
            this.modelFinder = modelFinder;
            this.assemblyEmbryo = assemblyEmbryo;
        }

        public Type GetModelType(string viewName)
        {
            var type = typeof(PageModel);

            modelFinder.FindModel(viewName).Apply(name =>
            {
                type = assemblyEmbryo.Assembly.GetTypes().FirstOrDefault(t => t.Name == name);
            });

            return type;
        }
    }
}
