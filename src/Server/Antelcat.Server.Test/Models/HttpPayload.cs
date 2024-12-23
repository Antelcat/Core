using Antelcat.Extensions;

namespace Antelcat.Server.Test.Models;

[Serializable]
public class HttpPayload
{
	public int Code { get; set; } = 1;
	
	public string? Message { get; set; }
	
	public long Timestamp { get; set; } = TimeExtension.CurrentTimestamp;
	
	public static HttpPayload Success => new ();

	public void EnsureSuccessStatusCode()
	{
		if (Code != 1)
		{
			throw new HttpRequestException(Message);
		}
	}

	public override string ToString() =>
		$"{{\n" +
		$"	Code: {Code},\n" +
		$"	Message: \"{Message}\",\n" +
		$"	Time: \"{Timestamp.ToLocalTime():s}\"\n" +
		$"}}\n";
	
	public static implicit operator HttpPayload(string error) => new ()
	{
		Code = 0,
		Message = error,
	};
	public static implicit operator HttpPayload(Exception e) => e.Message;
	public static implicit operator HttpPayload((int, string) tuple) => new () { Code = tuple.Item1, Message = tuple.Item2 };

	public static HttpPayload FromErrorMsg(string error) => new ()
	{
		Code = 0,
		Message = error,
	};

	public static HttpPayload FromException(Exception e) => FromErrorMsg(e.Message);

	public async static Task<HttpPayload> FromTask(Task promise)
	{
		try
		{
			await promise;
			return new HttpPayload();
		}
		catch (Exception e)
		{
			return FromException(e);
		}
	}

	public async static Task<HttpPayload> FromAsync(Func<Task> asyncFunc)
	{
		try
		{
			await asyncFunc.Invoke();
			return new HttpPayload();
		}
		catch (Exception e)
		{
			return FromException(e);
		}
	}
}

[Serializable]
public class HttpPayload<T> : HttpPayload
{
	public T? Data { get; set; }

	public HttpPayload() { }

	public HttpPayload(T data)
	{
		Data = data;
	}

	public override string ToString() =>
		$"{{\n" +
		$"	Data: {Data},\n" +
		$"	Code: {Code},\n" +
		$"	Message: \"{Message}\",\n" +
		$"	Time: \"{Timestamp.ToLocalTime():s}\"\n" +
		$"}}\n";
	
	public static implicit operator HttpPayload<T>(T data) => new (data);
	public static implicit operator HttpPayload<T>(string error) => new ()
	{
		Code = 0,
		Message = error,
	};

	public static implicit operator HttpPayload<T>((T, string) data) => new () { Code = 0, Message = data.Item2 };
	public static implicit operator HttpPayload<T>((int, T, string) data) => new () { Code = data.Item1, Data = data.Item2, Message = data.Item3 };
	public static implicit operator HttpPayload<T>(Exception e) => e.Message;
	public static implicit operator HttpPayload<T>(Func<T> getter)
	{
		try
		{
			return getter();
		}
		catch (Exception e)
		{
			return e;
		}
	}

	public static HttpPayload<T> FromData(T data) => new (data);

	public new static HttpPayload<T> FromErrorMsg(string error) => new ()
	{
		Code = 0,
		Message = error,
	};

	public new static HttpPayload<T> FromException(Exception e) => FromErrorMsg(e.Message);

	public async static Task<HttpPayload<T>> FromTask(Task<T> promise)
	{
		try
		{
			return new HttpPayload<T>(await promise);
		}
		catch (Exception e)
		{
			return FromException(e);
		}
	}

	public async static Task<HttpPayload<T>> FromAsync(Func<Task<T>> asyncFunc)
	{
		try
		{
			return new HttpPayload<T>(await asyncFunc.Invoke());
		}
		catch (Exception e)
		{
			return FromException(e);
		}
	}
}