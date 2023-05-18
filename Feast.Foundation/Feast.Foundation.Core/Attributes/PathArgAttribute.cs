using Feast.Foundation.Core.Implements;

namespace Feast.Foundation.Core.Attributes
{
    /// <summary>
    /// 使用 <see cref="PathResolver"/> 对该注解进行解析
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PathArgAttribute : Attribute
    {
        public readonly string? Name;
        public PathArgAttribute(string? name = null) => Name = name;
    }
}
