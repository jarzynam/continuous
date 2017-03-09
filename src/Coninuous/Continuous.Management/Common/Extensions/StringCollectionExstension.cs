using System.Collections.Generic;
using System.Text;

namespace Continuous.Management.Common.Extensions
{
    internal static class StringCollectionExstension
    {
        public static string ToFlatString(this List<string> collection, string elementSeparator = " ")
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
