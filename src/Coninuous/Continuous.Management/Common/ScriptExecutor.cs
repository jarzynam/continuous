using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;

namespace Continuous.Management.Common
{
    internal interface IScriptExecutor
    {
        ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters);
    }

    internal class ScriptExecutor : IScriptExecutor
    {
        private readonly IEmbededFileReader _embededFileReader;

        public ScriptExecutor()
        {
            _embededFileReader = new EmbededFileReader();
        }

        public ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters)
        {
            var script = _embededFileReader.Read(scriptFullPath, Assembly.GetCallingAssembly());

            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (var pipeline = runspace.CreatePipeline())
                {
                    var cmd = CreateCommand(parameters, script);

                    pipeline.Commands.Add(cmd);

                    var results = pipeline.Invoke();

                    ThrowErrorIfNecessary(pipeline);

                    return results;
                }
            }
        }

        private static Command CreateCommand(ICollection<CommandParameter> parameters, string script)
        {
            var cmd = new Command(script, true);

            if (parameters == null) return cmd;

            foreach (var p in parameters)
                cmd.Parameters.Add(p);
            
            return cmd;
        }


        private void ThrowErrorIfNecessary(Pipeline pipeline)
        {
            if (!pipeline.HadErrors) return;

            var errorBuilder = new StringBuilder();
            var errors = pipeline.Error.ReadToEnd();
            
            foreach (var error in errors)
                errorBuilder.AppendLine(error.ToString());

            throw new InvalidOperationException(errorBuilder.ToString());
        }
    }
}