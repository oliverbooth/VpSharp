using VpSharp.Entities;

namespace VpSharp.EventData;

/// <summary>
///     Provides event arguments for <see cref="VirtualParadiseClient.AvatarTypeChanged" />.
/// </summary>
public sealed class AvatarTypeChangedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AvatarTypeChangedEventArgs" /> class.
    /// </summary>
    /// <param name="avatar">The avatar whose type was changed.</param>
    /// <param name="typeAfter">The avatar's new type.</param>
    /// <param name="typeBefore">The avatar's old type.</param>
    public AvatarTypeChangedEventArgs(Avatar avatar, int typeAfter, int? typeBefore)
    {
        Avatar = avatar;
        TypeAfter = typeAfter;
        TypeBefore = typeBefore;
    }

    /// <summary>
    ///     Gets the avatar whose type was changed.
    /// </summary>
    /// <value>The avatar whose type was changed.</value>
    public Avatar Avatar { get; }

    /// <summary>
    ///     Gets the avatar's type after the change.
    /// </summary>
    /// <value>The avatar's new type.</value>
    public int TypeAfter { get; }

    /// <summary>
    ///     Gets the avatar's type before the change.
    /// </summary>
    /// <value>The avatar's old type.</value>
    public int? TypeBefore { get; }
}
