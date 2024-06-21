namespace Hydrogen.Abstraction.Exceptions;

public sealed class InvalidFileException(string path, string? message) : AbstractException(message)
{
    public string Path { get; } = path;
}
