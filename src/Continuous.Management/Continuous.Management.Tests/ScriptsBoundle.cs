using Continuous.Management.Common;

namespace Continuous.Management.Tests
{
    internal class ScriptsBoundle : BoundleBase
    {
        private readonly string _currentPath;

        public ScriptsBoundle()
        {
            _currentPath = AddToPath(BasePath, "Resources");
        }

        public string ValidScript => AddToPath(_currentPath, "ValidScript.ps1");
        public string InvalidScript => AddToPath(_currentPath, "InvalidScript.ps1");
    }
}