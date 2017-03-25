using System.Collections.Generic;

namespace Continuous.User.LocalUserGroups.Model
{
    public class LocalUserGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Members { get; set; }
    }
}
