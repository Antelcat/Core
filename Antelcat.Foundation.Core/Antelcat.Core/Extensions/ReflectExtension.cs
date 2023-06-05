using System.Reflection;
using System.Reflection.Emit;

namespace Antelcat.Core.Extensions;

#region Delegates

public delegate void Setter<TTarget, in TIn>(ref TTarget? target, TIn? value);

public delegate TOut? Getter<in TTarget, out TOut>(TTarget? target);

public delegate TReturn? Invoker<in TTarget, out TReturn>(TTarget? target, params object?[]? args);

public delegate T? Ctor<out T>(params object?[]? parameters);

#endregion
public static partial class ReflectExtension
{
    public static Ctor<object> CreateCtor(this Type type, params Type[] ctorParamTypes) =>
        CreateCtor<object>(type, ctorParamTypes);
    
    public static Setter<object,object> CreateSetter(this FieldInfo field) => 
        CreateSetter<object, object>(field);

    public static Setter<object, object> CreateSetter(this PropertyInfo prop) =>
        CreateSetter<object, object>(prop);

    public static Getter<object, object> CreateGetter(this PropertyInfo prop) =>
        CreateGetter<object, object>(prop);

    public static Getter<object, object> CreateGetter(this FieldInfo field) =>
        CreateGetter<object, object>(field);

    public static Invoker<object, object> CreateInvoker(this MethodInfo method) =>
        CreateInvoker<object, object>(method);

    public static Ctor<T> CreateCtor<T>(this Type type, params Type[] paramTypes)
    {
        var dynMethod = new DynamicMethod(string.Empty, typeof(T), new[] { typeof(object[]) });
        var il = dynMethod.GetILGenerator();
        CtorIL<T>(il, type, paramTypes);
        return (Ctor<T>)dynMethod.CreateDelegate(typeof(Ctor<T>));
    }
    
    public static Getter<TTarget, TReturn> CreateGetter<TTarget, TReturn>(this PropertyInfo prop) =>
        !prop.CanRead
            ? throw new InvalidOperationException("Property is not readable " + prop.Name)
            : GenerateDelegate<Getter<TTarget, TReturn>, PropertyInfo>(
                prop,
                MapPropGetter<TTarget>,
                typeof(TReturn),
                typeof(TTarget));

    public static Setter<TTarget, TValue> CreateSetter<TTarget, TValue>(this PropertyInfo prop) =>
        !prop.CanWrite
            ? throw new InvalidOperationException("Property is not writable " + prop.Name)
            : GenerateDelegate<Setter<TTarget, TValue>, PropertyInfo>(
                prop,
                MapPropSetter<TTarget>,
                typeof(void),
                typeof(TTarget).MakeByRefType(),
                typeof(TValue));

    public static Getter<TTarget, TReturn> CreateGetter<TTarget, TReturn>(this FieldInfo field) =>
        GenerateDelegate<Getter<TTarget, TReturn>, FieldInfo>(
            field,
            MapFieldGetter<TTarget>,
            typeof(TReturn),
            typeof(TTarget));

    public static Setter<TTarget, TValue> CreateSetter<TTarget, TValue>(this FieldInfo field) =>
        GenerateDelegate<Setter<TTarget, TValue>, FieldInfo>(
            field,
            MapFieldSetter<TTarget>,
            typeof(void),
            typeof(TTarget).MakeByRefType(),
            typeof(TValue));

  
    public static Invoker<TTarget, TReturn> CreateInvoker<TTarget, TReturn>(this MethodInfo method) =>
        GenerateDelegate<Invoker<TTarget, TReturn>, MethodInfo>(
            method,
            MethodInvokeIL<TTarget>,
            typeof(TReturn),
            typeof(TTarget),
            typeof(object[]));
}

#region Generator

public static partial class ReflectExtension
{
    private static TDelegate GenerateDelegate<TDelegate, TMember>(TMember member,
        Action<ILGenerator, TMember> generator,
        Type returnType,
        params Type[] paramTypes)
        where TMember : MemberInfo
        where TDelegate : class
    {
        var method = new DynamicMethod(string.Empty, returnType, paramTypes, true);
        var il = method.GetILGenerator();
        generator(il, member);
        var result = method.CreateDelegate(typeof(TDelegate));
        return (TDelegate)(object)result;
    }
}

#endregion

#region Mapper

public static partial class ReflectExtension
{
    private static void MapPropGetter<TTarget>(ILGenerator il, PropertyInfo prop)
    {
        var method = prop.GetGetMethod(true)!;
        MemberGetterIL<TTarget>(
            il,
            prop,
            prop.PropertyType,
            method.IsStatic,
            (e, p) => e.CallOrVirtualEx(method));
    }

    private static void MapPropSetter<TTarget>(ILGenerator il, PropertyInfo prop)
    {
        var method = prop.GetSetMethod(true)!;
        MemberSetterIL<TTarget>(
            il,
            prop,
            prop.PropertyType,
            method.IsStatic,
            (e, p) => e.CallOrVirtualEx(method)
        );
    }

    private static void MapFieldGetter<TTarget>(ILGenerator il, FieldInfo field) =>
        MemberGetterIL<TTarget>(il,
            field,
            field.FieldType,
            field.IsStatic,
            (e, f) =>
            {
                if (field.IsLiteral)
                {
                    if (field.FieldType == typeof(bool))
                        e.EmitEx(OpCodes.Ldc_I4_1);
                    else if (field.FieldType == typeof(int))
                        e.EmitEx(OpCodes.Ldc_I4, (int)field.GetRawConstantValue()!);
                    else if (field.FieldType == typeof(float))
                        e.EmitEx(OpCodes.Ldc_R4, (float)field.GetRawConstantValue()!);
                    else if (field.FieldType == typeof(double))
                        e.EmitEx(OpCodes.Ldc_R8, (double)field.GetRawConstantValue()!);
                    else if (field.FieldType == typeof(string))
                        e.EmitEx(OpCodes.Ldstr, (string)field.GetRawConstantValue()!);
                    else
                        throw new NotSupportedException(
                            $"Cannot create a FieldGetter for type: {field.FieldType.Name} ");
                }
                else
                    e.LodFldEx(field);
            });

    private static void MapFieldSetter<TTarget>(ILGenerator il, FieldInfo field) =>
        MemberSetterIL<TTarget>(
            il,
            field,
            field.FieldType,
            field.IsStatic,
            (e, f) => e.SetFldEx(field));
}

#endregion

#region IL

public static partial class ReflectExtension
{
    private static void CtorIL<T>(ILGenerator il, Type type, Type[] paramTypes)
    {
        var targetType = typeof(T) == typeof(object) ? type : typeof(T);

        if (targetType.IsValueType && paramTypes.Length == 0)
        {
            var tmp = il.DeclareLocal(targetType);
            il.EmitEx(OpCodes.Ldloca, tmp)
                .EmitEx(OpCodes.Initobj, targetType)
                .EmitEx(OpCodes.Ldloc, 0);
        }
        else
        {
            var ctor = targetType.GetConstructor(paramTypes);
            if (ctor == null)
                throw new Exception("Generating constructor for type: " + targetType +
                                    (paramTypes.Length == 0
                                        ? "No empty constructor found!"
                                        : "No constructor found that matches the following parameter types: " +
                                          string.Join(",", paramTypes.Select(x => x.Name).ToArray())));

            var parameters = ctor.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                il.EmitEx(OpCodes.Ldarg_0)
                    .EmitEx(OpCodes.Ldc_I4, i)
                    .EmitEx(OpCodes.Ldelem_Ref);
                if (!parameters[i].IsOut)
                {
                    il.EmitEx(OpCodes.Unbox_Any, paramTypes[i]);
                }
            }

            il.EmitEx(OpCodes.Newobj, ctor);
        }

        if (typeof(T) == typeof(object) && targetType.IsValueType)
            il.EmitEx(OpCodes.Box, targetType);
        il.EmitEx(OpCodes.Ret);
    }

    private static void MemberGetterIL<TTarget>(
        ILGenerator il,
        MemberInfo member,
        Type memberType,
        bool isStatic,
        Action<ILGenerator, MemberInfo> emitAction)
    {
        if (typeof(TTarget) == typeof(object)) // weakly-typed?
        {
            if (!isStatic)
                il.EmitEx(OpCodes.Ldarg_0)
                    .UnboxOrCastEx(member.DeclaringType!);
            il.Then(i => emitAction(i, member))
                .BoxIfValueType(memberType);
        }
        else
        {
            if (!isStatic)
                il.LdArgIfClass(0, typeof(TTarget));
            emitAction(il, member);
        }

        il.EmitEx(OpCodes.Ret);
    }

    private static void MemberSetterIL<TTarget>(
        ILGenerator il,
        MemberInfo member,
        Type memberType,
        bool isStatic,
        Action<ILGenerator, MemberInfo> emitAction)
    {
        var targetType = typeof(TTarget);
        var stronglyTyped = targetType != typeof(object);

        if (isStatic)
        {
            il.EmitEx(OpCodes.Ldarg_1);
            if (!stronglyTyped)
                il.EmitEx(OpCodes.Unbox_Any, memberType)
                    .Then(i => emitAction(i, member))
                    .EmitEx(OpCodes.Ret);
            return;
        }

        if (stronglyTyped)
        {
            il.EmitEx(OpCodes.Ldarg_0)
                .LdRefIfClass(targetType)
                .EmitEx(OpCodes.Ldarg_1)
                .Then(i => emitAction(i, member))
                .EmitEx(OpCodes.Ret);
            return;
        }

        targetType = member.DeclaringType!;
        if (!targetType.IsValueType)
        {
            il.EmitEx(OpCodes.Ldarg_0)
                .EmitEx(OpCodes.Ldind_Ref)
                .EmitEx(OpCodes.Castclass, targetType)
                .EmitEx(OpCodes.Ldarg_1)
                .EmitEx(OpCodes.Unbox_Any, memberType)
                .Then(i => emitAction(i, member))
                .EmitEx(OpCodes.Ret);
            return;
        }

        il.DeclareLocal(targetType);
        il.EmitEx(OpCodes.Ldarg_0)
            .EmitEx(OpCodes.Ldind_Ref)
            .EmitEx(OpCodes.Unbox_Any, targetType)
            .EmitEx(OpCodes.Stloc_0)
            .EmitEx(OpCodes.Ldloca, 0)
            .EmitEx(OpCodes.Ldarg_1)
            .EmitEx(OpCodes.Unbox_Any, memberType)
            .Then(i => emitAction(i, member))
            .EmitEx(OpCodes.Ldarg_0)
            .EmitEx(OpCodes.Ldloc_0)
            .EmitEx(OpCodes.Box, targetType)
            .EmitEx(OpCodes.Stind_Ref)
            .EmitEx(OpCodes.Ret);
    }

    private static void MethodInvokeIL<TTarget>(ILGenerator il, MethodInfo method)
    {
        var weaklyTyped = typeof(TTarget) == typeof(object);
        if (!method.IsStatic)
        {
            var targetType = weaklyTyped ? method.DeclaringType! : typeof(TTarget);
            il.DeclareLocal(targetType);
            il.EmitEx(OpCodes.Ldarg_0);
            if (weaklyTyped)
                il.EmitEx(OpCodes.Unbox_Any, targetType);
            il.EmitEx(OpCodes.Stloc_0)
                .LdLocIfClass(0, targetType);
        }

        var prams = method.GetParameters();
        for (var i = 0 ; i < prams.Length; i++)
        {
            il.EmitEx(OpCodes.Ldarg_1)
                .EmitFastInt(i)
                .EmitEx(OpCodes.Ldelem_Ref);

            var param = prams[i];
            var dataType = param.ParameterType;
            if (dataType.IsByRef) dataType = dataType.GetElementType()!;

            var tmp = il.DeclareLocal(dataType);
            (param.IsOut 
                    ? il 
                    : il.EmitEx(OpCodes.Unbox_Any, dataType))
                .EmitEx(OpCodes.Stloc, tmp)
                .LdLocIfNotByRef(tmp, param.ParameterType);
        }

        il.CallOrVirtualEx(method);

        var localVarStart = method.IsStatic ? 0 : 1;
        for (var i = 0; i < prams.Length; i++)
        {
            var paramType = prams[i].ParameterType;
            if (!paramType.IsByRef) continue;
            var refType = paramType.GetElementType()!;
            il.EmitEx(OpCodes.Ldarg_1)
                .EmitFastInt(i)
                .EmitEx(OpCodes.Ldloc, i + localVarStart);
            if (refType.IsValueType)
                il.EmitEx(OpCodes.Box, refType);
            il.EmitEx(OpCodes.Stelem_Ref);
        }

        if (method.ReturnType == typeof(void))
            il.EmitEx(OpCodes.Ldnull);
        else if (weaklyTyped)
            il.BoxIfValueType(method.ReturnType);

        il.EmitEx(OpCodes.Ret);
    }
}

#endregion