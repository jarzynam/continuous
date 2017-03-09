using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using Continuous.Management.Common.Extensions;

namespace Continuous.Management.LocalUserGroup
{
    internal class Mapper
    {
        private readonly Regex _wihteSpaceSeparatorRegex = new Regex(@"[\s]{2,}");
        private readonly Regex _lineBeforeUsersRegex = new Regex("[-]{2,}");
        private readonly Regex _lineAfterUsersRegex = new Regex("The command completed successfully.");

        private readonly int nameIndex = 0;
        private readonly int valueIndex = 1;
        private readonly char memberSeparator = '\n';

        private const string MemberProperty = "Member";
        private const string NameProperty = "Alias name";
        private const string DescriptionProperty = "Comment";


        internal Model.LocalUserGroup Map(ICollection<PSObject> psObjects)
        {
            var results = psObjects.Select(p => p.BaseObject.ToString()).ToList();

            var properties = new Dictionary<string, string>();

            ExtractProperties(ref properties, results);
            FindMembers(ref properties, results);

            return Map(properties);
        }

        private void FindMembers(ref Dictionary<string, string> properties, List<string> resultStrings)
        {
            var startIndex = resultStrings.FindIndex(_lineBeforeUsersRegex.IsMatch) + 1;
            var endIndex = resultStrings.FindIndex(_lineAfterUsersRegex.IsMatch);

            var members = startIndex > 0 && endIndex > startIndex
                ? resultStrings.GetRange(startIndex, endIndex - startIndex).ToFlatString(memberSeparator.ToString())
                : String.Empty;

            properties.Add(MemberProperty, members);   
        }

        private void ExtractProperties(ref Dictionary<string, string> properties,
            List<string> resultStrings)
        {
            foreach (var result in resultStrings)
            {
                var propertiesList = _wihteSpaceSeparatorRegex.Split(result);

                if (propertiesList.Length > valueIndex)
                {
                    properties.Add(propertiesList[nameIndex], propertiesList[valueIndex]);
                }
            }
        }

        private Model.LocalUserGroup Map(Dictionary<string, string> properties)
        {
            return new Model.LocalUserGroup
            {
                Name = properties[NameProperty],
                Description = properties[DescriptionProperty],
                Members = properties[MemberProperty]
                    .Split(memberSeparator)
                    .Where(p => !String.IsNullOrEmpty(p))
                    .ToList()
            };
        }
    }
}