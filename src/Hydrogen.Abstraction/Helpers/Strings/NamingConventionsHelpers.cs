using Hydrogen.Abstraction.Enums;
using Hydrogen.Abstraction.Exceptions;
using System.Text;

namespace Hydrogen.Abstraction.Helpers.Strings;

/// <summary>
///     This class contains a set of helper methods to identify or convert the naming
///     conventions of members of a class or struct.
/// </summary>
public static class NamingConventionsHelpers
{
    private static void BlockEmptyStrings(this string name, NamingConventions convention)
    {
        if (name.Length == 0)
        {
            throw new InvalidParameterException(nameof(name), name, "The name couldn't be empty or null string.");
        }
    }
    private static byte SkipInitialUnderlines(this string name, NamingConventions convention)
    {
        byte startIndex = 0;

        while (name[startIndex] == '_')
        {
            startIndex++;

            if (startIndex == byte.MaxValue)
            {
                throw new InvalidParameterException(nameof(name), name, "The name couldn't start with lots of underlines.");
            }
        }

        return startIndex;
    }
    private static byte SetFirstLetter(this string name, NamingConventions convention, ref byte startIndex, StringBuilder builder)
    {
        if (char.IsLetter(name[startIndex]) == false)
        {
            throw new InvalidParameterException(nameof(name), name, "The name should start with a letter due to applied convention.");
        }
        else
        {
            if (convention is NamingConventions.CamelCase or NamingConventions.LowerCase or NamingConventions.LowerSnakeCase)
            {
                builder.Append(char.ToLowerInvariant(name[startIndex]));
            }
            else
            {
                builder.Append(char.ToUpperInvariant(name[startIndex]));
            }

            startIndex++;
        }

        return startIndex;
    }
    private static void RemoveUnderlinesAndManipulateLetters(this string name, NamingConventions convention, ref byte startIndex, StringBuilder builder)
    {
        for (int index = startIndex; index < name.Length; index++)
        {
            if (convention == NamingConventions.UpperCase)
            {
                if (name[index] != '_')
                {
                    builder.Append(char.ToUpperInvariant(name[index]));
                }
                else if (index < name.Length - 1)
                {
                    index++;
                    builder.Append(char.ToUpperInvariant(name[index]));
                }
            }
            else if (convention == NamingConventions.LowerCase)
            {
                if (name[index] != '_')
                {
                    builder.Append(char.ToLowerInvariant(name[index]));
                }
                else if (index < name.Length - 1)
                {
                    index++;
                    builder.Append(char.ToLowerInvariant(name[index]));
                }
            }
            else if (convention == NamingConventions.CamelCase)
            {
                if (name[index] != '_')
                {
                    builder.Append(name[index]);
                }
                else if (index < name.Length - 1)
                {
                    index++;
                    builder.Append(char.ToUpperInvariant(name[index]));
                }
            }
            else if (convention == NamingConventions.PascalCase)
            {
                if (name[index] != '_')
                {
                    builder.Append(name[index]);
                }
                else if (index < name.Length - 1)
                {
                    index++;
                    builder.Append(char.ToUpperInvariant(name[index]));
                }
            }
        }
    }

    /// <summary>
    ///     This method will be used to get a list of all conventions that are compatible with the name. 
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>The list of compatible naming conventions</returns>
    public static NamingConventions[] DetectConventions(this string name)
    {
        List<NamingConventions> result =
        [
            NamingConventions.UpperCase,
            NamingConventions.LowerCase,
            NamingConventions.CamelCase,
            NamingConventions.PascalCase,
            NamingConventions.LowerSnakeCase,
            NamingConventions.UpperSnakeCase,
            NamingConventions.PascalSnakeCase,
        ];

        bool containsLetters = false;
        bool startsWithLowerCase = false;
        bool startsWithUnderline = false;
        bool containsUnderline = false;
        bool containsLowerCaseLetters = false;
        bool containsUpperCaseLetters = false;
        bool containsLowerCaseAfterUnderlines = false;
        bool containsUpperCaseAfterUnderlines = false;

        //bool containsLowerCaseAfterUperCase = false;
        bool containsUpperCaseAfterLowerCase = false;

        if (name.Length == 0 || char.IsDigit(name[0]))
        {
            return [];
        }

        startsWithUnderline = name[0] == '_';
        containsUnderline = startsWithUnderline;
        startsWithLowerCase = char.IsLetter(name[0]) && char.IsLower(name[0]);
        containsLowerCaseLetters = startsWithLowerCase;
        containsUpperCaseLetters = !startsWithLowerCase && !startsWithUnderline;

        containsLetters = char.IsLetter(name[0]);

        if (name.Length > 1)
        {
            for (var index = 1; index < name.Length - 1; index++)
            {
                if (containsUnderline == false && name[index] == '_') 
                    containsUnderline = true;
               
                if (containsLetters == false && char.IsLetter(name[index])) 
                    containsLetters = true;
                
                if (containsLowerCaseLetters == false && char.IsLower(name[index]))
                    containsLowerCaseLetters = true;
                
                if (containsUpperCaseLetters == false && char.IsUpper(name[index])) 
                    containsUpperCaseLetters = true;
                
                if (containsLowerCaseAfterUnderlines == false && name[index] == '_' && char.IsLower(name[index + 1])) 
                    containsLowerCaseAfterUnderlines = containsLetters = containsLowerCaseLetters = true;
                
                if (containsUpperCaseAfterUnderlines == false && name[index] == '_' && char.IsUpper(name[index + 1])) 
                    containsUpperCaseAfterUnderlines = containsLetters = containsUpperCaseLetters = true;

                if (containsUpperCaseAfterLowerCase == false && char.IsUpper(name[index ]) && char.IsLower( name[index - 1] ))
                    containsUpperCaseAfterLowerCase = containsLetters = containsUpperCaseLetters = true;
            }

            if (containsUnderline == false && name.Last() == '_') 
                containsUnderline = true;
            
            if (containsLetters == false && char.IsLetter(name.Last())) 
                containsLetters = true;
            
            if (containsLowerCaseLetters == false && char.IsLower(name.Last())) 
                containsLowerCaseLetters = containsLetters = true;
            
            if (containsUpperCaseLetters == false && char.IsUpper(name.Last())) 
                containsUpperCaseLetters = containsLetters = true;
        }

        if (containsLetters == false)
        {
            return [];
        }

        if (startsWithUnderline == true)
        {
            result.Remove(NamingConventions.UpperCase);
            result.Remove(NamingConventions.LowerCase);
            result.Remove(NamingConventions.CamelCase);
            result.Remove(NamingConventions.PascalCase);
        }
        else if (startsWithLowerCase == true)
        {
            result.Remove(NamingConventions.UpperCase);
            result.Remove(NamingConventions.PascalCase);
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }
        else
        {
            result.Remove(NamingConventions.LowerCase);
            result.Remove(NamingConventions.CamelCase);
            result.Remove(NamingConventions.LowerSnakeCase);
        }

        if (containsUpperCaseLetters == false)
        {
            result.Remove(NamingConventions.UpperCase);
            result.Remove(NamingConventions.PascalCase);
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }
        else
        {
            result.Remove(NamingConventions.LowerCase);
            result.Remove(NamingConventions.LowerSnakeCase);
        }

        if (containsLowerCaseLetters == false)
        {
            result.Remove(NamingConventions.LowerCase);
            result.Remove(NamingConventions.CamelCase);
            result.Remove(NamingConventions.LowerSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }
        else
        {
            result.Remove(NamingConventions.UpperCase);
            result.Remove(NamingConventions.UpperSnakeCase);
        }

        if (containsUnderline)
        {
            result.Remove(NamingConventions.PascalCase);
            result.Remove(NamingConventions.CamelCase);
            result.Remove(NamingConventions.UpperCase);
            result.Remove(NamingConventions.LowerCase);
        }
        else
        {
            result.Remove(NamingConventions.LowerSnakeCase);
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }

        if(containsUpperCaseAfterLowerCase)
        {
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }

        if (containsUpperCaseAfterUnderlines)
        {
            result.Remove(NamingConventions.LowerSnakeCase);
        }

        if (containsLowerCaseAfterUnderlines)
        {
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
        }

        return result.ToArray();
    }

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.LowerCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsLowerCase(this string name) => name.DetectConventions().Contains(NamingConventions.LowerCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.UpperCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsUpperCase(this string name) => name.DetectConventions().Contains(NamingConventions.UpperCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.CamelCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsCamelCase(this string name) => name.DetectConventions().Contains(NamingConventions.CamelCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.PascalCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsPascalCase(this string name) => name.DetectConventions().Contains(NamingConventions.PascalSnakeCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.LowerSnakeCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsLowerSnakeCase(this string name) => name.DetectConventions().Contains(NamingConventions.LowerSnakeCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.UpperSnakeCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsUpperSnakeCase(this string name) => name.DetectConventions().Contains(NamingConventions.UpperSnakeCase);

    /// <summary>
    ///     This method indicates that the name is compatible with <see cref="NamingConventions.PascalSnakeCase"/> or not.
    /// </summary>
    /// <param name="name">The name that should be checked</param>
    /// <returns>return true if the name is compatible; otherwise returns false.</returns>
    public static bool IsPascalSnakeCase(this string name) => name.DetectConventions().Contains(NamingConventions.PascalSnakeCase);

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.UpperCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToUpperCase(this string name)
    {
        StringBuilder stringBuilder = new();

        name.BlockEmptyStrings(NamingConventions.UpperCase);

        byte startIndex = name.SkipInitialUnderlines(NamingConventions.UpperCase);
        name.SetFirstLetter(NamingConventions.UpperCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndManipulateLetters(NamingConventions.UpperCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.LowerCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToLowerCase(this string name)
    {
        StringBuilder stringBuilder = new();

        name.BlockEmptyStrings(NamingConventions.LowerCase);

        byte startIndex = name.SkipInitialUnderlines(NamingConventions.LowerCase);
        name.SetFirstLetter(NamingConventions.LowerCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndManipulateLetters(NamingConventions.LowerCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.CamelCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToCamelCase(this string name)
    {
        StringBuilder stringBuilder = new();

        name.BlockEmptyStrings(NamingConventions.CamelCase);

        byte startIndex = name.SkipInitialUnderlines(NamingConventions.CamelCase);
        name.SetFirstLetter(NamingConventions.CamelCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndManipulateLetters(NamingConventions.CamelCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.PascalCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToPascalCase(this string name)
    {
        StringBuilder stringBuilder = new();

        name.BlockEmptyStrings(NamingConventions.PascalCase);

        byte startIndex = name.SkipInitialUnderlines(NamingConventions.PascalCase);
        name.SetFirstLetter(NamingConventions.PascalCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndManipulateLetters(NamingConventions.PascalCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.LowerSnakeCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToLowerSnakeCase(this string name)
    {
        StringBuilder stringBuilder = new();

        name.BlockEmptyStrings(NamingConventions.LowerSnakeCase);
        var startIndex = name.SkipInitialUnderlines(NamingConventions.LowerSnakeCase);

        stringBuilder.Append(char.ToLowerInvariant(name[startIndex]));
        startIndex++;

        for (int index = startIndex; index < name.Length; index++)
        {
            if (char.IsLetter(name[index]) && char.IsUpper(name[index]) && name[index - 1] != '_')
            {
                stringBuilder.Append('_');
            }

            stringBuilder.Append(char.ToLowerInvariant(name[index]));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.UpperSnakeCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToUpperSnakeCase(this string name)
    {
        StringBuilder sb = new();

        name.BlockEmptyStrings(NamingConventions.UpperSnakeCase);
        var startIndex = name.SkipInitialUnderlines(NamingConventions.UpperSnakeCase);

        sb.Append(char.ToUpperInvariant(name[startIndex]));
        startIndex++;

        for (int index = startIndex; index < name.Length; index++)
        {
            if (char.IsLetter(name[index]) && char.IsUpper(name[index]) && name[index - 1] != '_')
            {
                sb.Append('_');
            }

            sb.Append(char.ToUpperInvariant(name[index]));
        }

        return sb.ToString();
    }

    /// <summary>
    ///     This method will be used to make the name compatible with <see cref="NamingConventions.PascalSnakeCase"/> convention.
    /// </summary>
    /// <param name="name">The name that should be changed</param>
    /// <returns>Returns the name with new naming convention and applied changes.</returns>
    public static string ToPascalSnakeCase(this string name)
    {
        StringBuilder sb = new();

        name.BlockEmptyStrings(NamingConventions.UpperCase);
        var startIndex = name.SkipInitialUnderlines(NamingConventions.UpperSnakeCase);

        sb.Append(char.ToUpperInvariant(name[startIndex]));
        startIndex++;

        for (int index = startIndex; index < name.Length; index++)
        {
            if (char.IsLetter(name[index]) && name[index - 1] == '_')
            {
                sb.Append(char.ToUpperInvariant(name[index]));
            }
            else if (char.IsLetter(name[index]) && char.IsUpper(name[index]))
            {
                sb.Append('_');
                sb.Append(name[index]);
            }
            else
            {
                sb.Append(name[index]);
            }
        }

        return sb.ToString();
    }
}
