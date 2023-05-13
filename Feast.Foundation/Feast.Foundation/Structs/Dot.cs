using Feast.Foundation.Extensions;
using Feast.Foundation.Interfaces;

namespace Feast.Foundation.Structs
{
    public struct Dot : IDot<int>
    {
        public Dot() { Timestamp = TimeExtension.CurrentTimestamp; }
        public int X { get; set; }
        public int Y { get; set; }
        public long Timestamp { get; set; }
        public DotType Type { get; set; } = DotType.None;

        /// <summary>
        /// 相隔距离过远
        /// </summary>
        /// <param name="another">另一个点</param>
        /// <param name="distance">距离</param>
        /// <returns></returns>
        public bool AwayTo(IDot<int> another, int distance) => Math.Pow(Y - another.Y, 2) + Math.Pow(X - another.X, 2) > distance * distance;

        /// <summary>
        /// 相隔时间过长
        /// </summary>
        /// <param name="another">另一个点</param>
        /// <param name="interval">间隔</param>
        /// <returns></returns>
        public bool FarTo(IDot another, long interval) => Math.Abs(Timestamp - another.Timestamp) > interval;

        public static implicit operator Dot(DotType type) => new() { Type = type };
        public static implicit operator Dot(Tuple<int, int> location) =>
            new()
            {
                X = location.Item1,
                Y = location.Item2,
                Type = DotType.Move,
            };

        public override string ToString() =>
            $"{{\n" +
            $"  {nameof(X)}:{X},\n" +
            $"  {nameof(Y)}:{Y},\n" +
            $"  {nameof(Timestamp)}:{Timestamp},\n" +
            $"  {nameof(Type)}:{Type},\n" +
            $"}}\n";
    }
}
