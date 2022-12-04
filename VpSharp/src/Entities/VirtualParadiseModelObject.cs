using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.NativeMethods;

namespace VpSharp.Entities;

/// <summary>
///     Represents an object which renders as a 3D model. A "model" object will contain a <c>Model</c>, <c>Description</c>
///     and <c>Action</c> field.
/// </summary>
public class VirtualParadiseModelObject : VirtualParadiseObject
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="VirtualParadiseModelObject" /> class.
    /// </summary>
    /// <param name="client">The owning client.</param>
    /// <param name="id">The object ID.</param>
    /// <exception cref="ArgumentNullException"><paramref name="client" /> is <see langword="null" />.</exception>
    internal VirtualParadiseModelObject(VirtualParadiseClient client, int id)
        : base(client, id)
    {
    }

    /// <summary>
    ///     Gets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Description</c> field.</value>
    public string Action { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Description</c> field.</value>
    public string Description { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Model</c> field.</value>
    public string Model { get; internal set; } = string.Empty;

    /// <summary>
    ///     Modifies the object.
    /// </summary>
    /// <param name="action">The builder which defines parameters to change.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para><see cref="VirtualParadiseModelObjectBuilder.ModificationTimestamp" /> was assigned.</para>
    ///     -or-
    ///     <para><see cref="VirtualParadiseModelObjectBuilder.Owner" /> was assigned.</para>
    /// </exception>
    public async Task ModifyAsync(Action<VirtualParadiseModelObjectBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var builder = new VirtualParadiseModelObjectBuilder(Client, this, ObjectBuilderMode.Modify);
        await Task.Run(() => action(builder)).ConfigureAwait(false);

        lock (Client.Lock)
        {
            nint handle = Client.NativeInstanceHandle;
            _ = vp_int_set(handle, IntegerAttribute.ObjectId, Id);
            builder.ApplyChanges();

            _ = vp_object_change(handle);
        }
    }

    /// <inheritdoc />
    protected internal override void ExtractFromOther(VirtualParadiseObject virtualParadiseObject)
    {
        if (virtualParadiseObject is not VirtualParadiseModelObject model)
        {
            return;
        }

        base.ExtractFromOther(virtualParadiseObject);
        Action = model.Action;
        Description = model.Description;
        Model = model.Model;
    }

    /// <inheritdoc />
    protected internal override void ExtractFromInstance(nint handle)
    {
        base.ExtractFromInstance(handle);
        Action = vp_string(handle, StringAttribute.ObjectAction);
        Description = vp_string(handle, StringAttribute.ObjectDescription);
        Model = vp_string(handle, StringAttribute.ObjectModel);
    }
}
