using System.Drawing;
using VpSharp.Entities;

namespace VpSharp.Commands;

/// <summary>
///     Provides metadata about a command invocation.
/// </summary>
public sealed class CommandContext
{
    private readonly VirtualParadiseClient _client;

    internal CommandContext(VirtualParadiseClient client, VirtualParadiseAvatar avatar, string commandName, string alias,
        string rawArguments)
    {
        _client = client;
        Avatar = avatar;
        CommandName = commandName;
        Alias = alias;
        RawArguments = rawArguments;
        Arguments = rawArguments.Split();
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
    public string[] Arguments { get; }

    /// <summary>
    ///     Gets the avatar who executed the command.
    /// </summary>
    /// <value>The executing avatar.</value>
    public VirtualParadiseAvatar Avatar { get; }

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
    public Task<VirtualParadiseMessage> RespondAsync(string message, bool ephemeral = false)
    {
        return ephemeral
            ? Avatar.SendMessageAsync(_client.CurrentAvatar?.Name, message, FontStyle.Regular, Color.Black)
            : _client.SendMessageAsync(message);
    }
}
