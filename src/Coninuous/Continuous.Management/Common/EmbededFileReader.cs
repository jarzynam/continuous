using System.IO;
using System.Reflection;

namespace Continuous.Management.Common
{
    internal interface IEmbededFileReader
    {
        string Read(string resourceName, Assembly assembly);
    }

    internal class EmbededFileReader : IEmbededFileReader
    {
        public string Read(string resourceName, Assembly assembly)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new FileNotFoundException($"Can't find resource {resourceName} in assembly {assembly.GetName().Name}");

                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    return result;
                }
            }
        }
    }
}