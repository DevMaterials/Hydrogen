namespace Hydrogen.Abstraction.Exceptions;

public class InvaidFileException(string path, string? message) : AbstractException(message)
{
    public string Path { get; } = path;
}
