using Hydrogen.Abstraction.Helpers.Strings;

namespace Hydrogen.Abstraction.Tests.Helpers.Strings;

public class ConventionConvertorsTests
{
    [Theory]
    #region Inline data
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1")]
    [InlineData("-")]
    [InlineData("_")]
    [InlineData("__")]
    [InlineData(" _")]
    [InlineData("1_")]
    [InlineData("-_")]
    [InlineData("_ ")]
    [InlineData("_1")]
    [InlineData("_-")]
    #endregion
    public void Helpers_Strings_DetectedCases_ShouldNotContain_AnyCase(string value)
    {
        // Arrange and Act
        var detectedCases = value.DetectCases();

        // Assert
        Assert.Empty(detectedCases);
    }

    [Theory]
    #region Inline data
    [InlineData("A", 
        NamingConventions.UpperCase, 
        NamingConventions.PascalCase)]

    [InlineData("a",
        NamingConventions.LowerCase,
        NamingConventions.CamelCase)]

    [InlineData("Aa", 
        NamingConventions.PascalCase)]

    [InlineData("aA",
        NamingConventions.CamelCase)]

    [InlineData("aa",
        NamingConventions.CamelCase,
        NamingConventions.LowerCase)]

    [InlineData("A1",
        NamingConventions.UpperCase,
        NamingConventions.PascalCase)]

    [InlineData("a1",
        NamingConventions.LowerCase,
        NamingConventions.CamelCase)]

    [InlineData("A_",
        NamingConventions.SnakeCase,
        NamingConventions.UpperSnakeCase,
        NamingConventions.PascalSnakeCase)]

    [InlineData("a_",
        NamingConventions.SnakeCase,
        NamingConventions.LowerSnakeCase)]

    [InlineData("A_a",
        NamingConventions.SnakeCase)]

    [InlineData("a_A",
        NamingConventions.SnakeCase)]

    [InlineData("A_A",
        NamingConventions.SnakeCase,
        NamingConventions.PascalSnakeCase,
        NamingConventions.UpperSnakeCase)]

    [InlineData("a_a",
        NamingConventions.SnakeCase,
        NamingConventions.LowerSnakeCase)]

    [InlineData("A_1",
        NamingConventions.SnakeCase,
        NamingConventions.UpperSnakeCase,
        NamingConventions.PascalSnakeCase)]

    [InlineData("a_1",
        NamingConventions.SnakeCase,
        NamingConventions.LowerSnakeCase)]

    [InlineData("AaAa",
        NamingConventions.PascalCase)]

    [InlineData("aAaA",
        NamingConventions.CamelCase)]

    [InlineData("AaAa_",
        NamingConventions.SnakeCase,
        NamingConventions.PascalSnakeCase)]

    [InlineData("aAaA_",
        NamingConventions.SnakeCase)]

    [InlineData("AAAA",
        NamingConventions.PascalCase,
        NamingConventions.UpperCase)]

    [InlineData("aaaa",
        NamingConventions.CamelCase,
        NamingConventions.LowerCase)]

    [InlineData("AAAA_",
        NamingConventions.SnakeCase,
        NamingConventions.UpperSnakeCase,
        NamingConventions.PascalSnakeCase)]

    [InlineData("aaaa_",
        NamingConventions.SnakeCase,
        NamingConventions.LowerSnakeCase)]

    [InlineData("_AAAA",
        NamingConventions.SnakeCase)]

    [InlineData("_aaaa",
        NamingConventions.SnakeCase)]

    [InlineData("_AaAa",
        NamingConventions.SnakeCase)]

    [InlineData("_aAaA",
        NamingConventions.SnakeCase)]

    [InlineData("__aAaA",
        NamingConventions.SnakeCase)]

    [InlineData("__AAAA",
        NamingConventions.SnakeCase)]

    [InlineData("__Aaaa",
        NamingConventions.SnakeCase)]

    #endregion
    public void Helpers_Strings_Result_ShouldContains_ExpectedValues(string value, params NamingConventions[] expected)
    {
        // Arrange and Act
        var result = value.DetectCases();

        // Assert
        foreach (var item in expected)
            Assert.Contains<NamingConventions>(item, result);

        Assert.Equal(expected.Length, result.Length);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "A")]
    [InlineData("A", "A")]
    [InlineData("_a", "A")]
    [InlineData("_A", "A")]
    [InlineData("a_", "A")]
    [InlineData("A_", "A")]
    [InlineData("aa", "AA")]
    [InlineData("AA", "AA")]
    [InlineData("_aa", "AA")]
    [InlineData("_AA", "AA")]
    [InlineData("aa_", "AA")]
    [InlineData("AA_", "AA")]
    [InlineData("aa_a", "AAA")]
    [InlineData("AA_a", "AAA")]
    [InlineData("_aa_a", "AAA")]
    [InlineData("_AA_a", "AAA")]
    [InlineData("aa_a_", "AAA")]
    [InlineData("AA_a_", "AAA")]
    [InlineData("_aa_a_", "AAA")]
    [InlineData("_AA_a_", "AAA")]
    [InlineData("_aa_aa_aa", "AAAAAA")]
    [InlineData("_AA_aa_aa", "AAAAAA")]
    [InlineData("_aa_1a_aa", "AA1AAA")]
    [InlineData("_AA_1a_aa", "AA1AAA")]
    [InlineData("aaa", "AAA")]
    [InlineData("AAA", "AAA")]
    [InlineData("aAaaAaaA", "AAAAAAAA")]
    [InlineData("AaaAaaAaa", "AAAAAAAAA")]
    #endregion
    public void Helpers_Strings_ToUpperCase(string source, string expected)
    {
        var result = source.ToUpperCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "a")]
    [InlineData("A", "a")]
    [InlineData("_a", "a")]
    [InlineData("_A", "a")]
    [InlineData("a_", "a")]
    [InlineData("A_", "a")]
    [InlineData("aa", "aa")]
    [InlineData("AA", "aa")]
    [InlineData("_aa", "aa")]
    [InlineData("_AA", "aa")]
    [InlineData("aa_", "aa")]
    [InlineData("AA_", "aa")]
    [InlineData("aa_a", "aaa")]
    [InlineData("AA_a", "aaa")]
    [InlineData("_aa_a", "aaa")]
    [InlineData("_AA_a", "aaa")]
    [InlineData("aa_a_", "aaa")]
    [InlineData("AA_a_", "aaa")]
    [InlineData("_aa_a_", "aaa")]
    [InlineData("_AA_a_", "aaa")]
    [InlineData("_aa_aa_aa", "aaaaaa")]
    [InlineData("_AA_aa_aa", "aaaaaa")]
    [InlineData("_aa_1a_aa", "aa1aaa")]
    [InlineData("_AA_1a_aa", "aa1aaa")]
    [InlineData("aaa", "aaa")]
    [InlineData("AAA", "aaa")]
    [InlineData("aAaaAaaA", "aaaaaaaa")]
    [InlineData("AaaAaaAaa", "aaaaaaaaa")]
    #endregion
    public void Helpers_Strings_ToLowerCase(string source, string expected)
    {
        var result = source.ToLowerCase();

        Assert.Equal(expected, result);
    }


    [Theory]
    #region Inline data
    [InlineData("a", "a")]
    [InlineData("A", "a")]
    [InlineData("_a", "a")]
    [InlineData("_A", "a")]
    [InlineData("a_", "a")]
    [InlineData("A_", "a")]
    [InlineData("aa", "aa")]
    [InlineData("AA", "aA")]
    [InlineData("_aa", "aa")]
    [InlineData("_AA", "aA")]
    [InlineData("aa_", "aa")]
    [InlineData("AA_", "aA")]
    [InlineData("aa_a", "aaA")]
    [InlineData("AA_a", "aAA")]
    [InlineData("_aa_a", "aaA")]
    [InlineData("_AA_a", "aAA")]
    [InlineData("aa_a_", "aaA")]
    [InlineData("AA_a_", "aAA")]
    [InlineData("_aa_a_", "aaA")]
    [InlineData("_AA_a_", "aAA")]
    [InlineData("_aa_aa_aa", "aaAaAa")]
    [InlineData("_AA_aa_aa", "aAAaAa")]
    [InlineData("_aa_1a_aa", "aa1aAa")]
    [InlineData("_AA_1a_aa", "aA1aAa")]
    [InlineData("aaa", "aaa")]
    [InlineData("AAA", "aAA")]
    [InlineData("aAaaAaaA", "aAaaAaaA")]
    [InlineData("AaaAaaAaa", "aaaAaaAaa")]
    #endregion
    public void Helpers_Strings_ToCamelCase(string source, string expected)
    {
        var result = source.ToCamelCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "A")]
    [InlineData("A", "A")]
    [InlineData("_a", "A")]
    [InlineData("_A", "A")]
    [InlineData("a_", "A")]
    [InlineData("A_", "A")]
    [InlineData("aa", "Aa")]
    [InlineData("AA", "AA")]
    [InlineData("_aa", "Aa")]
    [InlineData("_AA", "AA")]
    [InlineData("aa_", "Aa")]
    [InlineData("AA_", "AA")]
    [InlineData("aa_a", "AaA")]
    [InlineData("AA_a", "AAA")]
    [InlineData("_aa_a", "AaA")]
    [InlineData("_AA_a", "AAA")]
    [InlineData("aa_a_", "AaA")]
    [InlineData("AA_a_", "AAA")]
    [InlineData("_aa_a_", "AaA")]
    [InlineData("_AA_a_", "AAA")]
    [InlineData("_aa_aa_aa", "AaAaAa")]
    [InlineData("_AA_aa_aa", "AAAaAa")]
    [InlineData("_aa_1a_aa", "Aa1aAa")]
    [InlineData("_AA_1a_aa", "AA1aAa")]
    [InlineData("aaa", "Aaa")]
    [InlineData("AAA", "AAA")]
    [InlineData("aAaaAaaA", "AAaaAaaA")]
    [InlineData("AaaAaaAaa", "AaaAaaAaa")]
    #endregion
    public void Helpers_Strings_ToPascalCase(string source, string expected)
    {
        var result = source.ToPascalCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "a")]
    [InlineData("A", "A")]
    [InlineData("_a", "_a")]
    [InlineData("_A", "_A")]
    [InlineData("a_", "a_")]
    [InlineData("A_", "A_")]
    [InlineData("aa", "aa")]
    [InlineData("AA", "A_A")]
    [InlineData("_aa", "_aa")]
    [InlineData("_AA", "_A_A")]
    [InlineData("aa_", "aa_")]
    [InlineData("AA_", "A_A_")]
    [InlineData("aa_a", "aa_a")]
    [InlineData("AA_a", "A_A_a")]
    [InlineData("_aa_a", "_aa_a")]
    [InlineData("_AA_a", "_A_A_a")]
    [InlineData("aa_a_", "aa_a_")]
    [InlineData("AA_a_", "A_A_a_")]
    [InlineData("_aa_a_", "_aa_a_")]
    [InlineData("_AA_a_", "_A_A_a_")]
    [InlineData("_aa_aa_aa", "_aa_aa_aa")]
    [InlineData("_AA_aa_aa", "_A_A_aa_aa")]
    [InlineData("_aa_1a_aa", "_aa_1a_aa")]
    [InlineData("_AA_1a_aa", "_A_A_1a_aa")]
    [InlineData("aaa", "aaa")]
    [InlineData("AAA", "A_A_A")]
    [InlineData("aAaaAaaA", "a_Aaa_Aaa_A")]
    [InlineData("AaaAaaAaa", "Aaa_Aaa_Aaa")]
    #endregion
    public void Helpers_Strings_ToSnakeCase(string source, string expected)
    {
        var result = source.ToSnakeCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "a")]
    [InlineData("A", "a")]
    [InlineData("_a", "a")]
    [InlineData("_A", "a")]
    [InlineData("a_", "a_")]
    [InlineData("A_", "a_")]
    [InlineData("aa", "aa")]
    [InlineData("AA", "a_a")]
    [InlineData("_aa", "aa")]
    [InlineData("_AA", "a_a")]
    [InlineData("aa_", "aa_")]
    [InlineData("AA_", "a_a_")]
    [InlineData("aa_a", "aa_a")]
    [InlineData("AA_a", "a_a_a")]
    [InlineData("_aa_a", "aa_a")]
    [InlineData("_AA_a", "a_a_a")]
    [InlineData("aa_a_", "aa_a_")]
    [InlineData("AA_a_", "a_a_a_")]
    [InlineData("_aa_a_", "aa_a_")]
    [InlineData("_AA_a_", "a_a_a_")]
    [InlineData("_aa_aa_aa", "aa_aa_aa")]
    [InlineData("_AA_aa_aa", "a_a_aa_aa")]
    [InlineData("_aa_1a_aa", "aa_1a_aa")]
    [InlineData("_AA_1a_aa", "a_a_1a_aa")]
    [InlineData("aaa", "aaa")]
    [InlineData("AAA", "a_a_a")]
    [InlineData("aAaaAaaA", "a_aaa_aaa_a")]
    [InlineData("AaaAaaAaa", "aaa_aaa_aaa")]
    #endregion
    public void Helpers_Strings_ToLowerSnakeCase(string source, string expected)
    {
        var result = source.ToLowerSnakeCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "A")]
    [InlineData("A", "A")]
    [InlineData("_a", "A")]
    [InlineData("_A", "A")]
    [InlineData("a_", "A_")]
    [InlineData("A_", "A_")]
    [InlineData("aa", "AA")]
    [InlineData("AA", "A_A")]
    [InlineData("_aa", "AA")]
    [InlineData("_AA", "A_A")]
    [InlineData("aa_", "AA_")]
    [InlineData("AA_", "A_A_")]
    [InlineData("aa_a", "AA_A")]
    [InlineData("AA_a", "A_A_A")]
    [InlineData("_aa_a", "AA_A")]
    [InlineData("_AA_a", "A_A_A")]
    [InlineData("aa_a_", "AA_A_")]
    [InlineData("AA_a_", "A_A_A_")]
    [InlineData("_aa_a_", "AA_A_")]
    [InlineData("_AA_a_", "A_A_A_")]
    [InlineData("_aa_aa_aa", "AA_AA_AA")]
    [InlineData("_AA_aa_aa", "A_A_AA_AA")]
    [InlineData("_aa_1a_aa", "AA_1A_AA")]
    [InlineData("_AA_1a_aa", "A_A_1A_AA")]
    [InlineData("aaa", "AAA")]
    [InlineData("AAA", "A_A_A")]
    [InlineData("aAaaAaaA", "A_AAA_AAA_A")]
    [InlineData("AaaAaaAaa", "AAA_AAA_AAA")]
    #endregion
    public void Helpers_Strings_ToUpperSnakeCase(string source, string expected)
    {
        var result = source.ToUpperSnakeCase();

        Assert.Equal(expected, result);
    }

    [Theory]
    #region Inline data
    [InlineData("a", "A")]
    [InlineData("A", "A")]
    [InlineData("_a", "A")]
    [InlineData("_A", "A")]
    [InlineData("a_", "A_")]
    [InlineData("A_", "A_")]
    [InlineData("aa", "Aa")]
    [InlineData("AA", "A_A")]
    [InlineData("_aa", "Aa")]
    [InlineData("_AA", "A_A")]
    [InlineData("aa_", "Aa_")]
    [InlineData("AA_", "A_A_")]
    [InlineData("aa_a", "Aa_A")]
    [InlineData("AA_a", "A_A_A")]
    [InlineData("_aa_a", "Aa_A")]
    [InlineData("_AA_a", "A_A_A")]
    [InlineData("aa_a_", "Aa_A_")]
    [InlineData("AA_a_", "A_A_A_")]
    [InlineData("_aa_a_", "Aa_A_")]
    [InlineData("_AA_a_", "A_A_A_")]
    [InlineData("_aa_aa_aa", "Aa_Aa_Aa")]
    [InlineData("_AA_aa_aa", "A_A_Aa_Aa")]
    [InlineData("_aa_1a_aa", "Aa_1a_Aa")]
    [InlineData("_AA_1a_aa", "A_A_1a_Aa")]
    [InlineData("aaa", "Aaa")]
    [InlineData("AAA", "A_A_A")]
    [InlineData("aAaaAaaA", "A_Aaa_Aaa_A")]
    [InlineData("AaaAaaAaa", "Aaa_Aaa_Aaa")]
    #endregion
    public void Helpers_Strings_ToPascalSnakeCase(string source, string expected)
    {
        var result = source.ToPascalSnakeCase();

        Assert.Equal(expected, result);
    }
}
