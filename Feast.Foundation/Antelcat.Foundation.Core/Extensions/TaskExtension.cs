using System.Runtime.CompilerServices;

namespace Antelcat.Foundation.Core.Extensions;

public static class TaskExtension
{
    public static TaskAwaiter GetAwaiter(this int delay) => 
        Task.Delay(delay).GetAwaiter();

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
}