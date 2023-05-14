using Feast.Foundation.Core.Structs;
using System.Reflection;

namespace Feast.Foundation.Core.Extensions
{
    public static class MethodExtension
    {
        public static IlInvoker CreateInvoker(this MethodInfo method) => new(method);
    }
}
