using System.Diagnostics;
using System.Reflection;
using System.Text;
using Optional;
using VpSharp.Building.Commands;
using VpSharp.Building.Extensions;
using VpSharp.Building.Triggers;

namespace VpSharp.Building;

/// <summary>
///     Represents a class that can serialize and deserialize action strings.
/// </summary>
public static partial class ActionSerializer
{
    private const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private static readonly Encoding Encoding = Encoding.UTF8;

    private static VirtualParadiseCommand? FindCommand(ReadOnlySpan<char> tokenValue, IEnumerable<Type> commandTypes)
    {
        foreach (Type type in commandTypes)
        {
            if (type.GetCommandName().AsSpan().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseCommand;
            }
        }

        return null;
    }

    private static VirtualParadiseTrigger? FindTrigger(ReadOnlySpan<char> tokenValue, IEnumerable<Type> triggerTypes)
    {
        foreach (Type type in triggerTypes)
        {
            if (type.GetTriggerName().AsSpan().Equals(tokenValue, StringComparison.OrdinalIgnoreCase))
            {
                return Activator.CreateInstance(type) as VirtualParadiseTrigger;
            }
        }

        return null;
    }

    private static object? CreateDefaultInstance(PropertyInfo property)
    {
        if (IsOptional(property))
        {
            Type underlyingType = GetUnderlyingType(property);
            object? instance = underlyingType.IsValueType ? Activator.CreateInstance(underlyingType) : null;
            return Activator.CreateInstance(property.PropertyType, PropertyBindingFlags, null, [instance, false], null);
        }

        return property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
    }

    private static Type GetUnderlyingType(PropertyInfo property)
    {
        Type type = property.PropertyType;

        if (IsOptional(property))
        {
            return type.GetGenericArguments()[0];
        }

        return IsNullableValueType(property) ? Nullable.GetUnderlyingType(type)! : type;
    }

    private static bool IsNullableValueType(PropertyInfo property)
    {
        Type type = property.PropertyType;
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static bool IsOptional(PropertyInfo property)
    {
        Type type = property.PropertyType;
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Option<>);
    }

    private static object? OverlayValue(PropertyInfo property, object? value)
    {
        if (value is null)
        {
            return CreateDefaultInstance(property);
        }

        if (IsOptional(property))
        {
            return Activator.CreateInstance(property.PropertyType, PropertyBindingFlags, null, [value, true], null);
        }

        return value;
    }
}
