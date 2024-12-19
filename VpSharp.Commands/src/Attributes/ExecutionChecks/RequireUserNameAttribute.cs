﻿using VpSharp.Entities;

namespace VpSharp.Commands.Attributes.ExecutionChecks;

#pragma warning disable CA1019 // Define accessors for attribute arguments

/// <summary>
///     Specifies that this command can only be run by bots.
/// </summary>
public sealed class RequireUserNameAttribute : PreExecutionCheckAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequireUserNameAttribute" /> class.
    /// </summary>
    /// <param name="names">An enumerable collection of allowed user names.</param>
    /// <exception cref="ArgumentNullException"><paramref name="names" /> is <see langword="null" />.</exception>
    public RequireUserNameAttribute(IEnumerable<string> names)
    {
        if (names is null)
        {
            throw new ArgumentNullException(nameof(names));
        }

        Names = names.ToArray();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RequireUserNameAttribute" /> class.
    /// </summary>
    /// <param name="names">An array of allowed user names.</param>
    /// <exception cref="ArgumentNullException"><paramref name="names" /> is <see langword="null" />.</exception>
    [CLSCompliant(false)]
    public RequireUserNameAttribute(params string[] names)
    {
        if (names is null)
        {
            throw new ArgumentNullException(nameof(names));
        }

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
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        VirtualParadiseUser user = context.Avatar.User;
        return Task.FromResult(Names.Contains(user.Name));
    }
}