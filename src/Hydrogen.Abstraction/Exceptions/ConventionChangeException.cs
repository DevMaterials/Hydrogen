using Hydrogen.Abstraction.Helpers.Strings;

namespace Hydrogen.Abstraction.Exceptions;

public class ConventionChangeException(string value, NamingConventions convention) : AbstractException
{
    public string Value { get; } = value;
    public NamingConventions Convention { get; } = convention;
}
