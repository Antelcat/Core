using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Feast.Foundation.Core.Attributes;
using Feast.Foundation.Core.Extensions;

namespace Feast.Foundation.Core.Implements
{
    /// <summary>
    /// 用于解析含有 <see cref="PathArgAttribute"/> 的类型
    /// </summary>
    public class PathResolver
    {
        public static Tuple<char, char> DefaultWrapper { get; set; } = new('{', '}');
        private readonly Tuple<char, char> wrapper;
        internal PathResolver(Tuple<char, char>? wrapper = null) => this.wrapper = wrapper ?? DefaultWrapper;
        private string Wrap(string name) => $"{wrapper.Item1}{name}{wrapper.Item2}";
        internal T ResolveInternal<T>(T raw)
        {
            var props = typeof(T)
                .GetProperties()
                .Where(static x => x.GetCustomAttribute<PathArgAttribute>() != null
                                   && x is { CanWrite: true, CanRead: true }
                                   && x.PropertyType == typeof(string)).ToList();
            return props.Aggregate(raw, (i, p1) =>
            {
                var name = Wrap(p1.GetCustomAttribute<PathArgAttribute>()!.Name ?? p1.Name);
                var self = (string)p1.GetValue(raw)!;
                props.ForEach(p2 =>
                {
                    if (p2 == p1) return;
                    p2.SetValue(raw, ((string)p2.GetValue(raw)!).Replace(name, self));
                });
                return i;
            });
        }
        public static T Resolve<T>(T raw, Tuple<char, char>? wrapper = null) =>
            new PathResolver(wrapper).ResolveInternal(raw);
    }
}
