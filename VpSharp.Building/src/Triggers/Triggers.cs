using VpSharp.Building.Annotations;

namespace VpSharp.Building.Triggers;

/// <summary>
///     Represents the <c>activate</c> trigger.
/// </summary>
[Trigger("activate")]
public sealed class ActivateTrigger : VirtualParadiseTrigger;

/// <summary>
///     Represents the <c>adone</c> trigger.
/// </summary>
[Trigger("adone")]
public sealed class AdoneTrigger : VirtualParadiseTrigger;

/// <summary>
///     Represents the <c>bump</c> trigger.
/// </summary>
[Trigger("bump")]
public sealed class BumpTrigger : VirtualParadiseTrigger;

/// <summary>
///     Represents the <c>bumpend</c> trigger.
/// </summary>
[Trigger("bumpend")]
public sealed class BumpEndTrigger : VirtualParadiseTrigger;

/// <summary>
///     Represents the <c>create</c> trigger.
/// </summary>
[Trigger("create")]
public sealed class CreateTrigger : VirtualParadiseTrigger;
