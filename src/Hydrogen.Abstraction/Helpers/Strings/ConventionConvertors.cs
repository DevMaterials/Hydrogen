using Hydrogen.Abstraction.Exceptions;
using System.Text;

namespace Hydrogen.Abstraction.Helpers.Strings;

public enum NamingConventions
{
    UpperCase = 1,          // without underline, just upper case letters and digits
    LowerCase = 2,          // without underline, just lower case letters and digits
    CamelCase = 3,          // without underline, upper case letters followed by lower case letters or digits, but starts with lower case letter
    SnakeCase = 4,          // with underline, letters and digits
    PascalCase = 5,         // without underline, upper case letters followed by lower case letters or digits
    LowerSnakeCase = 6,     // with underline, lower case letters and digits
    UpperSnakeCase = 7,     // with underline, upper case letters and digits
    PascalSnakeCase = 8,    // with underline, upper case letters after underlines, lower case letters or digits after upper case letters
}

public static class ConventionConvertors
{
    private static void GuardAgainstEmptyStrings(string value, NamingConventions convention)
    {
        if (value.Length == 0)
        {
            throw new ConventionChangeException(value, convention);
        }
    }
    private static byte SkipInitialUnderlines(string value, NamingConventions convention)
    {
        byte startIndex = 0;

        while (value[startIndex] == '_')
        {
            startIndex++;

            if (startIndex == byte.MaxValue)
            {
                throw new ConventionChangeException(value, convention);
            }
        }

        return startIndex;
    }
    private static byte SetFirstLetter(string value, NamingConventions convention, byte startIndex, StringBuilder sb)
    {
        if (char.IsLetter(value[startIndex]) == false)
        {
            throw new ConventionChangeException(value, convention);
        }
        else
        {
            if (convention is NamingConventions.CamelCase or NamingConventions.LowerCase or NamingConventions.LowerSnakeCase)
            {
                sb.Append(char.ToLowerInvariant(value[startIndex]));
            }
            else
            {
                sb.Append(char.ToUpperInvariant(value[startIndex]));
            }

            startIndex++;
        }

        return startIndex;
    }
    private static void RemoveUnderlinesAndManipulateFollowingLetters(string value, NamingConventions convention, byte startIndex, StringBuilder sb)
    {
        for (int index = startIndex; index < value.Length; index++)
        {
            if (convention == NamingConventions.UpperCase)
            {
                if (value[index] != '_')
                {
                    sb.Append(char.ToUpperInvariant(value[index]));
                }
                else if (index < value.Length - 1)
                {
                    index++;
                    sb.Append(char.ToUpperInvariant(value[index]));
                }
            }
            else if (convention == NamingConventions.LowerCase)
            {
                if (value[index] != '_')
                {
                    sb.Append(char.ToLowerInvariant(value[index]));
                }
                else if (index < value.Length - 1)
                {
                    index++;
                    sb.Append(char.ToLowerInvariant(value[index]));
                }
            }
            else if (convention == NamingConventions.CamelCase)
            {
                if (value[index] != '_')
                {
                    sb.Append(value[index]);
                }
                else if (index < value.Length - 1)
                {
                    index++;
                    sb.Append(char.ToUpperInvariant(value[index]));
                }
            }
            else if (convention == NamingConventions.PascalCase)
            {
                if (value[index] != '_')
                {
                    sb.Append(value[index]);
                }
                else if (index < value.Length - 1)
                {
                    index++;
                    sb.Append(char.ToUpperInvariant(value[index]));
                }
            }
        }
    }

    public static NamingConventions[] DetectCases(this string value)
    {
        List<NamingConventions> result =
        [
            NamingConventions.UpperCase,
            NamingConventions.LowerCase,
            NamingConventions.CamelCase,
            NamingConventions.SnakeCase,
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

        if (value.Length == 0 || char.IsDigit(value[0]))
        {
            return [];
        }

        startsWithUnderline = value[0] == '_';
        containsUnderline = startsWithUnderline;
        startsWithLowerCase = char.IsLetter(value[0]) && char.IsLower(value[0]);
        containsLowerCaseLetters = startsWithLowerCase;
        containsUpperCaseLetters = !startsWithLowerCase && !startsWithUnderline;

        containsLetters = char.IsLetter(value[0]);

        if (value.Length > 1)
        {
            for (var index = 1; index < value.Length - 1; index++)
            {
                if (containsUnderline == false && value[index] == '_') containsUnderline = true;
                if (containsLetters == false && char.IsLetter(value[index])) containsLetters = true;
                if (containsLowerCaseLetters == false && char.IsLower(value[index])) containsLowerCaseLetters = true;
                if (containsUpperCaseLetters == false && char.IsUpper(value[index])) containsUpperCaseLetters = true;
                if (containsLowerCaseAfterUnderlines == false && value[index] == '_' && char.IsLower(value[index + 1])) containsLowerCaseAfterUnderlines = true;
                if (containsUpperCaseAfterUnderlines == false && value[index] == '_' && char.IsUpper(value[index + 1])) containsUpperCaseAfterUnderlines = true;
                if (containsLowerCaseAfterUnderlines == false && value[index - 1] == '_' && char.IsLower(value[index + 1])) containsLowerCaseAfterUnderlines = true;
                if (containsUpperCaseAfterUnderlines == false && value[index - 1] == '_' && char.IsUpper(value[index + 1])) containsUpperCaseAfterUnderlines = true;
            }

            if (containsUnderline == false && value.Last() == '_') containsUnderline = true;
            if (containsLetters == false && char.IsLetter(value.Last())) containsLetters = true;
            if (containsLowerCaseLetters == false && char.IsLower(value.Last())) containsLowerCaseLetters = true;
            if (containsUpperCaseLetters == false && char.IsUpper(value.Last())) containsUpperCaseLetters = true;
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
            result.Remove(NamingConventions.LowerSnakeCase);
            result.Remove(NamingConventions.UpperSnakeCase);
            result.Remove(NamingConventions.PascalSnakeCase);
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
            result.Remove(NamingConventions.SnakeCase);
            result.Remove(NamingConventions.LowerSnakeCase);
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

        if (result.Count == 0)
        {
            return [];
        }

        return result.ToArray();
    }
    public static bool IsLowerCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.LowerCase);
    }
    public static bool IsUpperCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.UpperCase);
    }
    public static bool IsCamelCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.CamelCase);
    }
    public static bool IsSnakeCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.SnakeCase);
    }
    public static bool IsPascalCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.PascalSnakeCase);
    }
    public static bool IsLowerSnakeCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.LowerSnakeCase);
    }
    public static bool IsUpperSnakeCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.UpperSnakeCase);
    }
    public static bool IsPascalSnakeCase(this string value)
    {
        return value.DetectCases().Contains(NamingConventions.PascalSnakeCase);
    }
    public static string ToUpperCase(this string value)
    {
        byte startIndex = 0;
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.UpperCase);

        startIndex = SkipInitialUnderlines(value, NamingConventions.UpperCase);

        startIndex = SetFirstLetter(value, NamingConventions.UpperCase, startIndex, sb);

        RemoveUnderlinesAndManipulateFollowingLetters(value, NamingConventions.UpperCase, startIndex, sb);

        return sb.ToString();
    }
    public static string ToLowerCase(this string value)
    {
        byte startIndex = 0;
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.LowerCase);

        startIndex = SkipInitialUnderlines(value, NamingConventions.LowerCase);

        startIndex = SetFirstLetter(value, NamingConventions.LowerCase, startIndex, sb);

        RemoveUnderlinesAndManipulateFollowingLetters(value, NamingConventions.LowerCase, startIndex, sb);

        return sb.ToString();
    }
    public static string ToCamelCase(this string value)
    {
        byte startIndex = 0;
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.CamelCase);

        startIndex = SkipInitialUnderlines(value, NamingConventions.CamelCase);

        startIndex = SetFirstLetter(value, NamingConventions.CamelCase, startIndex, sb);

        RemoveUnderlinesAndManipulateFollowingLetters(value, NamingConventions.CamelCase, startIndex, sb);

        return sb.ToString();
    }
    public static string ToSnakeCase(this string value)
    {
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.UpperCase);

        sb.Append(value[0]);

        for (int index = 1; index < value.Length; index++)
        {
            if (char.IsLetter(value[index]) && char.IsUpper(value[index]) && value[index - 1] != '_')
            {
                sb.Append('_');
            }

            sb.Append(value[index]);
        }

        return sb.ToString();
    }
    public static string ToPascalCase(this string value)
    {
        byte startIndex = 0;
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.PascalCase);

        startIndex = SkipInitialUnderlines(value, NamingConventions.PascalCase);

        startIndex = SetFirstLetter(value, NamingConventions.PascalCase, startIndex, sb);

        RemoveUnderlinesAndManipulateFollowingLetters(value, NamingConventions.PascalCase, startIndex, sb);

        return sb.ToString();
    }
    public static string ToLowerSnakeCase(this string value)
    {
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.LowerSnakeCase);

        var startIndex = SkipInitialUnderlines(value, NamingConventions.LowerSnakeCase);

        sb.Append(char.ToLowerInvariant(value[startIndex]));
        startIndex++;

        for (int index = startIndex; index < value.Length; index++)
        {
            if (char.IsLetter(value[index]) && char.IsUpper(value[index]) && value[index - 1] != '_')
            {
                sb.Append('_');
            }

            sb.Append(char.ToLowerInvariant(value[index]));
        }

        return sb.ToString();
    }
    public static string ToUpperSnakeCase(this string value)
    {
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.UpperSnakeCase);

        var startIndex = SkipInitialUnderlines(value, NamingConventions.UpperSnakeCase);

        sb.Append(char.ToUpperInvariant(value[startIndex]));
        startIndex++;

        for (int index = startIndex; index < value.Length; index++)
        {
            if (char.IsLetter(value[index]) && char.IsUpper(value[index]) && value[index - 1] != '_')
            {
                sb.Append('_');
            }

            sb.Append(char.ToUpperInvariant(value[index]));
        }

        return sb.ToString();
    }
    public static string ToPascalSnakeCase(this string value)
    {
        StringBuilder sb = new();

        GuardAgainstEmptyStrings(value, NamingConventions.UpperCase);

        var startIndex = SkipInitialUnderlines(value, NamingConventions.UpperSnakeCase);

        sb.Append(char.ToUpperInvariant(value[startIndex]));
        startIndex++;

        for (int index = startIndex; index < value.Length; index++)
        {
            if (char.IsLetter(value[index]) && value[index - 1] == '_')
            {
                sb.Append(char.ToUpperInvariant(value[index]));
            }
            else if (char.IsLetter(value[index]) && char.IsUpper(value[index]))
            {
                sb.Append('_');
                sb.Append(value[index]);
            }
            else
            {
                sb.Append(value[index]);
            }
        }

        return sb.ToString();
    }
}
