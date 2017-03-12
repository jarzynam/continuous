using System;

namespace Continuous.Management.Users.Model
{
    public class User
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public DateTime? Expires { get; set; }
    }
}
