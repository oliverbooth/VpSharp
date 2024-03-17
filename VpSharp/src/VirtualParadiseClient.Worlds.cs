using System.Collections.Concurrent;
using System.Threading.Channels;
using VpSharp.Entities;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<string, string> _worldSettings = new();
    private readonly ConcurrentDictionary<string, World> _worlds = new();
    private Channel<World>? _worldListChannel = Channel.CreateUnbounded<World>();
    private TaskCompletionSource _worldSettingsCompletionSource = new();

    /// <summary>
    ///     Gets an enumerable of the worlds returned by the universe server.
    /// </summary>
    /// <returns>An <see cref="IAsyncEnumerable{T}" /> containing <see cref="World" /> values.</returns>
    /// <remarks>
    ///     This method will yield results back as they are received from the world server. To access a consumed collection,
    ///     use <see cref="GetWorldsAsync" />.
    /// </remarks>
    /// <seealso cref="GetWorldsAsync" />
    public IAsyncEnumerable<World> EnumerateWorldsAsync()
    {
        _worldListChannel = Channel.CreateUnbounded<World>();
        lock (Lock)
        {
            _ = vp_world_list(NativeInstanceHandle, 0);
        }

        return _worldListChannel.Reader.ReadAllAsync();
    }

    /// <summary>
    ///     Gets a world by its name.
    /// </summary>
    /// <param name="name">The name of the world.</param>
    /// <returns>
    ///     A <see cref="World" /> whose name is equal to <paramref name="name" />, or <see langword="null" />
    ///     if no match was found.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <paramref name="name" /> is <see langword="null" />, empty, or consists of only whitespace.
    /// </exception>
    public async Task<World?> GetWorldAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(ExceptionMessages.WorldNameCannotBeEmpty, nameof(name));
        }

        await foreach (World world in EnumerateWorldsAsync())
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
    /// <returns>An <see cref="IReadOnlyCollection{T}" /> containing <see cref="World" /> values.</returns>
    /// <remarks>
    ///     This method will consume the list in full before returning, and therefore can result in apparent "hang" while the
    ///     list is being fetched. For an <see cref="IAsyncEnumerable{T}" /> alternative, use
    ///     <see cref="EnumerateWorldsAsync" />.
    /// </remarks>
    /// <seealso cref="EnumerateWorldsAsync" />
    public async Task<IReadOnlyCollection<World>> GetWorldsAsync()
    {
        var worlds = new List<World>();

        await foreach (World world in EnumerateWorldsAsync())
        {
            worlds.Add(world);
        }

        return worlds.AsReadOnly();
    }
}
