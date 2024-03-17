using System.Collections.ObjectModel;
using System.Drawing;
using VpSharp.Entities;

namespace VpSharp.Commands;

/// <summary>
///     Provides metadata about a command invocation.
/// </summary>
public sealed class CommandContext
{
    internal CommandContext(VirtualParadiseClient client, IAvatar avatar, string commandName, string alias,
        string rawArguments)
    {
        Client = client;
        Avatar = avatar;
        CommandName = commandName;
        Alias = alias;
        RawArguments = rawArguments;
        Arguments = new ReadOnlyCollection<string>(rawArguments.Split());
    }

    /// <summary>
    ///     Gets the alias that was used to invoke the command.
    /// </summary>
    /// <value>The alias used.</value>
    public string Alias { get; }

    /// <summary>
    ///     Gets the arguments of the command.
    /// </summary>
    /// <value>The arguments passed by the avatar.</value>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    ///     Gets the avatar who executed the command.
    /// </summary>
    /// <value>The executing avatar.</value>
    public IAvatar Avatar { get; }

    /// <summary>
    ///     Gets the client which raised the event.
    /// </summary>
    /// <value>The Virtual Paradise client.</value>
    public VirtualParadiseClient Client { get; }

    /// <summary>
    ///     Gets the command name.
    /// </summary>
    /// <value>The name of the command being executed.</value>
    public string CommandName { get; }

    /// <summary>
    ///     Gets the raw argument string as sent by the avatar.
    /// </summary>
    /// <value>The raw argument string.</value>
    public string RawArguments { get; }

    /// <summary>
    ///     Sends a response message to the command.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="ephemeral">
    ///     <see langword="true" /> to respond only to the avatar which sent the command; <see langword="false" /> to send a
    ///     regular chat message.
    /// </param>
    /// <returns>The message which was sent.</returns>
    public async Task<IMessage> RespondAsync(string message, bool ephemeral = false)
    {
        return ephemeral
            ? await Avatar.SendMessageAsync(Client.CurrentAvatar?.Name, message, FontStyle.Regular, Color.Black)
                .ConfigureAwait(false)
            : await Client.SendMessageAsync(message).ConfigureAwait(false);
    }
}
