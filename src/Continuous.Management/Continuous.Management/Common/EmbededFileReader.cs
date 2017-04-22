using System;
using System.IO;
using System.Reflection;

namespace Continuous.Management.Common
{
    internal interface IEmbededFileReader
    {
        string Read(string resourceName);
    }

    internal class EmbededFileReader : IEmbededFileReader
    {
        private readonly Assembly _assembly;

        public EmbededFileReader(Type type)
        {
            _assembly = Assembly.GetAssembly(type);
        }

        public string Read(string resourceName)
        {
            using (var stream = _assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new FileNotFoundException($"Can't find resource {resourceName} in assembly {_assembly.GetName().Name}");

                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    return result;
                }
            }
        }
    }
}