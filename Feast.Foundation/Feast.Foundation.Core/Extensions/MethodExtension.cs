using System.Reflection;
using System.Reflection.Emit;
using Feast.Foundation.Core.Structs;

namespace Feast.Foundation.Core.Extensions
{
    public static class MethodExtension
    {
        public static IlInvoker CreateInvoker(this MethodInfo method) => new(method);
    }
}
