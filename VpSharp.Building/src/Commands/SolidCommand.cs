﻿using VpSharp.Building.Commands.Converters;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>solid</c> command.
/// </summary>
[Command("solid", ConverterType = typeof(SolidCommandConverter))]
public sealed class SolidCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the solid flag value.
    /// </summary>
    /// <value>The solid flag value.</value>
    public bool IsSolid { get; set; } = true;
}