using System.Numerics;
using VpSharp.Extensions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
using static VpSharp.Internal.Native;

namespace VpSharp.Entities;

/// <summary>
///     Provides mutability for <see cref="VirtualParadiseObject" />.
/// </summary>
public sealed class VirtualParadiseModelObjectBuilder : VirtualParadiseObjectBuilder
{
    internal VirtualParadiseModelObjectBuilder(VirtualParadiseClient client, ObjectBuilderMode mode)
        : base(client, mode)
    {
    }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Action</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Action</c> field, or <see langword="null" /> to leave unchanged.</value>
    public string? Action { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Description</c> field, or <see langword="null" /> to leave unchanged.</value>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Model</c> field, or <see langword="null" /> to leave unchanged.</value>
    public string? Model { get; set; }

    /// <summary>
    ///     Gets or sets the date and time at which this object was last modified.
    /// </summary>
    /// <value>
    ///     The date and time at which this object was last modified, or <see langword="null" /> to leave unchanged.
    /// </value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public DateTimeOffset? ModificationTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the owner of this object.
    /// </summary>
    /// <value>The owner of this object, or <see langword="null" /> to leave unchanged.</value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public VirtualParadiseUser? Owner { get; set; }

    /// <summary>
    ///     Gets or sets the position of the object.
    /// </summary>
    /// <value>The position of the object, or <see langword="null" /> to leave unchanged.</value>
    public Vector3d? Position { get; set; }

    /// <summary>
    ///     Gets or sets the rotation of the object.
    /// </summary>
    /// <value>The rotation of the object, or <see langword="null" /> to leave unchanged.</value>
    public Quaternion? Rotation { get; set; }

    /// <summary>
    ///     Sets the value of this object's <c>Action</c> field.
    /// </summary>
    /// <param name="action">The new value of the <c>Action</c> field, or <see langword="null" /> to leave unchanged.</param>
    /// <returns>The current instance of this builder.</returns>
    public VirtualParadiseModelObjectBuilder WithAction(string? action)
    {
        Action = action;
        return this;
    }

    /// <summary>
    ///     Sets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <param name="description">
    ///     The new value of the <c>Description</c> field, or <see langword="null" /> to leave unchanged.
    /// </param>
    /// <returns>The current instance of this builder.</returns>
    public VirtualParadiseModelObjectBuilder WithDescription(string? description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     Sets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <param name="model">The new value of the <c>Model</c> field, or <see langword="null" /> to leave unchanged.</param>
    /// <returns>The current instance of this builder.</returns>
    public VirtualParadiseModelObjectBuilder WithModel(string? model)
    {
        Model = model;
        return this;
    }

    internal void ApplyChanges()
    {
        nint handle = Client.NativeInstanceHandle;

        if (Action is { } action)
        {
            vp_string_set(handle, StringAttribute.ObjectAction, action);
        }

        if (Description is { } description)
        {
            vp_string_set(handle, StringAttribute.ObjectDescription, description);
        }

        if (Model is { } model)
        {
            vp_string_set(handle, StringAttribute.ObjectModel, model);
        }

        if (Position is { } position)
        {
            (double x, double y, double z) = position;
            vp_double_set(handle, FloatAttribute.ObjectX, x);
            vp_double_set(handle, FloatAttribute.ObjectY, y);
            vp_double_set(handle, FloatAttribute.ObjectZ, z);
        }
        else if (Mode == ObjectBuilderMode.Create)
        {
            throw new ArgumentException("Position must be assigned when creating a new object.");
        }

        if (Rotation is null && Mode == ObjectBuilderMode.Create)
        {
            Rotation = Quaternion.Identity;
        }

        if (Rotation is { } rotation)
        {
            (double x, double y, double z) = Vector3d.Zero;
            double angle = double.PositiveInfinity;
            if (rotation != Quaternion.Identity)
            {
                rotation.ToAxisAngle(out Vector3d axis, out angle);
                (x, y, z) = axis;
            }

            vp_double_set(handle, FloatAttribute.ObjectRotationX, x);
            vp_double_set(handle, FloatAttribute.ObjectRotationY, y);
            vp_double_set(handle, FloatAttribute.ObjectRotationZ, z);
            vp_double_set(handle, FloatAttribute.ObjectRotationAngle, angle);
        }

        if (ModificationTimestamp is { } modificationTimestamp)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Modification timestamp can only be assigned during an object load.");
            }

            vp_int_set(handle, IntegerAttribute.ObjectTime, (int) modificationTimestamp.ToUnixTimeSeconds());
        }

        if (Owner is { } owner)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Owner can only be assigned during an object load.");
            }

            vp_int_set(handle, IntegerAttribute.ObjectUserId, owner.Id);
        }
    }
}
