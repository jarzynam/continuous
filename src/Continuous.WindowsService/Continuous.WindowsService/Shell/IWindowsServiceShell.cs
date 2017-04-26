using System.Collections.Generic;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Shell
{
    /// <summary>
    /// Shell responisble for managing windows services
    /// </summary>
    public interface IWindowsServiceShell
    {
        /// <summary>
        /// Get windows service status
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns></returns>
        WindowsServiceState GetState(string serviceName);

        /// <summary>
        /// Install windows service with basic parameters
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <param name="fullServicePath">full service path to windows service executable file</param>
        void Install(string serviceName, string fullServicePath);
        
        /// <summary>
        /// Install windows service with customizable configuration 
        /// </summary>
        /// <param name="config">customizable configuration for windows service</param>
        void Install(WindowsServiceConfiguration config);

        /// <summary>
        /// Update existing windows service with customizable configuration
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <param name="config">customizable configuration for windows service.</param>
        void Update(string serviceName, WindowsServiceConfigurationForUpdate config);

        /// <summary>
        /// Uninstall windows service
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        void Uninstall(string serviceName);

        /// <summary>
        /// Stop running windows service
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns>true if service successfully stopped</returns>
        bool Stop(string serviceName);
        
        /// <summary>
        /// Start stopped windows service 
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns>true if service successfully starded</returns>
        bool Start(string serviceName);

        /// <summary>
        /// Change windows service running user account
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <param name="accountName">account name</param>
        /// <param name="password">account password</param>
        /// <param name="domain">account domain - leave '.' to use local domain</param>
        void ChangeAccount(string serviceName, string accountName, string password, string domain = ".");

        /// <summary>
        /// Get windows service
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns>full information about windows service</returns>
        WindowsServiceInfo Get(string serviceName);

        /// <summary>
        /// Get all available windows services
        /// </summary>
        /// <returns>list of services</returns>
        List<WindowsServiceInfo> GetAll();

        /// <summary>
        /// Resume paused service
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns>true if service successfully resumed</returns>
        bool Continue(string serviceName);

        /// <summary>
        /// Pause running service
        /// </summary>
        /// <param name="serviceName">windows service name</param>
        /// <returns>true if service successfully paused</returns>
        bool Pause(string serviceName);
    }
}