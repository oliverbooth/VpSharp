using System.ComponentModel;

// ReSharper disable InconsistentNaming

namespace VpSharp;

/// <summary>
///     An enumeration of headings.
/// </summary>
public enum Heading
{
    /// <summary>
    ///     North.
    /// </summary>
    [Description("North")]
    N,

    /// <summary>
    ///     Northeast.
    /// </summary>
    [Description("Northeast")]
    NE,

    /// <summary>
    ///     East.
    /// </summary>
    [Description("East")]
    E,

    /// <summary>
    ///     Southeast.
    /// </summary>
    [Description("Southeast")]
    SE,

    /// <summary>
    ///     South.
    /// </summary>
    [Description("South")]
    S,

    /// <summary>
    ///     Southwest.
    /// </summary>
    [Description("Southwest")]
    SW,

    /// <summary>
    ///     West.
    /// </summary>
    [Description("West")]
    W,

    /// <summary>
    ///     Northwest.
    /// </summary>
    [Description("Northwest")]
    NW
}
