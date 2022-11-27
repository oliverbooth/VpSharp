using System.Globalization;
using System.Reflection;
using VpSharp.ClientExtensions;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly List<VirtualParadiseClientExtension> _extensions = new();

    /// <summary>
    ///     Adds an extension to this client.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the extension constructor.</param>
    /// <typeparam name="T">The type of the extension to add.</typeparam>
    /// <returns>A new instance of the specified extension.</returns>
    /// <exception cref="ArgumentException">
    ///     <para><typeparamref name="T" /> is <c>abstract</c>.</para>
    /// </exception>
    public T AddExtension<T>(params object?[]? arguments) where T : VirtualParadiseClientExtension
    {
        return (AddExtension(typeof(T), arguments) as T)!;
    }

    /// <summary>
    ///     Adds an extension to this client.
    /// </summary>
    /// <param name="type">The type of the extension to add.</param>
    /// <param name="arguments">The arguments to pass to the extension constructor.</param>
    /// <returns>A new instance of the specified extension.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="type" /> does not inherit from <see cref="VirtualParadiseClientExtension" />.</para>
    ///     -or-
    ///     <para><paramref name="type" /> is <c>abstract</c>.</para>
    /// </exception>
    public VirtualParadiseClientExtension AddExtension(Type type, params object?[]? arguments)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsSubclassOf(typeof(VirtualParadiseClientExtension)))
        {
            throw new ArgumentException($"Type must inherit from {typeof(VirtualParadiseClientExtension)}");
        }

        if (type.IsAbstract)
        {
            throw new ArgumentException("Extension type cannot be abstract");
        }

        object?[] argumentsActual = {this};
        if (arguments is not null)
        {
            argumentsActual = argumentsActual.Concat(arguments).ToArray();
        }

        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        object? instance = Activator.CreateInstance(type, bindingFlags, null, argumentsActual, CultureInfo.InvariantCulture);
        if (instance is not VirtualParadiseClientExtension extension)
        {
            var innerException = new Exception($"Could not instantiate {type}");
            throw new TypeInitializationException(type.FullName, innerException);
        }

        _extensions.Add(extension);
        return extension;
    }
}
