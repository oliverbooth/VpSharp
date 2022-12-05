namespace VpSharp.Commands.Attributes.ExecutionChecks;

/// <summary>
///     Specifies that this command can only be run by bots.
/// </summary>
public sealed class RequireUserNameAttribute : PreExecutionCheckAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequireUserNameAttribute" /> class.
    /// </summary>
    /// <param name="names">An array of allowed user names.</param>
    /// <exception cref="ArgumentNullException"><paramref name="names" /> is <see langword="null" />.</exception>
    public RequireUserNameAttribute(params string[] names)
    {
        ArgumentNullException.ThrowIfNull(names);
        Names = names[..];
    }

    /// <summary>
    ///     Gets a read-only view of the user names allowed to run this command.
    /// </summary>
    /// <value>A <see cref="IReadOnlyList{T}" /> of <see cref="string" /> representing the allowed user names.</value>
    public IReadOnlyList<string> Names { get; }

    /// <inheritdoc />
    protected internal override Task<bool> PerformAsync(CommandContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return Task.FromResult(Names.Contains(context.Avatar.User.Name));
    }
}
