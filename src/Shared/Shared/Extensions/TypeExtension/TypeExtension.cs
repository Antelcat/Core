#if !NET && !NETSTANDARD
using System;
#endif

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Antelcat.Extensions;

public static partial class TypeExtension
{
    /// <summary>
    /// 创建未初始化的对象
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="SerializationException"></exception>
    public static object RawInstance(this Type type) => RuntimeHelpers.GetUninitializedObject(type);
    /// <summary>
    /// 创建未初始化的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T RawInstance<T>() => (T)RuntimeHelpers.GetUninitializedObject(typeof(T));
    /// <summary>
    /// 创建未初始化的对象
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="SerializationException"></exception>
    public static T RawInstance<T>(this Type type) => (T)RuntimeHelpers.GetUninitializedObject(type);
    /// <summary>
    /// 创建一个对象
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object? NewInstance(this Type type) => Activator.CreateInstance(type);
    /// <summary>
    /// 创建一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? NewInstance<T>() => Activator.CreateInstance<T>();
    /// <summary>
    /// 创建一个对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? NewInstance<T>(this Type type) => (T?)Activator.CreateInstance(type);
}