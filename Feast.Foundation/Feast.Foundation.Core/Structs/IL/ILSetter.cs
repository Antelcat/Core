using System.Reflection;
using Feast.Foundation.Core.Extensions;
using Feast.Foundation.Core.Interface.IL;

namespace Feast.Foundation.Core.Structs.IL;

public readonly struct ILSetter : ILMethod
{
    public readonly MemberInfo Method;
    private readonly Setter<object, object> setter;

    public ILSetter(PropertyInfo prop)
    {
        Method = prop;
        setter = prop.CreateSetter();
    }
    public ILSetter(FieldInfo field)
    {
        Method = field;
        setter = field.CreateSetter();
    }

    public static implicit operator ILSetter(PropertyInfo prop) => new ILSetter(prop);
    public static implicit operator ILSetter(FieldInfo field) => new ILSetter(field);

    public object? Invoke(object? target, params object?[]? parameters)
    {
        setter(ref target, parameters?.Length > 0 ? parameters[0] : null);
        return null;
    }
}
