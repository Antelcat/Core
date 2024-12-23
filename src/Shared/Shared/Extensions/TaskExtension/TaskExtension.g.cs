#if !NET && !NETSTANDARD
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
#endif
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Antelcat.Extensions;

public static partial class TaskExtension
{
	public static Boolean WaitAll(this Task[] tasks, TimeSpan timeout) => Task.WaitAll(tasks, timeout);

	public static Boolean WaitAll(this Task[] tasks, Int32 millisecondsTimeout) => Task.WaitAll(tasks, millisecondsTimeout);

	public static Boolean WaitAll(this Task[] tasks, Int32 millisecondsTimeout, CancellationToken cancellationToken) => Task.WaitAll(tasks, millisecondsTimeout, cancellationToken);

	public static Int32 WaitAny(this Task[] tasks) => Task.WaitAny(tasks);

	public static Int32 WaitAny(this Task[] tasks, TimeSpan timeout) => Task.WaitAny(tasks, timeout);

	public static Int32 WaitAny(this Task[] tasks, CancellationToken cancellationToken) => Task.WaitAny(tasks, cancellationToken);

	public static Int32 WaitAny(this Task[] tasks, Int32 millisecondsTimeout) => Task.WaitAny(tasks, millisecondsTimeout);

	public static Int32 WaitAny(this Task[] tasks, Int32 millisecondsTimeout, CancellationToken cancellationToken) => Task.WaitAny(tasks, millisecondsTimeout, cancellationToken);

	public static Task FromException(this Exception exception) => Task.FromException(exception);

	public static Task FromCanceled(this CancellationToken cancellationToken) => Task.FromCanceled(cancellationToken);

	public static Task Run(this Action action) => Task.Run(action);

	public static Task Run(this Action action, CancellationToken cancellationToken) => Task.Run(action, cancellationToken);

	public static Task Run(this Func<Task> function) => Task.Run(function);

	public static Task Run(this Func<Task> function, CancellationToken cancellationToken) => Task.Run(function, cancellationToken);

	public static Task Delay(this TimeSpan delay) => Task.Delay(delay);

	public static Task Delay(this TimeSpan delay, CancellationToken cancellationToken) => Task.Delay(delay, cancellationToken);

	public static Task Delay(this Int32 millisecondsDelay) => Task.Delay(millisecondsDelay);

	public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

	public static Task WhenAll(this Task[] tasks) => Task.WhenAll(tasks);

	public static Task<Task> WhenAny(this Task[] tasks) => Task.WhenAny(tasks);

	public static Task<Task> WhenAny(this IEnumerable<Task> tasks) => Task.WhenAny(tasks);

	public static Task<TResult> FromResult<TResult>(this TResult result) => Task.FromResult<TResult>(result);

	public static Task<TResult> FromException<TResult>(this Exception exception) => Task.FromException<TResult>(exception);

	public static Task<TResult> FromCanceled<TResult>(this CancellationToken cancellationToken) => Task.FromCanceled<TResult>(cancellationToken);

	public static Task<TResult> Run<TResult>(this Func<TResult> function) => Task.Run<TResult>(function);

	public static Task<TResult> Run<TResult>(this Func<TResult> function, CancellationToken cancellationToken) => Task.Run<TResult>(function, cancellationToken);

	public static Task<TResult> Run<TResult>(this Func<Task<TResult>> function) => Task.Run<TResult>(function);

	public static Task<TResult> Run<TResult>(this Func<Task<TResult>> function, CancellationToken cancellationToken) => Task.Run<TResult>(function, cancellationToken);

	public static Task Delay(this Int32 millisecondsDelay, CancellationToken cancellationToken) => Task.Delay(millisecondsDelay, cancellationToken);

	public static Task<TResult[]> WhenAll<TResult>(this IEnumerable<Task<TResult>> tasks) => Task.WhenAll<TResult>(tasks);

	public static Task<TResult[]> WhenAll<TResult>(this Task<TResult>[] tasks) => Task.WhenAll<TResult>(tasks);

	public static Task<Task<TResult>> WhenAny<TResult>(this Task<TResult>[] tasks) => Task.WhenAny<TResult>(tasks);

	public static Task<Task<TResult>> WhenAny<TResult>(this IEnumerable<Task<TResult>> tasks) => Task.WhenAny<TResult>(tasks);

}