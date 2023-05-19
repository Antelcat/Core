using System.Reflection;
using Feast.Foundation.Core.Extensions;
using Feast.Foundation.Core.Interface.IL;

namespace Feast.Foundation.Core.Structs.IL;

public readonly struct ILGetter : ILMethod
{
    public readonly MemberInfo Method;
    private readonly Getter<object, object> getter;

    public ILGetter(PropertyInfo prop)
    {
        Method = prop;
        getter = prop.CreateGetter();
    }
    public ILGetter(FieldInfo field)
    {
        Method = field;
        getter = field.CreateGetter();
    }

    public static implicit operator ILGetter(PropertyInfo prop) => new ILGetter(prop);
    public static implicit operator ILGetter(FieldInfo field) => new ILGetter(field);


    public object? Invoke(object? target, params object?[]? parameters) => 
        getter.Invoke(target);
}