using System.Runtime.CompilerServices;

namespace Antelcat.Core.Extensions
{
    public class InterlockedExtension
    {
        /// <summary>
        /// unsigned equivalent of <see cref="Interlocked.Increment(ref int)"/>
        /// </summary>
        public static uint Increment(ref uint location)
        {
            var incrementedSigned = Interlocked.Increment(ref Unsafe.As<uint, int>(ref location));
            return Unsafe.As<int, uint>(ref incrementedSigned);
        }

        /// <summary>
        /// unsigned equivalent of <see cref="Interlocked.Increment(ref long)"/>
        /// </summary>
        public static ulong Increment(ref ulong location)
        {
            var incrementedSigned = Interlocked.Increment(ref Unsafe.As<ulong, long>(ref location));
            return Unsafe.As<long, ulong>(ref incrementedSigned);
        }
    }
}
