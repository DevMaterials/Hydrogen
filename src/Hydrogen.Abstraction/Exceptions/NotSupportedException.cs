namespace Hydrogen.Abstraction.Exceptions;

public sealed class NotSupportedException<TValue>(string name, TValue? value, string? message = null) : AbstractException(message)
{
    public string Name { get; } = name;
    public TValue? Value { get; } = value;
}
