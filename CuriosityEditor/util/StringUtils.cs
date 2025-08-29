
using System;
using System.Text.RegularExpressions;

namespace CuriosityEditor;

public static class Case {
    public static string ToSnake(string value) => pascalBoundaryRegex.Replace(separatorRegex.Replace(value.Trim(), "_"), "_$1").ToLower();

    public static string ToKebab(string value) => pascalBoundaryRegex.Replace(separatorRegex.Replace(value.Trim(), "-"), "-$1").ToLower();

    public static string ToPascal(string value) => pascalSeparatorRegex.Replace(value.Trim(), match => match.Groups["1"]?.Value?.ToUpper());

    public static string ToCamel(string value) {
        var pascal = ToPascal(value);
        if (pascal.Length > 0) pascal = char.ToLowerInvariant(pascal[0]) + pascal.Substring(1);
        return pascal;
    }

    private static readonly Regex separatorRegex = new("[\\s-_]+");
    private static readonly Regex pascalSeparatorRegex = new("(?:[\\s_-]+|^)(\\w)");
    private static readonly Regex pascalBoundaryRegex = new("(?<!^|_)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])");
}