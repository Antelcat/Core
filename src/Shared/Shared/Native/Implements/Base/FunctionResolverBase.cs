#if !NET && !NETSTANDARD
using System;
using System.Collections.Generic;
#else
using System.Diagnostics.CodeAnalysis;
#endif
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Antelcat.Interfaces;

namespace Antelcat.Implements;

public abstract class FunctionResolverBase : IFunctionResolver
{
    private readonly Dictionary<string, IntPtr> loadedLibraries = new();
    private readonly object locker = new();

    private static readonly Func<IntPtr, Type, object> GetDelegateForFunctionPointerInternal =
        (Func<IntPtr, Type, object>)typeof(Marshal)
            .GetMethod(nameof(GetDelegateForFunctionPointerInternal),
                BindingFlags.Static | BindingFlags.NonPublic)!
            .CreateDelegate(typeof(Func<IntPtr, Type, object>));

    public bool TryGetFunctionDelegate<T>(string libraryPath, string functionName,
#if NET || NETSTANDARD
        [NotNullWhen(true)]
#endif
        out T? handler)
        where T : Delegate
    {
        handler = null;
        var handle = GetOrLoadLibrary(libraryPath, false);
        if (handle == IntPtr.Zero) return false;
        var address = FindFunctionPointer(handle, functionName);
        if (address == IntPtr.Zero) return false;
        handler = (T)GetDelegateForFunctionPointerInternal(address, typeof(T));
        return true;
    }

    public T? GetFunctionDelegate<T>(string libraryPath, string functionName, [DoesNotReturnIf(true)] bool throwOnError = true)
        where T : Delegate =>
        GetFunctionDelegate<T>(GetOrLoadLibrary(libraryPath, throwOnError), functionName, throwOnError);
    
    private T? GetFunctionDelegate<T>(IntPtr nativeLibraryHandle, string functionName, [DoesNotReturnIf(true)] bool throwOnError)
        where T : Delegate
    {
        var functionPointer = FindFunctionPointer(nativeLibraryHandle, functionName);
        if (functionPointer != IntPtr.Zero) return (T)GetDelegateForFunctionPointerInternal(functionPointer, typeof(T));
        if (!throwOnError) return null;
        throw new EntryPointNotFoundException($"Could not find the entry point for [ {functionName} ].");
    }

    private IntPtr GetOrLoadLibrary(string libraryPath, bool throwOnError)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (loadedLibraries.TryGetValue(libraryPath, out var ptr)) return ptr;
        lock (locker)
        {
            if (loadedLibraries.TryGetValue(libraryPath, out ptr)) return ptr;
            ptr = LoadNativeLibrary(libraryPath);
            if (ptr != IntPtr.Zero) loadedLibraries.Add(libraryPath, ptr);
            else if (throwOnError)
            {
                var err = Marshal.GetLastWin32Error();
                throw err != 0
                    ? new Win32Exception(err)
                    : new DllNotFoundException($"Could not load dll locates at : [ {libraryPath} ]");
            }

            return ptr;
        }
    }

    protected abstract IntPtr LoadNativeLibrary(string libraryName);
    protected abstract IntPtr FindFunctionPointer(IntPtr nativeLibraryHandle, string functionName);
}