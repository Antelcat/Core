using System.Text;
using Antelcat.AutoGen.ComponentModel;

namespace Antelcat.Core.Extensions;

[AutoStringTo]
internal static partial class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string? str) => string.IsNullOrWhiteSpace(str);
    
    public static StringBuilder AppendLineForEach<T>(this StringBuilder builder, 
                                                     IEnumerable<T> enumerable, 
                                                     Func<T, string> func)
    {
        foreach (var item in enumerable)
        {
            builder.AppendLine(func(item));
        }

        return builder;
    }
}