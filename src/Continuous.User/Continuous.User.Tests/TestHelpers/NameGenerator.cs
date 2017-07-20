using System;
using System.Security;

namespace Continuous.User.Tests.TestHelpers
{
    public class NameGenerator
    {
        private readonly string _prefix;
        private readonly Random _random;

        public NameGenerator()
        {
            _random = new Random();
        }

        public NameGenerator(string prefix) : this()
        {
            _prefix = prefix;
        }

        public string RandomName => _prefix + _random.Next(0, 5000);

        public SecureString RandomPassword => GetRandomPassword();


        private SecureString GetRandomPassword()
        {
            var s = new SecureString();
            
            s.AppendChar((char) _random.Next(0, 9));
            s.AppendChar((char) _random.Next(0, 9));
            s.AppendChar((char) _random.Next(0, 9));
            s.AppendChar((char) _random.Next(0, 9));
            s.AppendChar((char) _random.Next(0, 9));

            return s;
        }
    }
}