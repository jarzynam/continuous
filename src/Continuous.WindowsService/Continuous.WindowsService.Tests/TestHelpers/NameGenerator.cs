using System;

namespace Continuous.WindowsService.Tests.TestHelpers
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


        public string GetRandomName(string prefix)
        {
            return prefix + _random.Next(0, 5000);
        }
    }
}