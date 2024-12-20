using System.Globalization;
using System.Reflection;
using VpSharp.Building.Annotations;
using VpSharp.Building.Commands;

namespace VpSharp.Building;

public static partial class ActionSerializer
{
    private delegate void PropertyAssignDelegate(VirtualParadiseCommand command, PropertyInfo property, ReadOnlySpan<char> chars);

    private static readonly Dictionary<Type, PropertyAssignDelegate> ValueParsers = new()
    {
        {
            typeof(byte), (command, property, span) =>
            {
                if (byte.TryParse(span, CultureInfo.InvariantCulture, out byte value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(sbyte), (command, property, span) =>
            {
                if (sbyte.TryParse(span, CultureInfo.InvariantCulture, out sbyte value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(short), (command, property, span) =>
            {
                if (short.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out short value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(ushort), (command, property, span) =>
            {
                if (ushort.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out ushort value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(int), (command, property, span) =>
            {
                if (int.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(uint), (command, property, span) =>
            {
                if (uint.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(long), (command, property, span) =>
            {
                if (long.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(ulong), (command, property, span) =>
            {
                if (ulong.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out ulong value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(char), (command, property, span) =>
            {
                property.SetValue(command, span[0]);
            }
        },
        {
            typeof(bool), (command, property, span) =>
            {
                if (bool.TryParse(span, out bool value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(Half), (command, property, span) =>
            {
                if (Half.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out Half value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(float), (command, property, span) =>
            {
                if (float.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(double), (command, property, span) =>
            {
                if (double.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(decimal), (command, property, span) =>
            {
                if (decimal.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value))
                {
                    property.SetValue(command, value);
                }
            }
        },
        {
            typeof(string), (command, property, span) =>
            {
                property.SetValue(command, span.ToString());
            }
        }
    };

    private static void SetProperty(VirtualParadiseCommand command, ReadOnlySpan<char> name, ReadOnlySpan<char> value)
    {
        PropertyInfo[] properties = command.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<PropertyAttribute>() is not { } attribute)
            {
                continue;
            }

            if (!name.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            SetProperty(command, property, value);
        }
    }

    private static void SetProperty(VirtualParadiseCommand command, PropertyInfo property, ReadOnlySpan<char> chars)
    {
        Type propertyType = property.PropertyType;
        if (ValueParsers.TryGetValue(propertyType, out PropertyAssignDelegate? parser))
        {
            parser(command, property, chars);
            return;
        }

        if (propertyType.IsEnum && Enum.TryParse(propertyType, chars, true, out object? value))
        {
            property.SetValue(command, value);
            return;
        }

        value = Convert.ChangeType(chars.ToString(), propertyType, CultureInfo.InvariantCulture);
        property.SetValue(command, value);
    }
}
