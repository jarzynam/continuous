using System.Collections.Generic;

namespace Continuous.Management.WindowsServices.Resources
{
    internal interface IWin32ServiceMessages
    {
        string GetMessage(int code);
    }

    internal class Win32ServiceMessages : IWin32ServiceMessages
    {
        private readonly Dictionary<int, string> _returnMessagesByErrorCodes = new Dictionary<int, string>
        {
            {0, "The request was accepted"},
            {1, "The request is not supported"},
            {2, "The user did not have the necessary access"},
            {3, "The service cannot be stopped because other services that are running are dependent on it"},
            {4, "The requested control code is not valid, or it is unacceptable to the service"},
            {5, "The requested control code cannot be sent to the service because the state of the service (Win32_BaseService.State property) is equal to 0, 1, or 2"},
            {6, "The service has not been started"},
            {7, "The service did not respond to the start request in a timely fashion"},
            {8, "Unknown failure when starting the service"},
            {9, "The directory path to the service executable file was not found"},
            {10, "The service is already running"},
            {11, "The database to add a new service is locked"},
            {12, "A dependency this service relies on has been removed from the system"},
            {13, "The service failed to find the service needed from a dependent service"},
            {14, "The service has been disabled from the system"},
            {15, "The service does not have the correct authentication to run on the system"},
            {16, "This service is being removed from the system"},
            {17, "The service has no execution thread"},
            {18, "The service has circular dependencies when it starts"},
            {19, "A service is running under the same name"},
            {20, "The service name has invalid characters"},
            {21, "Invalid parameters have been passed to the service"},
            {22, "The account under which this service runs is either invalid or lacks the permissions to run the service"},
            {23, "The service exists in the database of services available from the system"},
            {24, "The service is currently paused in the system"}
        };

        public string GetMessage(int code)
        {
            return _returnMessagesByErrorCodes.ContainsKey(code)
                ? _returnMessagesByErrorCodes[code]
                : $"unknown code {code}";
        }
    }
}