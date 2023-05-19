namespace Antelcat.Foundation.Core.Enums
{
    [Flags]
    public enum SerializeOptions
    {
        None           = 0b000,
        LowerCamelCase = 0b001,
        EnumToString   = 0b010,
        DateTimeToLong = 0b100,
        Default        = 0b101,
        
        All            = 0b111,
    }
}
