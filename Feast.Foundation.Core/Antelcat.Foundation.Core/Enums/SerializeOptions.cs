namespace Antelcat.Foundation.Core.Enums
{
    [Flags]
    public enum SerializeOptions
    {
        None           = 0b00,
        EnumToString   = 0b01,
        DateTimeToLong = 0b10,
        All            = 0b11,
    }
}
