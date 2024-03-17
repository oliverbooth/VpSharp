using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VpSharp.ClientExtensions;
using VpSharp.Commands.Attributes;
using VpSharp.Commands.Attributes.ExecutionChecks;
using VpSharp.Entities;
using VpSharp.EventData;
using VpSharp.Internal;

namespace VpSharp.Commands;

/// <summary>
///     Implements the Commands extension for <see cref="VirtualParadiseClient" />.
/// </summary>
public sealed class CommandsExtension : VirtualParadiseClientExtension
{
    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance;

    private readonly Dictionary<string, Command> _commandMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly CommandsExtensionConfiguration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CommandsExtension" /> class.
    /// </summary>
    /// <param name="client">The owning client.</param>
    /// <param name="configuration">The configuration to use.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="client" /> or <paramref name="configuration" /> is <see langword="null" />.
    /// </exception>
    public CommandsExtension(VirtualParadiseClient client, CommandsExtensionConfiguration configuration)
        : base(client)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }


        _configuration = configuration;
        _configuration.Services ??= client.Services;
    }

    /// <summary>
    ///     Registers all commands from all modules in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly whose command modules to register.</param>
    /// <exception cref="ArgumentException">
    ///     A module in the specified assembly does not have a public constructor, or has more than one public constructor.
    /// </exception>
    /// <exception cref="TypeInitializationException">A command module could not be instantiated.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para>A command in the specified assembly does not have a <see cref="CommandContext" /> as its first parameter.</para>
    ///     -or-
    ///     <para>A command in the specified assembly contains a duplicate name or alias for a command.</para>
    ///     -or-
    ///     <para>
    ///         A command in the specified assembly has <see cref="RemainderAttribute" /> on a parameter that is not the last in
    ///         the parameter list.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         A module is expecting services but no <see cref="IServiceProvider" /> was registered.
    ///     </para>
    /// </exception>
    public void RegisterCommands(Assembly assembly)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsAbstract && type.IsSubclassOf(typeof(CommandModule)))
            {
                RegisterCommands(type);
            }
        }
    }

    /// <summary>
    ///     Registers the commands defined in the specified type.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///     <para><typeparamref name="T" /> refers to a type that does not inherit <see cref="CommandModule" />.</para>
    ///     -or-
    ///     <para>
    ///         <typeparamref name="T" /> does not have a public constructor, or has more than one public constructor.
    ///     </para>
    /// </exception>
    /// <exception cref="TypeInitializationException"><typeparamref name="T" /> could not be instantiated.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para>A command in the specified module does not have a <see cref="CommandContext" /> as its first parameter.</para>
    ///     -or-
    ///     <para>A command in the specified module contains a duplicate name or alias for a command.</para>
    ///     -or-
    ///     <para>
    ///         A command in the specified module has <see cref="RemainderAttribute" /> on a parameter that is not the last in the
    ///         parameter list.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         The module is expecting services but no <see cref="IServiceProvider" /> was registered.
    ///     </para>
    /// </exception>
    public void RegisterCommands<T>() where T : CommandModule
    {
        RegisterCommands(typeof(T));
    }

    /// <summary>
    ///     Registers the commands defined in the specified type.
    /// </summary>
    /// <param name="moduleType">The type whose command methods to register.</param>
    /// <exception cref="ArgumentNullException"><paramref name="moduleType" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="moduleType" /> refers to an <c>abstract</c> type.</para>
    ///     -or-
    ///     <para><paramref name="moduleType" /> refers to a type that does not inherit <see cref="CommandModule" />.</para>
    ///     -or-
    ///     <para>
    ///         <paramref name="moduleType" /> does not have a public constructor, or has more than one public constructor.
    ///     </para>
    /// </exception>
    /// <exception cref="TypeInitializationException"><paramref name="moduleType" /> could not be instantiated.</exception>
    /// <exception cref="InvalidOperationException">
    ///     <para>A command in the specified module does not have a <see cref="CommandContext" /> as its first parameter.</para>
    ///     -or-
    ///     <para>A command in the specified module contains a duplicate name or alias for a command.</para>
    ///     -or-
    ///     <para>
    ///         A command in the specified module has <see cref="RemainderAttribute" /> on a parameter that is not the last in the
    ///         parameter list.
    ///     </para>
    ///     -or-
    ///     <para>
    ///         The module is expecting services but no <see cref="IServiceProvider" /> was registered.
    ///     </para>
    /// </exception>
    public void RegisterCommands(Type moduleType)
    {
        if (moduleType is null)
        {
            throw new ArgumentNullException(nameof(moduleType));
        }

        if (moduleType.IsAbstract)
        {
            throw new ArgumentException("Module type cannot be abstract");
        }

        if (!moduleType.IsSubclassOf(typeof(CommandModule)))
        {
            throw new ArgumentException($"Module type is not a subclass of {typeof(CommandModule)}");
        }

        IServiceProvider? serviceProvider = _configuration.Services;
        object instance = DependencyInjectionUtility.CreateInstance(moduleType, serviceProvider);
        if (instance is not CommandModule module)
        {
            throw new TypeInitializationException(moduleType.FullName, null);
        }

        foreach (MethodInfo method in moduleType.GetMethods(BindingFlags))
        {
            RegisterCommandMethod(module, method);
        }
    }

    /// <inheritdoc />
    protected internal override Task OnMessageReceived(Message message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.Type != MessageType.ChatMessage)
        {
            return base.OnMessageReceived(message);
        }

        foreach (ReadOnlySpan<char> prefix in _configuration.Prefixes)
        {
            ReadOnlySpan<char> content = message.Content;
            if (!content.StartsWith(prefix))
            {
                continue;
            }

            ReadOnlySpan<char> commandName, rawArguments;
            int spaceIndex = content.IndexOf(' ');

            if (spaceIndex == -1)
            {
                commandName = content[prefix.Length..];
                rawArguments = ReadOnlySpan<char>.Empty;
            }
            else
            {
                commandName = content[prefix.Length..spaceIndex];
                rawArguments = content[(spaceIndex + 1)..];
            }

            var commandNameString = commandName.ToString();
            if (!_commandMap.TryGetValue(commandNameString, out Command? command))
            {
                return base.OnMessageReceived(message);
            }

            var context = new CommandContext(Client, message.Author, command.Name, commandNameString, rawArguments.ToString());
            MethodInfo commandMethod = command.Method;

            foreach (var attribute in commandMethod.GetCustomAttributes<PreExecutionCheckAttribute>())
            {
                if (!attribute.PerformAsync(context).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    return base.OnMessageReceived(message);
                }
            }

            object?[] arguments = {context};
            arguments = ParseArguments(rawArguments, arguments, command);

            ParameterInfo[] parameters = commandMethod.GetParameters();
            if (parameters.Length != arguments.Length || parameters[arguments.Length..].Any(p => !p.IsOptional))
            {
                return base.OnMessageReceived(message);
            }

            for (var index = 0; index < arguments.Length; index++)
            {
                Type parameterType = parameters[index].ParameterType;

                if (parameterType.IsEnum)
                {
                    var argumentString = arguments[index]?.ToString();

                    Type enumUnderlyingType = parameterType.GetEnumUnderlyingType();
                    if (enumUnderlyingType == typeof(int) && int.TryParse(argumentString, out int enumInt))
                    {
                        arguments[index] = Enum.ToObject(parameterType, enumInt);
                    }
                    else if (enumUnderlyingType == typeof(long) && long.TryParse(argumentString, out long enumLong))
                    {
                        arguments[index] = Enum.ToObject(parameterType, enumLong);
                    }
                    else if (Enum.TryParse(parameterType, argumentString, true, out object? enumValue))
                    {
                        arguments[index] = enumValue;
                    }
                }
                else
                {
                    arguments[index] = Convert.ChangeType(arguments[index], parameterType, CultureInfo.InvariantCulture);
                }
            }

            for (int index = arguments.Length; index < parameters.Length; index++)
            {
                arguments[index] = parameters[index].DefaultValue;
            }

            object? returnValue = commandMethod.Invoke(command.Module, arguments);
            if (returnValue is Task task)
            {
                return task;
            }

            return base.OnMessageReceived(message);
        }

        return base.OnMessageReceived(message);
    }

    private static object?[] ParseArguments(ReadOnlySpan<char> rawArguments, object?[] arguments, Command command)
    {
        if (rawArguments.Length <= 0)
        {
            return arguments;
        }

        int spaceIndex = rawArguments.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Array.Resize(ref arguments, 2);
            arguments[1] = rawArguments.ToString();
            return arguments;
        }

        var appendLast = true;
        for (var argumentIndex = 1; spaceIndex > -1; argumentIndex++)
        {
            Array.Resize(ref arguments, argumentIndex + 1);

            if (argumentIndex == command.Parameters.Length)
            {
                if (command.Parameters[argumentIndex - 1].GetCustomAttribute<RemainderAttribute>() is not null)
                {
                    appendLast = false;
                    arguments[argumentIndex] = rawArguments.ToString();
                    break;
                }
            }

            arguments[argumentIndex] = rawArguments[..spaceIndex].ToString();
            rawArguments = rawArguments[(spaceIndex + 1)..];
            spaceIndex = rawArguments.IndexOf(' ');
            argumentIndex++;
        }

        if (appendLast)
        {
            Array.Resize(ref arguments, arguments.Length + 1);
            arguments[^1] = rawArguments.ToString();
        }

        return arguments;
    }

    private void RegisterCommandMethod(CommandModule module, MethodInfo methodInfo)
    {
        var commandAttribute = methodInfo.GetCustomAttribute<CommandAttribute>();
        if (commandAttribute is null)
        {
            return;
        }

        var aliasesAttribute = methodInfo.GetCustomAttribute<AliasesAttribute>();
        ParameterInfo[] parameters = methodInfo.GetParameters();
        if (parameters.Length == 0 || parameters[0].ParameterType != typeof(CommandContext))
        {
            throw new InvalidOperationException($"Command method must have a {typeof(CommandContext)} parameter at index 0.");
        }

        for (var index = 0; index < parameters.Length - 1; index++)
        {
            if (parameters[index].GetCustomAttribute<RemainderAttribute>() is not null)
            {
                throw new InvalidOperationException($"{typeof(RemainderAttribute)} can only be placed on the last parameter.");
            }
        }

        var types = new Type[parameters.Length - 1];
        for (var index = 1; index < parameters.Length; index++)
        {
            types[index - 1] = parameters[index].ParameterType;
        }

        var command = new Command(
            commandAttribute.Name,
            aliasesAttribute?.Aliases ?? ArraySegment<string>.Empty,
            methodInfo,
            module
        );

        if (_commandMap.ContainsKey(command.Name))
        {
            throw new InvalidOperationException($"Duplicate command name registered ({command.Name})");
        }

        _commandMap[command.Name] = command;

        foreach (string alias in command.Aliases)
        {
            if (_commandMap.ContainsKey(alias))
            {
                throw new InvalidOperationException($"Duplicate command name registered ({alias})");
            }

            _commandMap[alias] = command;
        }
    }
}
