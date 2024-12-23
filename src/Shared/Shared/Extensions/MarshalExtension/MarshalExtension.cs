#if !NET && !NETSTANDARD
using System;
using System.IO;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Antelcat.Extensions;
public static partial class MarshalExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T* Pointer<T>(this T target) where T : unmanaged => &target;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToBytes(this IntPtr ptr) => 
        IntPtr.Size == sizeof(int) 
            ? BitConverter.GetBytes((int)ptr) 
            : BitConverter.GetBytes((long)ptr);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T* ToPointer<T>(this T[] array) where T : unmanaged
    {
        fixed (T* p = array) return p;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr ToIntPtr<T>(this T[] array, int index = 0) => Marshal.UnsafeAddrOfPinnedArrayElement(array, index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr CopyToIntPtr(this byte[] bytes)
    {
        var ret = Marshal.AllocHGlobal(IntPtr.Zero);
        Marshal.Copy(bytes, 0, ret, bytes.Length);
        return ret;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream ToStream(this IntPtr intPtr, int length)
    {
        unsafe
        {
            return new UnmanagedMemoryStream((byte*)intPtr, length, length, FileAccess.Read);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] CopyToBytes(this IntPtr intPtr, int length, bool free = true)
    {
        var ret = new byte[length];
        Marshal.Copy(intPtr, ret, 0, length);
        if (free) Marshal.FreeHGlobal(intPtr);
        return ret;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dispose(this IntPtr intPtr) => Marshal.FreeHGlobal(intPtr);
}