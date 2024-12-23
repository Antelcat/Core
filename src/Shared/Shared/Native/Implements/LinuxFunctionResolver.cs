#if !NET && !NETSTANDARD
using System;
#endif
using System.Runtime.InteropServices;

namespace Antelcat.Implements;

public class LinuxFunctionResolver : FunctionResolverBase
{
    private const string Libdl = "libdl.so.2";

    private const int RTLD_NOW = 0x002;
    protected override IntPtr LoadNativeLibrary(string libraryName) => dlopen(libraryName, RTLD_NOW);

    protected override IntPtr FindFunctionPointer(IntPtr nativeLibraryHandle, string functionName) =>
        dlsym(nativeLibraryHandle, functionName);

    [DllImport(Libdl)]
    private static extern IntPtr dlsym(IntPtr handle, string symbol);

    [DllImport(Libdl)]
    private static extern IntPtr dlopen(string fileName, int flag);
}