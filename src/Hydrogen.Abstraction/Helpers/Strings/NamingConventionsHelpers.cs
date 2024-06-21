using Hydrogen.Abstraction.Exceptions;
using Hydrogen.Abstraction.Helpers.Strings.Enums;
using System.Text;

namespace Hydrogen.Abstraction.Helpers.Strings;

public static class NamingConventionsHelpers
{
    private static void Validate(this string name)
    {
        if (name.Length == 0)
        {
            throw new InvalidParameterException(nameof(name), name, "The name couldn't be empty.");
        }
    }
    private static byte MoveStartIndex(this string name, NamingConventions convention)
    {
        byte startIndex = 0;

        while (name[startIndex] == '_' && startIndex < name.Length)
        {
            startIndex++;

            if (startIndex == byte.MaxValue)
            {
                throw new InvalidParameterException(nameof(name), name, "The name couldn't have lots of underlines at the beginning.");
            }
        }

        return startIndex;
    }
    private static byte ChangeFirstCharacter(this string name, NamingConventions convention, ref byte index, StringBuilder builder)
    {
        if (char.IsLetter(name[index]) == false)
        {
            throw new InvalidParameterException(nameof(name), name, "The name should start with a letter because of the convention.");
        }
        else if (convention is NamingConventions.CamelCase or NamingConventions.LowerCase or NamingConventions.LowerSnakeCase)
        {
            builder.Append(char.ToLowerInvariant(name[index]));
        }
        else
        {
            builder.Append(char.ToUpperInvariant(name[index]));
        }

        return index++;
    }
    private static void RemoveUnderlinesAndSetLetters(this string name, NamingConventions convention, ref byte startIndex, StringBuilder builder)
    {
        StringBuilder ProcessByUpperCaseConvention(byte startIndex)
        {
            for (int index = startIndex; index < name.Length; index++)
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

            return builder;
        }
        StringBuilder ProcessByLowerCaseConvention(byte startIndex)
        {
            for (int index = startIndex; index < name.Length; index++)
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

            return builder;
        }
        StringBuilder ProcessByCamelCaseConvention(byte startIndex)
        {
            for (int index = startIndex; index < name.Length; index++)
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

            return builder;
        }
        StringBuilder ProcessByPascalCaseConvention(byte startIndex)
        {
            for (int index = startIndex; index < name.Length; index++)
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

            return builder;
        }

        _ = convention switch
        {
            NamingConventions.UpperCase => ProcessByUpperCaseConvention(startIndex),
            NamingConventions.LowerCase => ProcessByLowerCaseConvention(startIndex),
            NamingConventions.CamelCase => ProcessByCamelCaseConvention(startIndex),
            NamingConventions.PascalCase => ProcessByPascalCaseConvention(startIndex),
            //TODO: Title case, Upper Title case are not supported yet. 

            _ => throw new InvalidParameterException(nameof(convention), convention, "This naming convention is not supported.")
        };
    }

    public static NamingConventions[] DetectConventions(this string name)
    {
        //TODO: Title case, Upper Title case are not supported yet. 
        List<NamingConventions> result = [];

        bool containsLetters = false;
        bool startsWithLowerCase = false;
        bool startsWithUpperCase = false;
        bool startsWithUnderline = false;
        bool containsUnderline = false;
        bool containsLowerCaseLetters = false;
        bool containsUpperCaseLetters = false;
        bool containsLowerCaseAfterUnderlines = false;
        bool containsUpperCaseAfterUnderlines = false;
        bool containsUpperCaseAfterLowerCase = false;

        NamingConventions[] ProcessSmallNames()
        {
            if (name.Length == 0 || char.IsDigit(name[0]) || (name.Length == 1 && name[0] == '_'))
            {
                return [];
            }
            else if (name.Length == 1 && char.IsLower(name[0]))
            {
                return [NamingConventions.LowerCase];
            }
            else if (name.Length == 1 && char.IsUpper(name[0]))
            {
                return [NamingConventions.UpperCase];
            }

            return [];
        }


        void ProcessName()
        {
            name.Validate();

            containsLetters = char.IsLetter(name[0]);
            startsWithUnderline = containsUnderline = name[0] == '_';
            startsWithLowerCase = containsLowerCaseLetters = char.IsLetter(name[0]) && char.IsLower(name[0]);
            startsWithUpperCase = containsUpperCaseLetters = char.IsLetter(name[0]) && char.IsUpper(name[0]);

            for (var index = 1; index < name.Length - 1; index++)
            {
                if (containsUnderline == false && name[index] == '_')
                {
                    containsUnderline = true;
                }

                if (containsLetters == false && char.IsLetter(name[index]))
                {
                    containsLetters = true;
                }

                if (containsLowerCaseLetters == false && char.IsLower(name[index]))
                {
                    containsLowerCaseLetters = true;
                }

                if (containsUpperCaseLetters == false && char.IsUpper(name[index]))
                {
                    containsUpperCaseLetters = true;
                }

                if (containsLowerCaseAfterUnderlines == false && name[index] == '_' && char.IsLower(name[index + 1]))
                {
                    containsLowerCaseAfterUnderlines = containsLetters = containsLowerCaseLetters = true;
                }

                if (containsUpperCaseAfterUnderlines == false && name[index] == '_' && char.IsUpper(name[index + 1]))
                {
                    containsUpperCaseAfterUnderlines = containsLetters = containsUpperCaseLetters = true;
                }

                if (containsUpperCaseAfterLowerCase == false && char.IsUpper(name[index]) && char.IsLower(name[index - 1]))
                {
                    containsUpperCaseAfterLowerCase = containsLetters = containsUpperCaseLetters = true;
                }
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

        void ProcessUpperCaseConventionCompatibility()
        {
            if (startsWithUpperCase &&
                containsUnderline == false &&
                containsLowerCaseLetters == false)
                result.Add(NamingConventions.UpperCase);
        }
        void ProcessLowerCaseConventionCompatibility()
        {
            if (startsWithLowerCase &&
                containsUnderline == false &&
                containsUpperCaseLetters == false)
                result.Add(NamingConventions.LowerCase);
        }
        void ProcessCamelCaseConventionCompatibility()
        {
            if (startsWithLowerCase &&
                containsUpperCaseLetters &&
                containsUnderline == false)
                result.Add(NamingConventions.CamelCase);
        }
        void ProcessPascalCaseConventionCompatibility()
        {
            if (startsWithUpperCase &&
                containsLowerCaseLetters &&
                containsUnderline == false)
                result.Add(NamingConventions.PascalCase);

        }
        void ProcessLowerSnakeCaseConventionCompatibility()
        {
            if ((startsWithUnderline || startsWithLowerCase) &&
                containsUnderline == true &&
                containsUpperCaseAfterUnderlines == false &&
                containsUpperCaseLetters == false)
                result.Add(NamingConventions.LowerSnakeCase);

        }
        void ProcessUpperSnakeCaseConventionCompatibility()
        {
            if ((startsWithUnderline || startsWithUpperCase) &&
                containsUnderline == true &&
                containsLowerCaseAfterUnderlines == false &&
                containsLowerCaseLetters == false)
                result.Add(NamingConventions.UpperSnakeCase);
        }
        void ProcessPascalSnakeCaseConventionCompatibility()
        {
            if ((startsWithUnderline || startsWithUpperCase) &&
                (startsWithUnderline || containsUpperCaseAfterUnderlines) &&
                containsUnderline == true &&
                containsUpperCaseAfterUnderlines == true &&
                containsUpperCaseAfterLowerCase == false &&
                containsLowerCaseAfterUnderlines == false &&
                containsLowerCaseLetters == true &&
                containsLowerCaseAfterUnderlines == false)
                result.Add(NamingConventions.PascalSnakeCase);

        }

        if (name.Length <= 1)
        {
            return ProcessSmallNames();
        }
        else
        {
            ProcessName();

            if (containsLetters == true)
            {
                ProcessUpperCaseConventionCompatibility();
                ProcessLowerCaseConventionCompatibility();
                ProcessCamelCaseConventionCompatibility();
                ProcessPascalCaseConventionCompatibility();
                ProcessLowerSnakeCaseConventionCompatibility();
                ProcessUpperSnakeCaseConventionCompatibility();
                ProcessPascalSnakeCaseConventionCompatibility();
            }

            return result.ToArray();
        }
    }
    public static bool IsLowerCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.LowerCase);
    public static bool IsUpperCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.UpperCase);
    public static bool IsCamelCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.CamelCase);
    public static bool IsPascalCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.PascalSnakeCase);
    public static bool IsLowerSnakeCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.LowerSnakeCase);
    public static bool IsUpperSnakeCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.UpperSnakeCase);
    public static bool IsPascalSnakeCase(this string name)
        => name.DetectConventions().Contains(NamingConventions.PascalSnakeCase);
    public static string ToUpperCase(this string name)
    {
        name.Validate();

        StringBuilder stringBuilder = new();
        byte startIndex = name.MoveStartIndex(NamingConventions.UpperCase);
        name.ChangeFirstCharacter(NamingConventions.UpperCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndSetLetters(NamingConventions.UpperCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }
    public static string ToLowerCase(this string name)
    {
        name.Validate();

        StringBuilder stringBuilder = new();
        byte startIndex = name.MoveStartIndex(NamingConventions.LowerCase);
        name.ChangeFirstCharacter(NamingConventions.LowerCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndSetLetters(NamingConventions.LowerCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }
    public static string ToCamelCase(this string name)
    {
        name.Validate();

        StringBuilder stringBuilder = new();
        byte startIndex = name.MoveStartIndex(NamingConventions.CamelCase);
        name.ChangeFirstCharacter(NamingConventions.CamelCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndSetLetters(NamingConventions.CamelCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }
    public static string ToPascalCase(this string name)
    {
        name.Validate();

        StringBuilder stringBuilder = new();
        byte startIndex = name.MoveStartIndex(NamingConventions.PascalCase);
        name.ChangeFirstCharacter(NamingConventions.PascalCase, ref startIndex, stringBuilder);
        name.RemoveUnderlinesAndSetLetters(NamingConventions.PascalCase, ref startIndex, stringBuilder);

        return stringBuilder.ToString();
    }
    public static string ToLowerSnakeCase(this string name)
    {
        name.Validate();

        StringBuilder stringBuilder = new();
        var startIndex = name.MoveStartIndex(NamingConventions.LowerSnakeCase);
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
    public static string ToUpperSnakeCase(this string name)
    {
        name.Validate();

        StringBuilder sb = new();
        var startIndex = name.MoveStartIndex(NamingConventions.UpperSnakeCase);
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
    public static string ToPascalSnakeCase(this string name)
    {
        name.Validate();

        StringBuilder sb = new();
        var startIndex = name.MoveStartIndex(NamingConventions.UpperSnakeCase);
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

    //TODO: Title case and Upper Title case are not supported in the helper yet.
    public static bool IsTitleCase(this string name) => throw new NotImplementedException();
    public static bool IsUpperTitleCase(this string name) => throw new NotImplementedException();
    public static string ToTitleCase(this string name) => throw new NotImplementedException();
    public static string ToUpperTitleCase(this string name) => throw new NotImplementedException();
}
