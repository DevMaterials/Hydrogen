using Hydrogen.Abstraction.Enums;

namespace Hydrogen.Abstraction.Exceptions;

public class NamingConventionException(string name, NamingConventions newConvention) : AbstractException("The name or specified conventions are not valid.")
{
    public string Name { get; } = name;
    public NamingConventions NewConvention { get; } = newConvention;
}
