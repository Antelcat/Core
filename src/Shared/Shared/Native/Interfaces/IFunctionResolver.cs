using System.Diagnostics.CodeAnalysis;

namespace Antelcat.Interfaces;

public interface IFunctionResolver
{
    bool TryGetFunctionDelegate<T>(string libraryPath, string functionName, 
#if !NETFRAMEWORK
        [NotNullWhen(true)] 
#endif
        out T? handler)
        where T : Delegate;

    T? GetFunctionDelegate<T>(string libraryPath, string functionName, [DoesNotReturnIf(true)] bool throwOnError = true) where T : Delegate;
    
    //NativeLibrary.SetDllImportResolver(typeof(LinuxFunctionResolver).Assembly, (libraryName, assembly, searchPath) =>
    //{
    //    if (libraryName == Libdl) return dlopen(libraryName, RTLD_NOW);
    //    return IntPtr.Zero;
    //});
}