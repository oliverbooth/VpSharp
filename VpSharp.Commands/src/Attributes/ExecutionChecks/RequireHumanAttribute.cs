namespace VpSharp.Commands.Attributes.ExecutionChecks;

/// <summary>
///     Specifies that this command cannot be run by bots.
/// </summary>
public sealed class RequireHumanAttribute : PreExecutionCheckAttribute
{
    /// <inheritdoc />
    protected internal override Task<bool> PerformAsync(CommandContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return Task.FromResult(!context.Avatar.IsBot);
    }
}
