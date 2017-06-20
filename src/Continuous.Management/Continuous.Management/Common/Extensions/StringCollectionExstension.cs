using System.Collections.Generic;
using System.Text;

namespace Continuous.Management.Common.Extensions
{
    internal static class StringCollectionExstension
    {
        /// <summary>
        /// Transorm string collection to flat string separated with custom value
        /// </summary>
        /// <param name="collection">string collection</param>
        /// <param name="elementSeparator">custom value to separate strings</param>
        /// <returns></returns>
        internal static string ToFlatString(this List<string> collection, string elementSeparator = " ")
        {
            var builder = new StringBuilder();

            for (var i = 0; i < collection.Count; i++)
            {
                builder.Append(collection[i]);

                if (i < collection.Count - 1)
                    builder.Append(elementSeparator);
            }

            return builder.ToString();
        }
    }
}
