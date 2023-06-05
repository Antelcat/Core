using Antelcat.Core.Implements.Services;

namespace Antelcat.Core.Attributes;

/// <summary>
/// 不依赖构造函数实现属性和字段自动注入的注解，需要使用 <see cref="AutowiredServiceProvider{TAttribute}"/> 代理服务提供器
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class AutowiredAttribute : Attribute
{ }