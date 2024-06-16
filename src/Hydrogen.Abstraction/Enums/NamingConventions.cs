namespace Hydrogen.Abstraction.Enums;

/// <summary>
///     This enum defines different kinds of naming conventions.
/// </summary>
public enum NamingConventions
{
    /// <summary>
    ///     A name with this convention starts with a letter and contains only upper-case letters and digits. 
    ///     For example, "THISISASAMPLE" is in the form of the <see cref="UpperCase"/>.
    /// </summary>
    UpperCase = 1,

    /// <summary>
    ///     A name with this convention starts with a letter and contains only lower-case letters and digits. 
    ///     For example, "thisisasample" is in the form of the <see cref="LowerCase"/>.
    /// </summary>
    LowerCase = 2,

    /// <summary>
    ///     A name with this convention starts with a lower-case letter and contains only letters and digits. 
    ///     For example, "thisIsASample" is in the form of the <see cref="CamelCase"/>.
    /// </summary>
    CamelCase = 3,

    /// <summary>
    ///     A name with this convention starts with a upper-case letter and contains only letters and digits. 
    ///     For multi-part names there is no separator in this convention and each part should start with 
    ///     upper-case letter. For example, "ThisIsASample" is in the form of the <see cref="PascalCase"/>.
    /// </summary>
    PascalCase = 4,

    /// <summary>
    ///     A name with this convention starts with a letter or underline(s) and contains only lower-case 
    ///     letters, underline, and digits. For multi-part names, we should separate each part with an underline. 
    ///     For example, "this_is_a_sample" or "_this_is_a_sample" are in the form of 
    ///     the <see cref="LowerSnakeCase"/>.
    /// </summary>
    LowerSnakeCase = 5,

    /// <summary>
    ///     A name with this convention starts with a letter or underline(s) and contains only upper-case 
    ///     letters, underline, and digits. For multi-part names, we should separate each part with an underline. 
    ///     For example, "THIS_IS_A_SAMPLE" or "_THIS_IS_A_SAMPLE" are in the form of 
    ///     the <see cref="UpperSnakeCase"/>.
    /// </summary>
    UpperSnakeCase = 6, 

    /// <summary>
    ///     A name with this convention starts with a letter or underline(s) and contains only letters, underline, 
    ///     and digits. For multi-part names, we should separate each part with an underline, but after that should
    ///     place an upper-case letter. For example, "This_Is_A_Sample" or "_This_Is_A_Sample" are in the form of 
    ///     the <see cref="PascalSnakeCase"/>.
    /// </summary>
    PascalSnakeCase = 7,
}
