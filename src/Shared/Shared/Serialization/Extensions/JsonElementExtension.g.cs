#if !NET && !NETSTANDARD
using System;
#endif
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace Antelcat.Extensions;
#nullable enable
public static partial class JsonElementExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static sbyte? GetSByteOrNull(this JsonElement jsonElement) => jsonElement.TryGetSByte(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte? GetByteOrNull(this JsonElement jsonElement) => jsonElement.TryGetByte(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int? GetInt32OrNull(this JsonElement jsonElement) => jsonElement.TryGetInt32(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint? GetUInt32OrNull(this JsonElement jsonElement) => jsonElement.TryGetUInt32(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long? GetInt64OrNull(this JsonElement jsonElement) => jsonElement.TryGetInt64(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong? GetUInt64OrNull(this JsonElement jsonElement) => jsonElement.TryGetUInt64(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double? GetDoubleOrNull(this JsonElement jsonElement) => jsonElement.TryGetDouble(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal? GetDecimalOrNull(this JsonElement jsonElement) => jsonElement.TryGetDecimal(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float? GetSingleOrNull(this JsonElement jsonElement) => jsonElement.TryGetSingle(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime? GetDateTimeOrNull(this JsonElement jsonElement) => jsonElement.TryGetDateTime(out var result) ? result : null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid? GetGuidOrNull(this JsonElement jsonElement) => jsonElement.TryGetGuid(out var result) ? result : null;

}
