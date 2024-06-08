namespace Hydrogen.Abstraction.Exceptions;

public abstract class AbstractException : Exception
{
    protected AbstractException() { }
    protected AbstractException(string? message) : base(message) { }
    protected AbstractException(string? message, Exception? innerException) : base(message, innerException) { }
}
