using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Antelcat.Core.Extensions;

// ReSharper disable InconsistentNaming

namespace Antelcat.Core.Extensions
{
    /// <summary>
    /// IPAddress 扩展方法
    /// </summary>
    public static partial class IPAddressExtension
    {
        private static readonly Regex IpV4Regex = MyRegex();

        /// <summary>
        /// IPAddress 转 Int32
        /// <para>NOTE: 可能产生负数</para>
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int ToInt32(this IPAddress ip)
        {
            var x = 3;
            var bytes = ip.GetAddressBytes();
            return bytes.Sum(f => f << 8 * x--);
        }

        /// <summary>
        /// IPAddress 转 Int64
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ToInt64(this IPAddress ip)
        {
            var x = 3;
            var bytes = ip.GetAddressBytes();
            return bytes.Sum(f => (long)f << 8 * x--);
        }

        /// <summary>
        /// Int32 转 IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this int ip) => ((long)ip).ToIPAddress();

        /// <summary>
        /// Int64 转 IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this long ip)
        {
            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                bytes[3 - i] = (byte)(ip >> 8 * i & 255);
            }
            return new IPAddress(bytes);
        }

        /// <summary>
        /// 是否 IPv4 格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPv4(this string ip) => 
            !ip.IsNullOrWhiteSpace() && ip.Length is >= 7 and <= 15 && IpV4Regex.IsMatch(ip);

        /// <summary>
        /// 获取本机 IP 地址
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static IEnumerable<IPAddress> GetLocalIPAddresses(this AddressFamily addressFamily) => 
            Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .Where(m => m.AddressFamily == addressFamily);

        /// <summary>
        /// 获取一个本机的 IPv4 地址
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static IPAddress? GetLocalIPv4IPAddress(this AddressFamily addressFamily) => 
            GetLocalIPAddresses(addressFamily).LastOrDefault(m => !IPAddress.IsLoopback(m));


        [GeneratedRegex(@"^\d{1,3}[.]\d{1,3}[.]\d{1,3}[.]\d{1,3}$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}
