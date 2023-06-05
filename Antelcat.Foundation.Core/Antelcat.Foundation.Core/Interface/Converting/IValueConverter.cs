namespace Antelcat.Foundation.Core.Interface.Converting;

public interface IValueConverter<TIn, TOut>
{
    TOut? To(TIn? input);
    TIn? From(TOut? input);
}

public interface IValueConverter : IValueConverter<object, object> { }