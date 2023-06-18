#if NETLEGACY

namespace System.Diagnostics.CodeAnalysis;

/// <summary>Indicates that certain members on a specified <see cref="T:System.Type" /> are accessed dynamically, for example, through <see cref="N:System.Reflection" />.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, Inherited = false)]
internal sealed class DynamicallyAccessedMembersAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute" /> class with the specified member types.</summary>
    /// <param name="memberTypes">The types of the dynamically accessed members.</param>
    public DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes) => this.MemberTypes = memberTypes;

    /// <summary>Gets the <see cref="T:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes" /> that specifies the type of dynamically accessed members.</summary>
    public DynamicallyAccessedMemberTypes MemberTypes { get; }
}

/// <summary>Indicates that the specified method requires dynamic access to code that is not referenced statically, for example, through <see cref="N:System.Reflection" />.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
internal sealed class RequiresUnreferencedCodeAttribute : Attribute
{
    /// <summary>Initializes a new instance of the <see cref="System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute" /> class with the specified message.</summary>
    /// <param name="message">A message that contains information about the usage of unreferenced code.</param>
    public RequiresUnreferencedCodeAttribute(string message)
    {
        this.Message = message;
    }

    /// <summary>Gets a message that contains information about the usage of unreferenced code.</summary>
    public string Message { get; }

    /// <summary>Gets or sets an optional URL that contains more information about the method, why it requires unreferenced code, and what options a consumer has to deal with it.</summary>
    public string? Url { get; set; }
}
#endif