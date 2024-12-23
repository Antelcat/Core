namespace Antelcat.Core.Extensions;

public static class AndExtension
{
    public static T AndAddTo<T>(this T o, ICollection<T> collection) where T : notnull
    {
        collection.Add(o);
        return o;
    }

    public static T AndAddTo<T>(this T o, ICollection<object> collection) where T : notnull
    {
        collection.Add(o);
        return o;
    }

    public static T AndAddTo<T>(this T o, IDictionary<string, T> dictionary, string name)
    {
        dictionary.Add(name, o);
        return o;
    }

    public static T AndAddTo<T>(this T o, IDictionary<string, object?> dictionary, string name)
    {
        dictionary.Add(name, o);
        return o;
    }
}