// ReSharper disable CheckNamespace
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class CompilerFeatureRequiredAttribute : Attribute
{
    public const string RefStructs = nameof(RefStructs);

    public const string RequiredMembers = nameof(RequiredMembers);

    public string FeatureName { get; }

    public bool IsOptional { get; init; }

    public CompilerFeatureRequiredAttribute(string featureName)
    {
        FeatureName = featureName;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class RequiredMemberAttribute : Attribute { }

public sealed class IsExternalInit{}