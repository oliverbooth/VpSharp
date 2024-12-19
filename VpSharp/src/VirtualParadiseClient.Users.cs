using System.Collections.Concurrent;
using VpSharp.Entities;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<int, VirtualParadiseUser> _users = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<VirtualParadiseUser>> _usersCompletionSources = new();

    /// <summary>
    ///     Gets a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to get.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     The user whose ID is equal to <paramref name="userId" />, or <see langword="null" /> if no match was found.
    /// </returns>
    public async Task<VirtualParadiseUser?> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (_users.TryGetValue(userId, out VirtualParadiseUser? user))
        {
            return user;
        }

        if (_usersCompletionSources.TryGetValue(userId, out TaskCompletionSource<VirtualParadiseUser>? taskCompletionSource))
        {
            try
            {
                await using (cancellationToken.Register(() => taskCompletionSource.TrySetCanceled()))
                {
                    return await taskCompletionSource.Task;
                }
            }
            catch (TaskCanceledException)
            {
                _usersCompletionSources.TryRemove(userId, out _);
                return null;
            }
        }

        taskCompletionSource = new TaskCompletionSource<VirtualParadiseUser>();
        _usersCompletionSources.TryAdd(userId, taskCompletionSource);

        lock (Lock)
        {
            _ = vp_user_attributes_by_id(NativeInstanceHandle, userId);
        }

        try
        {
            await using (cancellationToken.Register(() => taskCompletionSource.TrySetCanceled()))
            {
                user = await taskCompletionSource.Task;
                user = AddOrUpdateUser(user);
            }
        }
        catch (TaskCanceledException)
        {
            _usersCompletionSources.TryRemove(userId, out _);
            return null;
        }

        _usersCompletionSources.TryRemove(userId, out _);
        return user;
    }

    private VirtualParadiseUser AddOrUpdateUser(VirtualParadiseUser user)
    {
        return _users.AddOrUpdate(user.Id, user, (_, existing) =>
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            existing ??= new VirtualParadiseUser(this, user.Id);
            existing.Name = user.Name;
            existing.EmailAddress = user.EmailAddress;
            existing.LastLogin = user.LastLogin;
            existing.OnlineTime = user.OnlineTime;
            existing.RegistrationTime = user.RegistrationTime;
            return existing;
        });
    }
}
