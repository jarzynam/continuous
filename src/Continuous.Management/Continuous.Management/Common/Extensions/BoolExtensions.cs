namespace Continuous.Management.Common.Extensions
{
    internal static class BoolExtensions
    {
        /// <summary>
        /// Convert boolean value to integer representation
        /// </summary>
        /// <param name="value">value to map</param>
        /// <returns>false => 0; true => 1</returns>
        internal static int ToInteger(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
