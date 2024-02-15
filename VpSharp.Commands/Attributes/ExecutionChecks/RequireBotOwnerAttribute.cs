namespace VpSharp.Commands.Attributes.ExecutionChecks;

/// <summary>
///     Specifies that this command can only be run by the user under whom this bot is authenticated.
/// </summary>
public sealed class RequireBotOwnerAttribute : PreExecutionCheckAttribute
{
    /// <inheritdoc />
    protected internal override Task<bool> PerformAsync(CommandContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return Task.FromResult(context.Avatar.UserId == context.Client.CurrentUser?.Id);
    }
}
