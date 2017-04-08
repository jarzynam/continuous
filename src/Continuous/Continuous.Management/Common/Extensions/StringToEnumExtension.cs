using System;
using System.Text.RegularExpressions;

namespace Continuous.Management.Common.Extensions
{
    internal static class StringToEnumExtension
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s+");
     
        /// <summary>
        /// Trimm input and parse it to related enum
        /// </summary>
        /// <typeparam name="T">related enum</typeparam>
        /// <param name="input">string input</param>
        /// <param name="ignoreCase">if true parsing is not case sensitive</param>
        /// <returns></returns>
        internal static T ToEnum<T>(this string input, bool ignoreCase = true) where T : struct
        {
            var inputWithoutWhiteSpaces = WhiteSpaceRegex.Replace(input, "");

            return (T) Enum.Parse(typeof(T), inputWithoutWhiteSpaces, ignoreCase);
        }
    }
}