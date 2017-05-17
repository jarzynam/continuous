using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo
{
    /// <summary>
    /// Fast updating windows service info properties directly from class
    /// </summary>
    public interface IWindowsServiceInfoUpdate
    {
        /// <summary>
        /// Fully qualified path to the service binary file
        /// <param name="newPath">new path</param>
        /// </summary>
        IWindowsServiceInfoUpdate Path(string newPath);

        /// <summary>
        /// Description of the service. 
        /// </summary>
        /// <param name="newDescription">new description</param>
        /// <returns></returns>
        IWindowsServiceInfoUpdate Description(string newDescription);

        /// <summary>
        /// Display name of the service (max 256 characters).
        /// </summary>
        /// <param name="newName">new display name</param>
        /// <returns></returns>
        IWindowsServiceInfoUpdate DisplayName(string newName);

        /// <summary>
        /// Account name used to run this service.
        /// </summary>
        /// <param name="newName">new account name</param>
        /// <returns></returns>
        IWindowsServiceInfoUpdate AccountName(string newName);

        /// <summary>
        /// Account domain used to run this service.
        /// </summary>
        /// <param name="newDomain">new domain</param>
        /// <returns></returns>
        IWindowsServiceInfoUpdate AccountDomain(string newDomain);

        /// <summary>
        /// Account password used to run this service. Can't rollback this property.
        /// </summary>
        /// <param name="newPassword">new account password</param>
        /// <returns></returns>
        IWindowsServiceInfoUpdate AccountPassword(string newPassword);

        /// <summary>
        /// Type of process which will be invoking this service.
        /// <param name="newType">new windows service type</param>
        /// </summary>
        IWindowsServiceInfoUpdate Type(WindowsServiceType newType);

        /// <summary>
        /// Severity of the error if the Create method fails to start.
        /// <param name="newErrorControl">new error control</param> 
        /// </summary>
        IWindowsServiceInfoUpdate ErrorControl(WindowsServiceErrorControl newErrorControl);

        /// <summary>
        /// Start mode of the Windows base service
        /// <param name="newStartMode">new start mode</param>
        /// </summary>
        IWindowsServiceInfoUpdate StartMode(WindowsServiceStartMode newStartMode);

        /// <summary>
        /// If true, the service can create or communicate with windows on the desktop. False as default
        /// <param name="newInteracWithDesktop">new interact with desktop flag</param>
        /// </summary>
        IWindowsServiceInfoUpdate InteractWithDesktop(bool newInteracWithDesktop);

        /// <summary>
        /// Rollback all properties except user password when error occur.
        /// </summary>
        /// <returns>sss</returns>
        IWindowsServiceInfoUpdate RollbackOnError();

        /// <summary>
        /// Cancel updating proccess. No changes will be made.
        /// </summary>
        /// <returns></returns>
        Model.WindowsServiceInfo Cancel();

        /// <summary>
        /// Apply updating proccess. All changes will be made. 
        /// </summary>
        /// <returns></returns>
        Model.WindowsServiceInfo Apply();
    }
}