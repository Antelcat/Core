using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Antelcat.Core.Extensions;

public static class MarshalExtension
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte* ToPointer(this byte[] bytes)
    {
        fixed (byte* p = bytes) return p;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr ToIntPtr(this byte[] bytes)
    {
        unsafe
        {
            return (IntPtr)bytes.ToPointer();
        }
    }

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