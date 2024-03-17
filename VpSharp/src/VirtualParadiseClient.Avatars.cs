using System.Collections.Concurrent;
using VpSharp.Entities;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp;

// TODO temporarily... I should probably honour this analyzer at some point
#pragma warning disable CA1506

public sealed partial class VirtualParadiseClient
{
    private readonly ConcurrentDictionary<int, Avatar> _avatars = new();

    /// <summary>
    ///     Gets the avatar with the specified session.
    /// </summary>
    /// <param name="session">The session of the avatar to get.</param>
    /// <returns>
    ///     The avatar whose session is equal to <paramref name="session" />, or <see langword="null" /> if no match was
    ///     found.
    /// </returns>
    public Avatar? GetAvatar(int session)
    {
        _avatars.TryGetValue(session, out Avatar? avatar);
        return avatar;
    }

    private Avatar AddOrUpdateAvatar(Avatar avatar)
    {
        return _avatars.AddOrUpdate(avatar.Session, avatar, (_, existing) =>
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            existing ??= new Avatar(this, avatar.Session);
            existing.Name = avatar.Name;
            existing.Location = avatar.Location;
            existing.Application = avatar.Application;
            existing.Type = avatar.Type;
            existing.UserId = avatar.UserId;
            return existing;
        });
    }

    private Avatar ExtractAvatar(nint sender)
    {
        lock (Lock)
        {
            double x = vp_double(sender, FloatAttribute.AvatarX);
            double y = vp_double(sender, FloatAttribute.AvatarY);
            double z = vp_double(sender, FloatAttribute.AvatarZ);
            var pitch = (float)vp_double(sender, FloatAttribute.AvatarPitch);
            var yaw = (float)vp_double(sender, FloatAttribute.AvatarYaw);

            var position = new Vector3d(x, y, z);
            var rotation = Rotation.CreateFromTiltYawRoll(pitch, yaw, 0);

            string applicationName = vp_string(sender, StringAttribute.AvatarApplicationName);
            string applicationVersion = vp_string(sender, StringAttribute.AvatarApplicationVersion);

            int session = vp_int(sender, IntegerAttribute.AvatarSession);
            return new Avatar(this, session)
            {
                Name = vp_string(sender, StringAttribute.AvatarName),
                Location = new Location(CurrentWorld!, position, rotation),
                Application = new Application(applicationName, applicationVersion),
                UserId = vp_int(sender, IntegerAttribute.UserId)
            };
        }
    }
}
