using System.Reflection;

namespace FlatoutCMS.ConntentManagement
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
