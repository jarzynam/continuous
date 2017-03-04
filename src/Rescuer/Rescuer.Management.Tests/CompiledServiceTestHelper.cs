using System;
using System.IO;

namespace Rescuer.Management.Tests
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
            var path = Path.Combine(location, "CompiledTestService",
             "Rescuer.Services.EmptyTestService.exe");

            return path;
        }

        internal string RandomServiceName => "TestService" + _random.Next(0, 5000);
    }
}
