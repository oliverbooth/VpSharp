using System.Reflection;
using VpSharp.ClientExtensions;
using VpSharp.Commands.Attributes;
using VpSharp.EventData;

namespace VpSharp.Commands;

/// <summary>
///     Implements the Commands extension for <see cref="VirtualParadiseClient" />.
/// </summary>
public sealed class CommandsExtension : VirtualParadiseClientExtension
{
    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
                                              System.Reflection.BindingFlags.NonPublic |
                                              System.Reflection.BindingFlags.Instance;

    private readonly CommandsExtensionConfiguration _configuration;
    private readonly Dictionary<string, Command> _commandMap = new(StringComparer.OrdinalIgnoreCase);

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
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    ///     Registers all commands from all modules in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly whose command modules to register.</param>
    /// <exception cref="ArgumentException">
    ///     
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
    /// </exception>
    public void RegisterCommands(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsAbstract && type.IsSubclassOf(typeof(CommandModule)))
            {
                RegisterCommands(type);
            }
        }
    }

    /// <summary>
    ///     Registers the command 
    /// </summary>
    /// <exception cref="ArgumentException">
    ///     <typeparamref name="T" /> refers to a type that does not inherit <see cref="CommandModule" />.
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
    /// </exception>
    public void RegisterCommands<T>() where T : CommandModule
    {
        RegisterCommands(typeof(T));
    }

    /// <summary>
    ///     Registers the command 
    /// </summary>
    /// <param name="moduleType"></param>
    /// <exception cref="ArgumentNullException"><paramref name="moduleType" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     <para><paramref name="moduleType" /> refers to an <c>abstract</c> type.</para>
    ///     -or-
    ///     <para><paramref name="moduleType" /> refers to a type that does not inherit <see cref="CommandModule" />.</para>
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
    /// </exception>
    public void RegisterCommands(Type moduleType)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        if (moduleType.IsAbstract)
        {
            throw new ArgumentException("Module type cannot be abstract");
        }

        if (!moduleType.IsSubclassOf(typeof(CommandModule)))
        {
            throw new ArgumentException($"Module type is not a subclass of {typeof(CommandModule)}");
        }

        if (Activator.CreateInstance(moduleType) is not CommandModule module)
        {
            var innerException = new Exception($"Could not instantiate {moduleType.FullName}");
            throw new TypeInitializationException(moduleType.FullName, innerException);
        }

        foreach (MethodInfo method in moduleType.GetMethods(BindingFlags))
        {
            RegisterCommandMethod(module, method);
        }
    }

    /// <inheritdoc />
    protected override Task OnMessageReceived(MessageReceivedEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);
        var message = args.Message;

        if (message.Type != MessageType.ChatMessage)
        {
            return base.OnMessageReceived(args);
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
                return base.OnMessageReceived(args);
            }

            var context = new CommandContext(Client, message.Author, command.Name, commandNameString, rawArguments.ToString());
            object?[] arguments = {context};

            if (rawArguments.Length > 0)
            {
                spaceIndex = rawArguments.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    Array.Resize(ref arguments, 2);
                    arguments[1] = rawArguments.ToString();
                }
                else
                {
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
                }
            }

            Console.WriteLine(string.Join(';', arguments));
            object? returnValue = command.Method.Invoke(command.Module, arguments);
            if (returnValue is Task task)
            {
                return task;
            }

            return base.OnMessageReceived(args);
        }

        return base.OnMessageReceived(args);
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
