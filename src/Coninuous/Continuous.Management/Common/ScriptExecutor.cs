using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Continuous.Management.Common
{
   public  class ScriptExecutor
    {
        public ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters)
        {
            var runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            var pipeline = runspace.CreatePipeline();
            var cmd = new Command(scriptFullPath);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }
            }
            pipeline.Commands.Add(cmd);
            var results = pipeline.Invoke();

            pipeline.Dispose();
            runspace.Dispose();
            return results;
        }
    }
}
