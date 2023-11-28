using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using Antelcat.Attributes;
using Feast.CodeAnalysis.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using TypeInfo = Feast.CodeAnalysis.Utils.TypeInfo;

namespace Antelcat.Core.SourceGenerators.Generators;

[Generator]
public class ClaimSerializeGenerator : IIncrementalGenerator
{
    private const string Attribute          = "Antelcat.Attributes.ClaimSerializableAttribute";
    private const string Claim              = "global::System.Security.Claims.Claim";
    private const string IClaimSerializable = $"global::Antelcat.Interfaces.{nameof(IClaimSerializable)}";
    private const string JsonSerializer     = "global::System.Text.Json.JsonSerializer";
    private const string IEnumerable        = "global::System.Collections.Generic.IEnumerable";
    private const string ICollection        = "global::System.Collections.Generic.ICollection";
    private const string Dictionary         = "global::System.Collections.Generic.Dictionary";
    private const string List               = "global::System.Collections.Generic.List";
    private static string GetClaimValueTypes(ITypeSymbol symbol)
    {
        return symbol.SpecialType switch
        {
            SpecialType.System_Boolean =>
                "global::System.Security.Claims.ClaimValueTypes.Boolean",
            >= SpecialType.System_SByte and <= SpecialType.System_Decimal =>
                "global::System.Security.Claims.ClaimValueTypes.Integer",
            SpecialType.System_String or SpecialType.System_Char =>
                "global::System.Security.Claims.ClaimValueTypes.String",
            _ => "null"
        };
    }

    private static string GeneratePropToClaim(
        string claimType,
        ITypeSymbol propertyType,
        string propertyName)
    {
        if (propertyType.IsJsonBool() || propertyType.IsJsonNumber() || propertyType.IsJsonString())
        {
            return $"yield return new {Claim}({claimType}, $\"{{{propertyName}}}\", {GetClaimValueTypes(propertyType)});";
        }

        var enumerable = (RuntimeTypeInfo)typeof(IEnumerable<>);
        if (TypeInfo.FromSymbol(propertyType).IsAssignableTo(enumerable))
        {
            var comp = propertyType
                .Interfaces
                .FirstOrDefault(x =>
                    x.GetFullyQualifiedName() == enumerable.FullName
                    && x.TypeArguments is [INamedTypeSymbol]) ?? (propertyType as INamedTypeSymbol)!;
            var argument = (comp.TypeArguments[0] as INamedTypeSymbol)!;
            
            return $$"""
                   foreach (var item in {{propertyName}})
                   {
                       {{GeneratePropToClaim(claimType, argument, "item")}}
                   }
                   """;
        }
        return $"yield return new {Claim}({claimType}, {JsonSerializer}.Serialize({propertyName}));";
    }

    private static string GenerateClaimToProp(
        string argumentName,
        ITypeSymbol propertyType)
    {
        if (propertyType.SpecialType == SpecialType.System_String)
        {
            return $"{argumentName}";
        }
        
        if (propertyType.IsJsonBool() || propertyType.IsJsonNumber() || propertyType.IsJsonString())
        {
            return $"{propertyType.GetFullyQualifiedName()}.Parse({argumentName})";
        }
   
        var enumerable = (RuntimeTypeInfo)typeof(IEnumerable<>);
        if (TypeInfo.FromSymbol(propertyType).IsAssignableTo(enumerable))
        {
            var comp = propertyType
                .Interfaces
                .FirstOrDefault(x =>
                    x.GetFullyQualifiedName() == enumerable.FullName
                    && x.TypeArguments is [INamedTypeSymbol]) ?? (propertyType as INamedTypeSymbol)!;
            var argument = (comp.TypeArguments[0] as INamedTypeSymbol)!;
            var suffix = propertyType.SpecialType switch
            {
                SpecialType.System_Array => ".ToArray()",
                _                        => ".ToList()"
            };
            
            return $$"""
                   pair.Value.Select(x=> {{GenerateClaimToProp("x.Value", argument)}}){{suffix}}
                   """;
        }
        
        return $"{JsonSerializer}.Deserialize<{propertyType.GetFullyQualifiedName()}>({argumentName})";
    }
    

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                Attribute,
                (s, _) => s is ClassDeclarationSyntax,
                (ctx, _) => (ctx.TargetNode as ClassDeclarationSyntax)!);

        context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
            (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
    }

    private void GenerateCode(SourceProductionContext context, Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classDeclarations)
    {
        // Go through all filtered class declarations.
        foreach (var classDeclarationSyntax in classDeclarations)
        {
            // We need to get semantic model of the class to retrieve metadata.
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            // Symbols allow us to get the compile-time information.
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
                continue;

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            // 'Identifier' means the token of the node. Get class name from the syntax node.
            var className = classDeclarationSyntax.Identifier.Text;

            // Go through all class members with a particular type (property) to generate method lines.
            var transform = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Select(p =>
                {
                    var canRead   = !p.IsWriteOnly;
                    var canWrite  = !p.IsReadOnly;
                    var claimType = $"nameof({p.Name})";
                    var propName  = $"this.{p.Name}";
                    var attrs     = p.GetAttributes().ToList();
                    var attr = attrs.FirstOrDefault(x =>
                        x.AttributeClass.GetFullyQualifiedName() == $"global::{typeof(ClaimTypeAttribute).FullName}");
                    if (attr is { ConstructorArguments.Length: 1 })
                    {
                        claimType = $"\"{attr.ConstructorArguments[0].Value}\"";
                    }

                    return (claimType, p.Type, propName, canRead, canWrite);
                }).ToList();
            
            var propToClaims = transform
                .Where(x=>x.canRead)
                .Select(p => GeneratePropToClaim(p.claimType, p.Type, p.propName));

            var claimsToProps = transform
                .Where(x => x.canWrite)
                .Select(p => $"case {p.claimType} : {p.propName} = "                 +
                             GenerateClaimToProp("pair.Value.First().Value", p.Type) +
                             "; break;");

            
            // Build up the source code
            var code = $$"""
                         // <auto-generated/> By Antelcat.Core.SourceGenerators

                         using System;
                         using System.Collections.Generic;
                         using System.Linq;

                         namespace {{namespaceName}};

                         partial class {{className}} : {{IClaimSerializable}}
                         {
                             public {{IEnumerable}}<{{Claim}}> {{nameof(Interfaces.IClaimSerializable.ToClaims)}}()
                             {
                                 {{string.Join("\n", propToClaims).Replace("\n","\n\t\t")}}
                             }
                             
                             public void {{nameof(Interfaces.IClaimSerializable.FromClaims)}}({{IEnumerable}}<{{Claim}}> claims)
                             {
                                 foreach (var pair in claims.Aggregate(new {{Dictionary}}<string, {{ICollection}}<{{Claim}}>>(), (d, c) =>
                                    {
                                        if (!d.TryGetValue(c.Type, out var value))
                                        {
                                            value     = new {{List}}<{{Claim}}>();
                                            d[c.Type] = value;
                                        }
                         
                                        value.Add(c);
                                        return d;
                                    }))
                                 {
                                     switch (pair.Key)
                                     {
                                         {{string.Join("\n", claimsToProps).Replace("\n","\n\t\t\t\t")}}
                                     }
                                 }
                             }
                         }

                         """;

            // Add the source code to the compilation.
            context.AddSource($"{className}.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }
}