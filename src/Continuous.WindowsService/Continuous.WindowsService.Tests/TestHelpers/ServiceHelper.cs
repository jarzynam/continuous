using System;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;
using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Tests.TestHelpers
{
    internal static class ServiceHelper
    {
        private const int WaitInSeconds = 5;

        internal static int GetErrorControl(string serviceName)
        {
            var result = GetProperty(serviceName, "ErrorControl");

            return Int32.Parse(result.ToString());
        }

        internal static int GetServiceType(string serviceName)
        {
            var result = GetProperty(serviceName, "Type");

            return Int32.Parse(result.ToString());
        }

        internal static int GetStartMode(string serviceName)
        {
            var result = GetProperty(serviceName, "Start");

            return Int32.Parse(result.ToString());
        }

        internal static string GetPath(string serviceName)
        {
            return GetProperty(serviceName, "ImagePath").ToString();
        }

        internal static string GetDisplayName(string serviceName)
        {
            return GetProperty(serviceName, "DisplayName").ToString();
        }

        internal static string GetAccount(string serviceName)
        {
            return GetProperty(serviceName, "ObjectName").ToString();
        }

        private static PSObject GetProperty(string serviceName, string property)
        {
            string command = $@"(Get-ItemProperty 'HKLM:\SYSTEM\CurrentControlSet\Services\{serviceName}').{property}";

            return ScriptInvoker.InvokeScript(command).FirstOrDefault();
        }


        internal static void StartService(string name)
        {
            var service = new ServiceController(name);

            service.Start();

            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(WaitInSeconds));
        }

        internal static void StopService(string name)
        {
            var service  = new ServiceController(name);

            service.Stop();

            service.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        internal static void PauseService(string serviceName, bool waitForStatus = true)
        {
            var service = new ServiceController(serviceName);

            service.Pause();

            if (waitForStatus)
            {
                service.WaitForStatus(ServiceControllerStatus.Paused);
            }
        }

        internal static WindowsServiceState GetState(string serviceName)
        {
            var service = new ServiceController(serviceName);

            return (WindowsServiceState) Enum.Parse(typeof(WindowsServiceState), service.Status.ToString());
        }

        internal static WindowsServiceTestModel GetService(string name)
        {
            var model = new WindowsServiceTestModel
            {
                Name = name,
                DisplayName = GetDisplayName(name),
                ErrorControl = (WindowsServiceErrorControl) GetErrorControl(name),
                Type = (WindowsServiceType) GetServiceType(name),
                StartMode = (WindowsServiceStartMode) GetStartMode(name),
                Account = GetAccount(name),
                Path = GetPath(name)
            };

            return model;
        }
    }

    internal class WindowsServiceTestModel
    {
        internal string Name { get; set; }
        internal string DisplayName { get; set; }
        internal WindowsServiceErrorControl ErrorControl { get; set; }
        internal WindowsServiceType Type { get; set; }
        internal WindowsServiceStartMode StartMode { get; set; }
        internal string Account { get; set; }
        public string Path { get; set; }
    }
}