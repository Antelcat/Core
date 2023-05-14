using System.Reflection;
using System.Reflection.Emit;

namespace Feast.Foundation.Core.Structs
{
    public readonly struct IlInvoker
    {
        public delegate object Handler(object? target, params object?[]? parameters);
        public readonly MethodInfo Method;
        public readonly Handler Invoker;
        public IlInvoker(MethodInfo method)
        {
            Method = method;
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new[] { typeof(object), typeof(object[]) });
            var il = dynamicMethod.GetILGenerator();
            var ps = method.GetParameters();
            var paramTypes = new Type[ps.Length];
            for (var i = 0; i < paramTypes.Length; i++)
            {
                paramTypes[i] = ps[i].ParameterType.IsByRef
                    ? ps[i].ParameterType.GetElementType() ?? typeof(void)
                    : ps[i].ParameterType;
            }
            var locals = new LocalBuilder[paramTypes.Length];
            for (var i = 0; i < paramTypes.Length; i++)
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            for (var i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(paramTypes[i].IsValueType
                    ? OpCodes.Unbox_Any
                    : OpCodes.Castclass, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!method.IsStatic)
                il.Emit(OpCodes.Ldarg_0);
            for (var i = 0; i < paramTypes.Length; i++)
                il.Emit(ps[i].ParameterType.IsByRef
                    ? OpCodes.Ldloca_S
                    : OpCodes.Ldloc, locals[i]);
            il.EmitCall(method.IsStatic
                ? OpCodes.Call
                : OpCodes.Callvirt, method, null);

            if (method.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else if (method.ReturnType.IsValueType)
                il.Emit(OpCodes.Box, method.ReturnType);
            for (var i = 0; i < paramTypes.Length; i++)
            {
                if (!ps[i].ParameterType.IsByRef) continue;
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldloc, locals[i]);
                if (locals[i].LocalType.IsValueType) 
                    il.Emit(OpCodes.Box, locals[i].LocalType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Ret);
            Invoker = dynamicMethod.CreateDelegate<Handler>();
        }
        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;

                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;

                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;

                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;

                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;

                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;

                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;

                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;

                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;

                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }
            il.Emit(value is > -129 and < 128 
                ? OpCodes.Ldc_I4_S 
                : OpCodes.Ldc_I4, value);
        }

        public static implicit operator IlInvoker(MethodInfo method) => new (method);
    }
}
