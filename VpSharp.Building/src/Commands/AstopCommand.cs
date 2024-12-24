﻿using VpSharp.Building.Annotations;

namespace VpSharp.Building.Commands;

/// <summary>
///     Represents the <c>astop</c> command.
/// </summary>
[Command("astop")]
public sealed class AstopCommand : VirtualParadiseCommand
{
    /// <summary>
    ///     Gets or sets the name value.
    /// </summary>
    /// <value>The name value.</value>
    [Parameter(0)]
    public string Name { get; set; } = string.Empty;
}