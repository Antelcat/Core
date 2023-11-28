using System.Collections.Generic;
using System.Security.Claims;
using Antelcat.Attributes;

namespace Antelcat.Core.SourceGenerators.Sample;

[ClaimSerializable]
public partial class ClaimSerializableClass
{
    [ClaimType(ClaimTypes.Role)]
    public ICollection<string> S { get; set; }
    public int Num { get; set; }
    
    public string Str { get; set; } = string.Empty;
    
}