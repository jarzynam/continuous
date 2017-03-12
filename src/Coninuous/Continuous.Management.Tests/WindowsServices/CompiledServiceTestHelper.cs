using System;
using System.IO;

namespace Continuous.Management.Library.Tests.WindowsServices
{
    internal class CompiledServiceTestHelper
    {
        private readonly Random _random;

        public CompiledServiceTestHelper()
        {
            _random = new Random();
        }

        internal string GetTestServicePath()
        {
            var location = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(location, "WindowsServices", "CompiledTestService",
             "Continuous.EmptyTestService.exe");

            return path;
        }

        internal string RandomServiceName => "TestService" + _random.Next(0, 5000);
    }
}
