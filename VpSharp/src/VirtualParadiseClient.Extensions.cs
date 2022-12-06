using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using VpSharp.ClientExtensions;

namespace VpSharp;

public sealed partial class VirtualParadiseClient
{
    private readonly List<VirtualParadiseClientExtension> _extensions = new();

    /// <summary>
    ///     Gets a read-only view of the extensions currently added to this client.
    /// </summary>
    /// <value>A read-only view of the extensions.</value>
    public IReadOnlyList<VirtualParadiseClientExtension> Extensions
    {
        get => _extensions.AsReadOnly();
    }

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
            throw new ArgumentException(
                string.Format(CultureInfo.InvariantCulture, ExceptionMessages.TypeMustInherit,
                    typeof(VirtualParadiseClientExtension)), nameof(type));
        }

        if (type.IsAbstract)
        {
            throw new ArgumentException(ExceptionMessages.TypeCannotBeAbstract, nameof(type));
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
            throw new TypeInitializationException(type.FullName, null);
        }

        _extensions.Add(extension);
        return extension;
    }

    /// <summary>
    ///     Gets the extension whose type matches a specified type.
    /// </summary>
    /// <param name="type">The type of the extension to get.</param>
    /// <returns>The extension instance whose type matches <paramref name="type" />.</returns>
    /// <exception cref="InvalidOperationException">No extension with the specified type is added to this client.</exception>
    public VirtualParadiseClientExtension GetExtension(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        VirtualParadiseClientExtension? result = _extensions.Find(e => e.GetType() == type);
        if (result is null)
        {
            throw new InvalidOperationException($"No extension with the type {type} is added to this client.");
        }

        return result;
    }

    /// <summary>
    ///     Gets the extension whose type matches a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the extension to get.</typeparam>
    /// <returns>The extension instance whose type matches <typeparamref name="T" />.</returns>
    /// <exception cref="InvalidOperationException">No extension with the specified type is added to this client.</exception>
    public T GetExtension<T>() where T : VirtualParadiseClientExtension
    {
        var result = _extensions.Find(e => e.GetType() == typeof(T)) as T;
        if (result is null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ExceptionMessages.NoSuchExtension,
                typeof(T)));
        }

        return result;
    }

    /// <summary>
    ///     Attempts to get the extension whose type matches a specified type.
    /// </summary>
    /// <param name="type">The type of the extension to get.</param>
    /// <param name="extension">
    ///     When this method returns, contains the extension instance whose type matches <paramref name="type" />, or
    ///     <see langword="null" /> if no such extension exists.
    /// </param>
    /// <returns><see langword="true" /> if a matching extension was found; otherwise, <see langword="false" />.</returns>
    /// <exception cref="InvalidOperationException">No extension with the specified type is added to this client.</exception>
    public bool TryGetExtension(Type type, [NotNullWhen(true)] out VirtualParadiseClientExtension? extension)
    {
        ArgumentNullException.ThrowIfNull(type);

        extension = _extensions.Find(e => e.GetType() == type);
        return extension is not null;
    }

    /// <summary>
    ///     Attempts to get the extension whose type matches a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the extension to get.</typeparam>
    /// <param name="extension">
    ///     When this method returns, contains the extension instance whose type matches <paramref name="type" />, or
    ///     <see langword="null" /> if no such extension exists.
    /// </param>
    /// <returns><see langword="true" /> if a matching extension was found; otherwise, <see langword="false" />.</returns>
    /// <exception cref="InvalidOperationException">No extension with the specified type is added to this client.</exception>
    public bool TryGetExtension<T>([NotNullWhen(true)] out T? extension) where T : VirtualParadiseClientExtension
    {
        extension = _extensions.Find(e => e.GetType() == typeof(T)) as T;
        return extension is not null;
    }
}
