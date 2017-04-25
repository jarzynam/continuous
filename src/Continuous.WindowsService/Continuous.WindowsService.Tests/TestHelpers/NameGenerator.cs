using System;

namespace Continuous.WindowsService.Tests.TestHelpers
{
    public class NameGenerator
    {
        private readonly Random _random;

        public NameGenerator()
        {
            _random = new Random();
        }

        public string GetRandomName(string prefix)
        {
            return prefix + _random.Next(0, 5000);
        }
    }
}
