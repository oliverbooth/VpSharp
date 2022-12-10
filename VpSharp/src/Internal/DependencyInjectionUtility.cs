using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace VpSharp.Internal;

internal static class DependencyInjectionUtility
{
    public static T CreateInstance<T>(VirtualParadiseClient client)
    {
        return CreateInstance<T>(client.Services);
    }

    public static T CreateInstance<T>(IServiceProvider? serviceProvider = null)
    {
        return (T)CreateInstance(typeof(T), serviceProvider);
    }

    public static object CreateInstance(Type type, VirtualParadiseClient client)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(client);
        return CreateInstance(type, client.Services);
    }

    public static object CreateInstance(Type type, IServiceProvider? serviceProvider = null)
    {
        ArgumentNullException.ThrowIfNull(type);
        object? instance;

        TypeInfo typeInfo = type.GetTypeInfo();
        ConstructorInfo[] constructors = typeInfo.DeclaredConstructors.Where(c => c.IsPublic).ToArray();

        if (constructors.Length != 1)
        {
            throw new InvalidOperationException($"{type} has no public constructors, or has more than one public constructor.");
        }

        ConstructorInfo constructor = constructors[0];
        ParameterInfo[] parameters = constructor.GetParameters();

        if (parameters.Length > 0 && serviceProvider is null)
        {
            throw new InvalidOperationException("No ServiceProvider has been registered!");
        }

        if (parameters.Length == 0)
        {
            instance = Activator.CreateInstance(type);
            if (instance is null)
            {
                throw new TypeInitializationException(type.FullName, null);
            }

            return instance;
        }

        var args = new object[parameters.Length];
        for (var index = 0; index < parameters.Length; index++)
        {
            args[index] = serviceProvider!.GetRequiredService(parameters[index].ParameterType);
        }

        instance = Activator.CreateInstance(type, args);
        if (instance is null)
        {
            throw new TypeInitializationException(type.FullName, null);
        }

        return instance;
    }
}
