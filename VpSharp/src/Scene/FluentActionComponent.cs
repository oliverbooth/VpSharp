using System.Drawing;
using System.Text;

namespace VpSharp.Scene;

public sealed class FluentAction
{
    internal List<FluentActionComponent> Components { get; } = new();

    public static FluentActionTrigger Activate()
    {
        var action = new FluentAction();
        var trigger = new FluentActionTrigger(action, Trigger.Activate);
        action.Components.Add(trigger);
        return trigger;
    }

    public static FluentActionTrigger Adone()
    {
        var action = new FluentAction();
        var trigger = new FluentActionTrigger(action, Trigger.Adone);
        action.Components.Add(trigger);
        return trigger;
    }

    public static FluentActionTrigger Bump()
    {
        var action = new FluentAction();
        var trigger = new FluentActionTrigger(action, Trigger.Bump);
        action.Components.Add(trigger);
        return trigger;
    }

    public static FluentActionTrigger BumpEnd()
    {
        var action = new FluentAction();
        var trigger = new FluentActionTrigger(action, Trigger.BumpEnd);
        action.Components.Add(trigger);
        return trigger;
    }

    public static FluentActionTrigger Create()
    {
        var action = new FluentAction();
        var trigger = new FluentActionTrigger(action, Trigger.Create);
        action.Components.Add(trigger);
        return trigger;
    }
}

public abstract class FluentActionComponent
{
    protected FluentActionComponent(FluentAction action)
    {
        Action = action;
    }

    protected internal FluentAction Action { get; }

    public FluentActionTrigger Activate()
    {
        var trigger = new FluentActionTrigger(Action, Trigger.Activate);
        Action.Components.Add(trigger);
        return trigger;
    }

    public FluentActionTrigger Adone()
    {
        var trigger = new FluentActionTrigger(Action, Trigger.Adone);
        Action.Components.Add(trigger);
        return trigger;
    }

    public FluentActionTrigger Bump()
    {
        var trigger = new FluentActionTrigger(Action, Trigger.Bump);
        Action.Components.Add(trigger);
        return trigger;
    }

    public FluentActionTrigger BumpEnd()
    {
        var trigger = new FluentActionTrigger(Action, Trigger.BumpEnd);
        Action.Components.Add(trigger);
        return trigger;
    }

    public FluentActionTrigger Create()
    {
        var trigger = new FluentActionTrigger(Action, Trigger.Create);
        Action.Components.Add(trigger);
        return trigger;
    }

    public FluentActionCommand Color(Color color)
    {
        var builder = new StringBuilder();
        builder.Append($"color {color.ToArgb():X6}");
        return new FluentActionCommand(Action, Command.Color);
    }

    public FluentActionCommand Texture(string texture, string? mask = null, string? tag = null)
    {
        var builder = new StringBuilder();
        builder.Append($"texture {texture}");

        if (!string.IsNullOrWhiteSpace(mask))
        {
            builder.Append($" mask={mask}");
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            builder.Append($" tag={tag}");
        }

        return new FluentActionCommand(Action, Command.Texture);
    }
}

public sealed class FluentActionCommand : FluentActionComponent
{
    internal FluentActionCommand(FluentAction action, Command command) : base(action)
    {
        Command = command;
    }

    internal Command Command { get; }
}

public sealed class FluentActionTrigger : FluentActionComponent
{
    internal FluentActionTrigger(FluentAction action, Trigger trigger) : base(action)
    {
        Trigger = trigger;
    }

    internal Trigger Trigger { get; }
}
