using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuous.Compability.WindowsService.Logic
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
