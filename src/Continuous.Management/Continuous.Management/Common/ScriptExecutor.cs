using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Continuous.Management.Common
{
    /// <summary>
    /// Executor for embeded powershell scripts
    /// </summary>
    internal interface IScriptExecutor
    {
        /// <summary>
        /// Execute embedded script
        /// </summary>
        /// <param name="scriptFullPath">script full path in assembly</param>
        /// <param name="parameters">script parameters</param>
        /// <param name="ignoreErrorStream">throw exception when powershell not throw but  error-stream contains elements</param>
        /// <returns></returns>
        ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters, bool ignoreErrorStream = false);
    }

    internal class ScriptExecutor : IScriptExecutor
    {
        private readonly IEmbededFileReader _embededFileReader;
        
        public ScriptExecutor(Type typeForAssembly)
        {
            _embededFileReader = new EmbededFileReader(typeForAssembly);
        }

        public ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters, bool ignoreErrorStream = false)
        {
            var script = _embededFileReader.Read(scriptFullPath);

            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (var pipeline = runspace.CreatePipeline())
                {
                    var cmd = CreateCommand(parameters, script);

                    pipeline.Commands.Add(cmd);

                    var results = pipeline.Invoke();

                    if(!ignoreErrorStream)
                        ThrowFromErrorStream(pipeline);

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


        private void ThrowFromErrorStream(Pipeline pipeline)
        {
            if (!pipeline.HadErrors) return;

            var errors = pipeline.Error.ReadToEnd();

            var str = new StringBuilder();

            foreach (var error in errors)
                str.AppendLine(error.ToString());
            

            throw new InvalidOperationException(str.ToString());
        }
    }
}