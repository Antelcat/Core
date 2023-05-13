
namespace Feast.Foundation.Extensions
{
    public static class TimeExtension
    {
        public enum TimeStampMode
        {
            MillionSecond,
            Second
        }
        
        /// <summary>
        /// 基准时间
        /// </summary>
        public static DateTime BaseTime { get; } = new(1970, 1, 1);

        /// <summary>
        /// 当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimestamp => DateTimeOffset.Now.ToUnixTimeMilliseconds();

        /// <summary>
        /// 转化为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime, TimeStampMode mode = TimeStampMode.MillionSecond) =>
            mode switch
            {
                TimeStampMode.Second => new DateTimeOffset(dateTime).ToUnixTimeSeconds(),
                _ => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds(),
            };

        /// <summary>
        /// 转化为UTC时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToUniversalTime(this long timestamp) =>
            DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

        /// <summary>
        /// 转化为本地时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this long timestamp) =>
            DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
    }
}
