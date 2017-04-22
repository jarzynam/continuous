using System;
using System.Linq;
using System.Management.Automation;
using Continuous.Test.WindowsService.Logic;

namespace Continuous.Test.WindowsService.TestHelpers
{
    internal static class ServiceHelper
    {
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

       
        
    }
}