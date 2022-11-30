using System.Collections.Concurrent;
using System.Threading.Channels;
using VpSharp.Entities;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<string, string> _worldSettings = new();
    private Channel<VirtualParadiseWorld>? _worldListChannel = Channel.CreateUnbounded<VirtualParadiseWorld>();
    private TaskCompletionSource _worldSettingsCompletionSource = new();

    /// <summary>
    ///     Gets a world by its name.
    /// </summary>
    /// <param name="name">The name of the world.</param>
    /// <returns>
    ///     A <see cref="VirtualParadiseWorld" /> whose name is equal to <paramref name="name" />, or <see langword="null" />
    ///     if no match was found.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <paramref name="name" /> is <see langword="null" />, empty, or consists of only whitespace.
    /// </exception>
    public async Task<VirtualParadiseWorld?> GetWorldAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(ExceptionMessages.WorldNameCannotBeEmpty, nameof(name));
        }

        await foreach (VirtualParadiseWorld world in EnumerateWorldsAsync())
        {
            if (string.Equals(world.Name, name, StringComparison.Ordinal))
            {
                return world;
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets a read-only view of the worlds returned by the universe server. 
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{T}" /> containing <see cref="VirtualParadiseWorld" /> values.</returns>
    /// <remarks>
    ///     This method will consume the list in full before returning, and therefore can result in apparent "hang" while the
    ///     list is being fetched. For an <see cref="IAsyncEnumerable{T}" /> alternative, use
    ///     <see cref="EnumerateWorldsAsync" />.
    /// </remarks>
    /// <seealso cref="EnumerateWorldsAsync" />
    public async Task<IReadOnlyCollection<VirtualParadiseWorld>> GetWorldsAsync()
    {
        var worlds = new List<VirtualParadiseWorld>();

        await foreach (VirtualParadiseWorld world in EnumerateWorldsAsync())
        {
            worlds.Add(world);
        }

        return worlds.AsReadOnly();
    }
}
