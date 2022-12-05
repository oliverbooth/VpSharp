namespace VpSharp.Commands.Attributes.ExecutionChecks;

/// <summary>
///     Represents the base class for all execution check attributes.
/// </summary>
public abstract class PreExecutionCheckAttribute : Attribute
{
    /// <summary>
    ///     Performs the execution check.
    /// </summary>
    /// <returns><see langword="true" /> if the execution check has passed; otherwise, <see langword="false" />.</returns>
    protected internal abstract Task<bool> PerformAsync(CommandContext context);
}
