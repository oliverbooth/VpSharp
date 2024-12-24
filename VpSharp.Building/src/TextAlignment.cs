using System.ComponentModel;

namespace VpSharp.Building;

/// <summary>
///     An enumeration of text alignments.
/// </summary>
public enum TextAlignment
{
    /// <summary>
    ///     Center text alignment.
    /// </summary>
    [Description("Center text alignment.")]
    Center,

    /// <summary>
    ///     Left text alignment.
    /// </summary>
    [Description("Left text alignment.")] Left,

    /// <summary>
    ///     Right text alignment.
    /// </summary>
    [Description("Right text alignment.")] Right
}
