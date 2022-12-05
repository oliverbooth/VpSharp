namespace VpSharp.Commands.Attributes.ExecutionChecks;

/// <summary>
///     Specifies that this command can only be run by bots.
/// </summary>
public sealed class RequireUserIdAttribute : PreExecutionCheckAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequireUserIdAttribute" /> class.
    /// </summary>
    /// <param name="userIds">An array of allowed user IDs.</param>
    /// <exception cref="ArgumentNullException"><paramref name="userIds" /> is <see langword="null" />.</exception>
    public RequireUserIdAttribute(params int[] userIds)
    {
        ArgumentNullException.ThrowIfNull(userIds);
        UserIds = userIds[..];
    }

    /// <summary>
    ///     Gets a read-only view of the user IDs allowed to run this command.
    /// </summary>
    /// <value>A <see cref="IReadOnlyList{T}" /> of <see cref="string" /> representing the allowed user IDs.</value>
    public IReadOnlyList<int> UserIds { get; }

    /// <inheritdoc />
    protected internal override Task<bool> PerformAsync(CommandContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return Task.FromResult(UserIds.Contains(context.Avatar.User.Id));
    }
}
