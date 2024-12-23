#if !NET && !NETSTANDARD
using System;
#endif
namespace Antelcat.Enums;

[Flags]
public enum SerializeOptions
{
    None           = 0b0000,
    LowerCamelCase = 0b0001,
    EnumToString   = 0b0010,
    DateTimeToLong = 0b0100,
    WriteIndented  = 0b1000,
    Default        = 0b0101,
        
    All            = 0b1111,
}