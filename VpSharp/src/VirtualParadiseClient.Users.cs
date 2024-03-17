using System.Collections.Concurrent;
using VpSharp.Entities;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<User>> _usersCompletionSources = new();

    /// <summary>
    ///     Gets a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to get.</param>
    /// <returns>
    ///     The user whose ID is equal to <paramref name="userId" />, or <see langword="null" /> if no match was found.
    /// </returns>
    public async Task<User> GetUserAsync(int userId)
    {
        if (_users.TryGetValue(userId, out User? user))
        {
            return user;
        }

        if (_usersCompletionSources.TryGetValue(userId, out TaskCompletionSource<User>? taskCompletionSource))
        {
            return await taskCompletionSource.Task.ConfigureAwait(false);
        }

        taskCompletionSource = new TaskCompletionSource<User>();
        _usersCompletionSources.TryAdd(userId, taskCompletionSource);

        lock (Lock)
        {
            _ = vp_user_attributes_by_id(NativeInstanceHandle, userId);
        }

        user = await taskCompletionSource.Task.ConfigureAwait(false);
        user = AddOrUpdateUser(user);

        _usersCompletionSources.TryRemove(userId, out TaskCompletionSource<User> _);
        return user;
    }

    private User AddOrUpdateUser(User user)
    {
        return _users.AddOrUpdate(user.Id, user, (_, existing) =>
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            existing ??= new User(this, user.Id);
            existing.Name = user.Name;
            existing.EmailAddress = user.EmailAddress;
            existing.LastLogin = user.LastLogin;
            existing.OnlineTime = user.OnlineTime;
            existing.RegistrationTime = user.RegistrationTime;
            return existing;
        });
    }
}
