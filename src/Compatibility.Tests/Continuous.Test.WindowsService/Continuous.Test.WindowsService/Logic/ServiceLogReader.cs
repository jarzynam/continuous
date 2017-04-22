using System;
using System.IO;
using System.Linq;

namespace Continuous.Test.WindowsService.Logic
{
    public class ServiceLogReader
    {
        private const string FilePath = @"C:\temp\basicService.txt";

        public const string ServiceStarted = "SERVICE STARTED";
        public const string ServiceFinished = "SERVICE FINISHED";

        public string LastLine()
        {
            var lines = File.ReadAllLines(FilePath);

            return lines.LastOrDefault();
        }

        public void ClearLog()
        {
            File.WriteAllText(FilePath, String.Empty);
        }

    }
}
