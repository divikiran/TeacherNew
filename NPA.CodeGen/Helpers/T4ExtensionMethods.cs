using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPA.CodeGen
{
    /// <summary>
    /// Handful of extension methods used in the code generation process.
    /// </summary>
    public static class T4ExtensionMethods
    {

        public static string SplitUpperCasedString(this string input)
        {
            return Regex.Replace(input, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]|(?<=^])[A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string ParamCase(this string input)
        {
            var m = Regex.Match(input, @"^([A-Z]+).*$");

            if (m.Success)
            {
                var length = m.Groups[1].Value.Length;

                var ca = input.ToCharArray();

                if (length == 1)
                {
                    ca[0] = Char.ToLowerInvariant(ca[0]);
                }
                else
                {
                    for (var i = 0; i < length - 1; i++)
                    {
                        ca[i] = Char.ToLowerInvariant(ca[i]);
                    }
                }

                return new string(ca);
            }

            return input;
        }

        public static string CleanIdentifierName(this string input)
        {
            input = input.Replace("#", "Number").Replace("&", "And");

            return Regex.Replace(input, @"[^A-Za-z0-9_]", String.Empty);
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (Regex.IsMatch(input, "^[A-Z][a-z]+$|^[A-Z][^A-Z]*[A-Z][A-Za-z]*$"))
                return input;

            var ti = new CultureInfo("en-US").TextInfo;

            return ti.ToTitleCase(input.ToLower());
        }
    }
}
