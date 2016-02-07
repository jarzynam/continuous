using System;

namespace Rescuer.Management
{
    public class RescuerFactory : IRescuerFactory
    {        
        public IRescuer Create(string serviceName, RescuerType rescuerType)
        {
            throw new NotImplementedException();
        }
    }
}