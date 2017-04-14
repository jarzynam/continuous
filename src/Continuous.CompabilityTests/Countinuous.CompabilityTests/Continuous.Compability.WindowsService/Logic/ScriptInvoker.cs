using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Continuous.Compability.WindowsService.Logic
{
    public static class ScriptInvoker
    {
        public static Collection<PSObject> InvokeScript(string script)
        {
            using (var instance = PowerShell.Create())
            {
                instance.AddScript(script);

                var results = instance.Invoke();

                return results;
            }
        }


    }
}
