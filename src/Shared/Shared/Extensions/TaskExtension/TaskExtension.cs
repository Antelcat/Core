using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Antelcat.Extensions;
#nullable enable
public static partial class TaskExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TaskAwaiter GetAwaiter(this int delay) => 
	    Task.Delay(delay).GetAwaiter();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task RunAsync(this Action action, CancellationToken? token = null) =>
	    Task.Run(action, token ?? default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<T> RunAsync<T>(this Func<T> func, CancellationToken? token = null) =>
	    Task.Run(func, token ?? default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TaskAwaiter GetAwaiter(this Action action) => 
	    action.RunAsync().GetAwaiter();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TaskAwaiter<T> GetAwaiter<T>(this Func<T> func) => 
	    func.RunAsync().GetAwaiter();

	public static void Detach(this Task task, Action<Exception>? exceptionHandler = null) 
	{
		if (exceptionHandler == null) 
		{
			var ctx = SynchronizationContext.Current;

			if (ctx == null) 
			{
				ctx = new SynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(ctx);
			}

			task.ContinueWith(t => 
			{
				if (t.Exception != null) 
				{
					ctx.Send(_ => throw t.Exception, null);
				}
			});
		} 
		else 
		{
			task.ContinueWith(t => 
			{
				if (t.Exception != null) 
				{
                    exceptionHandler.Invoke(t.Exception);
				}
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Detach<T>(this Task<T> task, Action<Exception>? exceptionHandler = null) => Detach((Task)task, exceptionHandler);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Detach(this ValueTask task, Action<Exception>? exceptionHandler = null) => Detach(task.AsTask(), exceptionHandler);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Detach<T>(this ValueTask<T> task, Action<Exception>? exceptionHandler = null) => Detach((Task)task.AsTask(), exceptionHandler);
	
	public static async Task WithTimeout(this Task task, TimeSpan timeout, Action? cancelled = null)
	{
		using var timerCancellation = new CancellationTokenSource();
		var timeoutTask = Task.Delay(timeout, timerCancellation.Token);
		var firstCompletedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);
		if (firstCompletedTask == timeoutTask)
		{
			if (cancelled == null)
			{
				throw new TimeoutException();
			}
			cancelled();
		}
		timerCancellation.Cancel();
		await task.ConfigureAwait(false);
	}
	
	public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, Action? cancelled = null)
	{
		await WithTimeout((Task)task, timeout, cancelled).ConfigureAwait(false);
		return task.GetAwaiter().GetResult();
	}
	
	public static TaskCompletionSource<TResult> WithTimeout<TResult>(this TaskCompletionSource<TResult> taskCompletionSource, TimeSpan timeout)
	{
		return WithTimeout(taskCompletionSource, timeout, null);
	}

	public static TaskCompletionSource<TResult> WithTimeout<TResult>(this TaskCompletionSource<TResult> taskCompletionSource, TimeSpan timeout, Action? cancelled)
	{
		Timer? timer = null;
		timer = new Timer(_ =>
		{
			timer?.Dispose();
			if (taskCompletionSource.Task.Status == TaskStatus.RanToCompletion) return;
			taskCompletionSource.TrySetCanceled();
			cancelled?.Invoke();
		}, null, timeout, TimeSpan.FromMilliseconds(-1));

		return taskCompletionSource;
	}


}