using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuous.CompabilityTests.BasicService
{
    public class FileWriter
    {
        private readonly string _filePath = @"C:\temp\basicService.txt";

        public void LogStart()
        {
            File.WriteAllText(_filePath, "SERVICE STARTED");
        }

        public void LogEnd()
        {
            File.WriteAllText(_filePath, "SERVICE FINISHED");
        }
    }
}
