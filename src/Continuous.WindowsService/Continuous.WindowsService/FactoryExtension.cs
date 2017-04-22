using Continuous.Management;
using Continuous.WindowsService.Shell;

namespace Continuous.WindowsService
{
    public static class FactoryExtension
    {
        public static IWindowsServiceShell WindowsServiceShell(this ContinuousManagementFactory factory)
        {
            return new WindowsServiceShell();
        }
    }
}
