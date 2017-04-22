using System.IO;

namespace Continuous.Test.BasicService
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
