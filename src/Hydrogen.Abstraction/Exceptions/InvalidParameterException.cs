namespace Hydrogen.Abstraction.Exceptions;

public sealed class InvalidParameterException(string parameter, object? value, string? message = null) : AbstractException(message)
{
    public string Parameter { get; } = parameter;
    public object? Value { get; } = value;
}
