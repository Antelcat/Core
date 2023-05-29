using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Antelcat.Foundation.Core.Extensions;

public static class DebugExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Debug<T>(this T target)
    {
#if DEBUG
        Debugger.Break();
#endif
        return target;
    } 
}