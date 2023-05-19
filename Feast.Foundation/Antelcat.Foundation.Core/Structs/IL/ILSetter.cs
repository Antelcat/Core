using System.Reflection;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Interface.IL;

namespace Antelcat.Foundation.Core.Structs.IL;

public readonly struct ILSetter : ILMethod
{
    public readonly MemberInfo Method;
    public readonly Setter<object, object> Setter;

    public ILSetter(PropertyInfo prop)
    {
        Method = prop;
        Setter = prop.CreateSetter();
    }
    public ILSetter(FieldInfo field)
    {
        Method = field;
        Setter = field.CreateSetter();
    }

    public static implicit operator ILSetter(PropertyInfo prop) => new ILSetter(prop);
    public static implicit operator ILSetter(FieldInfo field) => new ILSetter(field);

    public object? Invoke(object? target, params object?[]? parameters)
    {
        Setter(ref target, parameters?.Length > 0 ? parameters[0] : null);
        return null;
    }
}
