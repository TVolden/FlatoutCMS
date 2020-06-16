using System.Reflection;

namespace ContentManagement
{
    public class AssemblyEmbryo : IAssemblyEmbryo
    {
        public Assembly Assembly { get; }

        public AssemblyEmbryo(Assembly assembly)
        {
            Assembly = assembly;
        }
    }
}
