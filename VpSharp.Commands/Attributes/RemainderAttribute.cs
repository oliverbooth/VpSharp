namespace VpSharp.Commands.Attributes;

/// <summary>
///     Indicates that a string parameter should consume the remainder of the arguments as one string.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class RemainderAttribute : Attribute;
