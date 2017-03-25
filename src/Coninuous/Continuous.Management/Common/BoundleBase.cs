using System.Reflection;
using System.Text;

namespace Continuous.Management.Common
{
    internal abstract class BoundleBase
    {
        protected string BasePath { get;  } = Assembly.GetCallingAssembly().GetName().Name;

        protected string AddToPath(string path, params string[] arguments)
        {
            var builder = new StringBuilder(path);

            foreach (var argument in arguments)
            {
                builder.Append(".");
                builder.Append(argument);
            }

            return builder.ToString();

        }
    }
}
