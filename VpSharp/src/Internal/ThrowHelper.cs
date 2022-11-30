using System.Diagnostics.CodeAnalysis;
using VpSharp.Exceptions;

namespace VpSharp.Internal;

internal static class ThrowHelper
{
    public static InvalidOperationException CannotUseSelfException()
    {
        return new InvalidOperationException(ExceptionMessages.CannotUseSelf);
    }

    [DoesNotReturn]
    public static void ThrowCannotUseSelfException()
    {
        throw CannotUseSelfException();
    }

    public static InvalidOperationException NotInWorldException()
    {
        return new InvalidOperationException(ExceptionMessages.NotInWorld);
    }

    [DoesNotReturn]
    public static void ThrowNotInWorldException()
    {
        throw NotInWorldException();
    }

    public static ObjectNotFoundException ObjectNotFoundException()
    {
        return new ObjectNotFoundException(ExceptionMessages.ObjectNotFound);
    }

    [DoesNotReturn]
    public static void ThrowObjectNotFoundException()
    {
        throw ObjectNotFoundException();
    }

    public static ArgumentException StringTooLongException(string paramName)
    {
        return new ArgumentException(ExceptionMessages.StringTooLong, paramName);
    }

    [DoesNotReturn]
    public static void ThrowStringTooLongException(string paramName)
    {
        throw StringTooLongException(paramName);
    }

    public static ArgumentOutOfRangeException ZeroThroughOneException(string paramName)
    {
        return new ArgumentOutOfRangeException(paramName, ExceptionMessages.ZeroThroughOne);
    }

    [DoesNotReturn]
    public static void ThrowZeroThroughOneException(string paramName)
    {
        throw ZeroThroughOneException(paramName);
    }
}
