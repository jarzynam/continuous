using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Continuous.Management.Common
{
   internal  class ScriptExecutor
    {
        internal ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters)
        {
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();
                using (var pipeline = runspace.CreatePipeline())
                {
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

                 ThrowErrorIfNecessary(pipeline);

                    return results;
                }
            }
        }

        private void ThrowErrorIfNecessary(Pipeline pipeline)
        {
            if (pipeline.HadErrors)
            {
                var errors = pipeline.Error.ReadToEnd();
                var errorBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    errorBuilder.AppendLine(error.ToString());
                }
                
                throw new InvalidOperationException(errorBuilder.ToString());
            }
        }
    }
}
