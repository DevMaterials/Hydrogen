namespace Hydrogen.Abstraction.Enums;

/// <summary>
///     This enum will be used to define different kinds of the naming conventions.
/// </summary>
public enum NamingConventions
{
    /// <summary>
    ///     This convention enforces a name to only have upper case letters, digits, and start with a letter. 
    /// </summary>
    UpperCase = 1,

    /// <summary>
    ///     This convention enforces a name to only have lower case letters, digits, and start with a letter. 
    /// </summary>
    LowerCase = 2,

    /// <summary>
    ///     This convention enforces a name to only have letters, digits, and start with a lower case letter. 
    /// </summary>
    CamelCase = 3,

    /// <summary>
    ///     This convention enforces a name to only have letters, digits, and start with an upper case letter. 
    /// </summary>
    PascalCase = 4,

    /// <summary>
    ///     This convention enforces a name to only have lower case letters, digits, underlines, and start with
    ///     one or more underlines or a lower case letter. 
    /// </summary>
    LowerSnakeCase = 5,

    /// <summary>
    ///     This convention enforces a name to only have upper case letters, digits, underlines, and start with
    ///     one or more underlines or an upper case letter.
    /// </summary>
    UpperSnakeCase = 6,

    /// <summary>
    ///     This convention enforces a name to only have letters, digits, underlines, and start with one or more
    ///     underlines or an upper case letter. Also, each underline at the middle of the name should be followed
    ///     by an upper case letter.
    /// </summary>
    PascalSnakeCase = 7,
}
