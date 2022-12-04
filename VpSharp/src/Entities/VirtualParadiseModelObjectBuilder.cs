﻿using System.Numerics;
using VpSharp.Extensions;
using VpSharp.Internal;
using VpSharp.Internal.NativeAttributes;
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
    /// <value>The value of this object's <c>Action</c> field, or <see langword="null" /> to leave unchanged.</value>
    public Optional<string> Action { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Description</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Description</c> field, or <see langword="null" /> to leave unchanged.</value>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <value>The value of this object's <c>Model</c> field, or <see langword="null" /> to leave unchanged.</value>
    public Optional<string> Model { get; set; }

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
    public Optional<DateTimeOffset> ModificationTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the owner of this object.
    /// </summary>
    /// <value>The owner of this object, or <see langword="null" /> to leave unchanged.</value>
    /// <remarks>
    ///     This property may only be set during an object load, and will throw <see cref="InvalidOperationException" /> at
    ///     any other point.
    /// </remarks>
    public Optional<VirtualParadiseUser> Owner { get; set; }

    /// <summary>
    ///     Gets or sets the position of the object.
    /// </summary>
    /// <value>The position of the object, or <see langword="null" /> to leave unchanged.</value>
    public Optional<Vector3d> Position { get; set; }

    /// <summary>
    ///     Gets or sets the rotation of the object.
    /// </summary>
    /// <value>The rotation of the object, or <see langword="null" /> to leave unchanged.</value>
    public Optional<Quaternion> Rotation { get; set; }

    /// <summary>
    ///     Sets the value of this object's <c>Action</c> field.
    /// </summary>
    /// <param name="action">The new value of the <c>Action</c> field, or <see langword="null" /> to leave unchanged.</param>
    /// <returns>The current instance of this builder.</returns>
    public VirtualParadiseModelObjectBuilder WithAction(Optional<string> action)
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
    public VirtualParadiseModelObjectBuilder WithDescription(Optional<string> description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    ///     Sets the value of this object's <c>Model</c> field.
    /// </summary>
    /// <param name="model">The new value of the <c>Model</c> field, or <see langword="null" /> to leave unchanged.</param>
    /// <returns>The current instance of this builder.</returns>
    public VirtualParadiseModelObjectBuilder WithModel(Optional<string> model)
    {
        Model = model;
        return this;
    }

    internal void ApplyChanges()
    {
        nint handle = Client.NativeInstanceHandle;
        var targetObject = (VirtualParadiseModelObject)TargetObject;

        vp_string_set(handle, StringAttribute.ObjectModel, Model.HasValue ? Model.Value! : targetObject.Model);
        vp_string_set(handle, StringAttribute.ObjectDescription, Description.HasValue ? Description.Value! : targetObject.Description);
        vp_string_set(handle, StringAttribute.ObjectAction, Action.HasValue ? Action.Value! : targetObject.Action);

        if (Position.HasValue)
        {
            (double x, double y, double z) = Position.Value;
            _ = vp_double_set(handle, FloatAttribute.ObjectX, x);
            _ = vp_double_set(handle, FloatAttribute.ObjectY, y);
            _ = vp_double_set(handle, FloatAttribute.ObjectZ, z);
        }
        else if (Mode == ObjectBuilderMode.Create)
        {
            throw new ArgumentException("Position must be assigned when creating a new object.");
        }
        else
        {
            (double x, double y, double z) = targetObject.Location.Position;
            _ = vp_double_set(handle, FloatAttribute.ObjectX, x);
            _ = vp_double_set(handle, FloatAttribute.ObjectY, y);
            _ = vp_double_set(handle, FloatAttribute.ObjectZ, z);
        }

        if (!Rotation.HasValue && Mode == ObjectBuilderMode.Create)
        {
            Rotation = Quaternion.Identity;
        }

        if (Rotation.HasValue)
        {
            (double x, double y, double z) = Vector3d.Zero;
            double angle = double.PositiveInfinity;
            if (Rotation.Value != Quaternion.Identity)
            {
                Rotation.Value.ToAxisAngle(out Vector3d axis, out angle);
                (x, y, z) = axis;
            }

            _ = vp_double_set(handle, FloatAttribute.ObjectRotationX, x);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationY, y);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationZ, z);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationAngle, angle);
        }
        else
        {
            targetObject.Location.Rotation.ToAxisAngle(out Vector3 axis, out float angle);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationX, axis.X);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationY, axis.Y);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationZ, axis.Z);
            _ = vp_double_set(handle, FloatAttribute.ObjectRotationAngle, angle);
        }

        if (ModificationTimestamp.HasValue)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Modification timestamp can only be assigned during an object load.");
            }

            _ = vp_int_set(handle, IntegerAttribute.ObjectTime, (int)ModificationTimestamp.Value.ToUnixTimeSeconds());
        }
        else
        {
            _ = vp_int_set(handle, IntegerAttribute.ObjectTime, (int)targetObject.ModificationTimestamp.ToUnixTimeSeconds());
        }

        if (Owner.HasValue)
        {
            if (Mode != ObjectBuilderMode.Load)
            {
                throw new InvalidOperationException("Owner can only be assigned during an object load.");
            }

            _ = vp_int_set(handle, IntegerAttribute.ObjectUserId, Owner.Value!.Id);
        }
        else
        {
            _ = vp_int_set(handle, IntegerAttribute.ObjectUserId, targetObject.Owner.Id);
        }
    }
}
