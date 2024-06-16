namespace Hydrogen.Abstraction.Exceptions;

/// <summary>
///     When a parameter of the method is not valid, we can raise an exception from this type. 
/// </summary>
/// <param name="parameter">The name of the method's parameter.</param>
/// <param name="value">The value that was passed to the method.</param>
/// <param name="message">A short message that describes the problem.</param>
public class InvalidParameterException(string parameter, object? value, string? message = null) : AbstractException(message)
{
    /// <summary>
    ///     This property indicates that which parameter of the method is invalid.
    /// </summary>
    public string Parameter { get; } = parameter;

    /// <summary>
    ///     This property holds the invalid value of the parameter.
    /// </summary>
    public object? Value { get; } = value;
}
