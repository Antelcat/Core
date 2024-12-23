using System.Reflection;
using System.Runtime.InteropServices;
using Antelcat.Implements;
using Antelcat.Interfaces;
#if !NET && !NETSTANDARD
using System;
using System.IO;
#else
using System.Diagnostics.CodeAnalysis;
#endif

#nullable enable
namespace Antelcat.Extensions;

public static class NativeExtension
{
    private static readonly IFunctionResolver Resolver;

    static NativeExtension()
    {
        Resolver = Environment.OSVersion.Platform switch
        {
            PlatformID.Unix   => new LinuxFunctionResolver(),
            PlatformID.MacOSX => new MacFunctionResolver(),
            _                 => new WindowsFunctionResolver(),
        };
    }

    public static TDelegate GetFunctionDelegate<TDelegate>(string libraryPath, string functionName) where TDelegate : Delegate =>
        Resolver.GetFunctionDelegate<TDelegate>(libraryPath, functionName);

    public static TDelegate GetFunctionDelegate<TDelegate>(this FileInfo fileInfo, string functionName) where TDelegate : Delegate =>
        GetFunctionDelegate<TDelegate>(fileInfo.FullName, functionName);

    public static bool TryGetFunctionDelegate<T>(string libraryPath, string functionName,
#if NET || NETSTANDARD
        [NotNullWhen(true)]
#endif
        out T? handler) where T : Delegate =>
        Resolver.TryGetFunctionDelegate(libraryPath, functionName, out handler);

    public static bool TryGetFunctionDelegate<T>(this FileInfo fileInfo, string functionName,
#if NET || NETSTANDARD
        [NotNullWhen(true)]
#endif
        out T? handler) where T : Delegate =>
        TryGetFunctionDelegate(fileInfo.FullName, functionName, out handler);


    public static IntPtr ToPointer(this Delegate @delegate) => Marshal.GetFunctionPointerForDelegate(@delegate);

    public static TDelegate ToDelegate<TDelegate>(this IntPtr pointer) where TDelegate : Delegate =>
        Marshal.GetDelegateForFunctionPointer<TDelegate>(pointer);

    public static void SetDllImportResolver(this Assembly assembly,
        Func<string, Assembly, DllImportSearchPath?, string> resolver) =>
        NativeLibrary.SetDllImportResolver(assembly,
            (libraryName, targetAssembly, dllImportSearchPath) =>
                NativeLibrary.Load(resolver(libraryName, targetAssembly, dllImportSearchPath)));
}