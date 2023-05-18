using Feast.Foundation.Core.Implements.Services;

namespace Feast.Foundation.Core.Attributes
{
    /// <summary>
    /// 不依赖构造函数实现属性和字段自动注入的注解，需要使用 <see cref="AutowiredServiceProviderFactory"/> 代理 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class AutowiredAttribute : Attribute
    { }
}
