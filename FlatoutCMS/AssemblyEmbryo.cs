using FlatoutCMS.Core;
using System.Reflection;

namespace FlatoutCMS
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
