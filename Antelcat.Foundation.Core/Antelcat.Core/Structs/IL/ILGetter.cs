using System.Reflection;
using Antelcat.Core.Extensions;
using Antelcat.Core.Interface.IL;
using Antelcat.Extensions;

namespace Antelcat.Core.Structs.IL;

public readonly struct ILGetter : ILMethod
{
    public readonly MemberInfo Method;
    public readonly Getter<object, object> Getter;

    public ILGetter(PropertyInfo prop)
    {
        Method = prop;
        Getter = prop.CreateGetter();
    }
    public ILGetter(FieldInfo field)
    {
        Method = field;
        Getter = field.CreateGetter();
    }

    public static implicit operator ILGetter(PropertyInfo prop) => new ILGetter(prop);
    public static implicit operator ILGetter(FieldInfo field) => new ILGetter(field);


    public object? Invoke(object? target, params object?[]? parameters) => Getter.Invoke(target);
}