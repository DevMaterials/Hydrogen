namespace Hydrogen.Abstraction.Exceptions;

/// <summary>
///     This class will be used as a base class for all exception types of the Hydrogen framework, its 
///     libraries, and services.
/// </summary>
public abstract class AbstractException : Exception
{
    protected AbstractException() { }
    protected AbstractException(string? message) : base(message) { }
    protected AbstractException(string? message, Exception? innerException) : base(message, innerException) { }
}
