using VpSharp.Internal;
using static VpSharp.Internal.NativeAttributes.StringAttribute;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp.Entities;

/// <summary>
///     Provides mutability for <see cref="VirtualParadiseObject" />.
/// </summary>
public sealed class VirtualParadiseModelObjectBuilder : VirtualParadiseObjectBuilder
{
    internal VirtualParadiseModelObjectBuilder(
        VirtualParadiseClient client,
        VirtualParadiseModelObject targetObject,
        ObjectBuilderMode mode
    )
        : base(client, targetObject, mode)
    {
    }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Action</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Action</c> field, or <see langword="default" /> to leave unchanged.</value>
    public Optional<string> Action { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Description</c> field, or <see langword="default" /> to leave unchanged.</value>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Model</c> field, or <see langword="default" /> to leave unchanged.</value>
    public Optional<string> Model { get; set; }

    internal override void ApplyChanges()
    {
        base.ApplyChanges();

        nint handle = Client.NativeInstanceHandle;
        var targetObject = (VirtualParadiseModelObject)TargetObject;
        vp_string_set(handle, ObjectModel, Model.HasValue ? Model.Value! : targetObject.Model);
        vp_string_set(handle, ObjectDescription, Description.HasValue ? Description.Value! : targetObject.Description);
        vp_string_set(handle, ObjectAction, Action.HasValue ? Action.Value! : targetObject.Action);
    }
}
