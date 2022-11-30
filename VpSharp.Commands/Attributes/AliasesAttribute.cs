using System.Collections.ObjectModel;

namespace VpSharp.Commands.Attributes;

/// <summary>
///     Defines the aliases of a command.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
[CLSCompliant(false)]
public sealed class AliasesAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AliasesAttribute" /> class.
    /// </summary>
    /// <param name="alias">The first alias.</param>
    /// <param name="aliases">Additional aliases.</param>
    /// <exception cref="ArgumentNullException">
    ///     <para><paramref name="alias" /> is <see langword="null" />.</para>
    ///     -or-
    ///     <para><paramref name="aliases" /> is <see langword="null" />.</para>
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="alias" /> is empty, or consists of only whitespace.</para>
    ///     -or-
    ///     <para>An element in <paramref name="aliases" /> is null, empty, or consists of only whitespace.</para>
    /// </exception>
#pragma warning disable CA1019
    public AliasesAttribute(string alias, params string[] aliases)
#pragma warning restore CA1019
    {
        ArgumentNullException.ThrowIfNull(alias);
        ArgumentNullException.ThrowIfNull(aliases);

        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias cannot be empty");
        }

        foreach (string a in aliases)
        {
            if (string.IsNullOrWhiteSpace(a))
            {
                throw new ArgumentException("Cannot have a null alias");
            }
        }

        var buffer = new string[aliases.Length + 1];
        buffer[0] = alias;
        Array.Copy(aliases, 0, buffer, 1, aliases.Length);
        Aliases = new ReadOnlyCollection<string>(buffer);
    }

    /// <summary>
    ///     Gets the command aliases.
    /// </summary>
    /// <value>The command aliases.</value>
    public IReadOnlyList<string> Aliases { get; }
}
