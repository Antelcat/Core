using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Antelcat.Attributes;
using Antelcat.Core.SourceGenerators.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Antelcat.Core.SourceGenerators.Tests;

public class SampleIncrementalSourceGeneratorTests
{
    private const string VectorClassText = @"
using Antelcat.Attributes;

namespace Antelcat.Core.SourceGenerators.Sample;

[ClaimSerializable]
public partial class ClaimSerializableClass
{
    public int Num { get; set; }
    
    public string Str { get; set; } = string.Empty;
}

";

    [Fact]
    public void GenerateReportMethod()
    {
        var list = new List<(string Key, string Value)>()
        {
            ("a", "a"), ("a", "b")
        };
        var r =
            list.Aggregate(new Dictionary<string, ICollection<string>>(), (d, t) =>
            {
                if (!d.TryGetValue(t.Key, out var value))
                {
                    value    = new List<string>();
                    d[t.Key] = value;
                }

                value.Add(t.Value);
                return d;
            });
        var s    = JsonSerializer.Serialize('a');
        var path = Path.GetFullPath(@"..\..\..\..\");
        // Create an instance of the source generator.
        var generator = new ClaimSerializeGenerator();

        // Source generators should be tested using 'GeneratorDriver'.
        var driver = CSharpGeneratorDriver.Create(generator);

        // We need to create a compilation with the required source code.
        var compilation = CSharpCompilation.Create(nameof(SampleIncrementalSourceGeneratorTests),
            new[]
            {
                CSharpSyntaxTree.ParseText(File.ReadAllText(
                    Path.Combine(path, @"Antelcat.Core.SourceGenerators.Sample\ClaimSerializableClass.cs")))
            },
            new[]
            {
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ClaimSerializableAttribute).Assembly.Location)
            });

        // Run generators and retrieve all results.
        var runResult = driver.RunGenerators(compilation).GetRunResult();

        // All generated files can be found in 'RunResults.GeneratedTrees'.
        var generatedFileSyntax = runResult.GeneratedTrees.Single(t => t.FilePath.EndsWith("Vector3.g.cs"));
    }
}