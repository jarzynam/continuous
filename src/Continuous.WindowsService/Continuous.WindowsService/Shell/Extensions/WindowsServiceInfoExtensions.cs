using System;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Resources;
using Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo;

namespace Continuous.WindowsService.Shell.Extensions
{
    /// <summary>
    ///     Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public interface IWindowsServiceInfoExtensions
    {
        /// <summary>
        ///     Start windows service modification proccess. To make changes perform Apply() method.
        /// </summary>
        /// <returns></returns>
        IWindowsServiceInfoUpdate Change();

        /// <summary>
        ///     Refresh all data
        /// </summary>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Refresh();

        /// <summary>
        ///     Wait until service get expected state. TimeoutException is possible.
        /// </summary>
        /// <param name="state">expected state</param>
        IWindowsServiceInfoExtensions WaitForState(WindowsServiceState state);

        /// <summary>
        ///     Start stopped windows service
        /// </summary>
        /// <param name="waitForState">wait until service start running</param>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Start(bool waitForState = true);

        /// <summary>
        ///     Stop running service
        /// </summary>
        /// <param name="waitForState">wait until service stopped</param>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Stop(bool waitForState = true);

        /// <summary>
        ///     Pause running service
        /// </summary>
        /// <param name="waitForState">wait until service paused</param>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Pause(bool waitForState = true);

        /// <summary>
        ///     Resume paused service
        /// </summary>
        /// <param name="waitForState">wait until service start running</param>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Continue(bool waitForState = true);

        /// <summary>
        ///     Execute a custom command on the service. The value must be between 128 and 256, inclusive.
        /// </summary>
        /// <param name="commandCode">code which will be send to service</param>
        /// <returns></returns>
        IWindowsServiceInfoExtensions ExecuteCommand(int commandCode);

        /// <summary>
        ///     Check if service has not been removed
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        ///     Uninstall this service
        /// </summary>
        void Uninstall();
    }

    /// <summary>
    ///     Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public class WindowsServiceInfoExtensions : IWindowsServiceInfoExtensions
    {
        private Model.WindowsServiceInfo _info;
        private Lazy<IWindowsServiceModelManager> _mapper;
        private Lazy<IWindowsServiceInfoUpdate> _service;
        private Lazy<WindowsServiceShell> _shell;
        private Lazy<IWindowsServiceExceptionFactory> _exceptionFactory;

        protected WindowsServiceInfoExtensions()
        {
        }

        /// <inheritdoc />
        public IWindowsServiceInfoUpdate Change()
        {
            return _service.Value;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions Refresh()
        {
            var source = _shell.Value.Get(_info.Name);

            if(source == null)
                _exceptionFactory.Value.ServiceNotFoundException(_info.Name);

            _mapper.Value.CopyProperties(_info, source);

            return _info;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions WaitForState(WindowsServiceState state)
        {
            _shell.Value.WaitForState(_info.Name, state);

            return Refresh();
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions Start(bool waitForState = true)
        {
            _shell.Value.Start(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Running);

            return _info;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions Stop(bool waitForState = true)
        {
            _shell.Value.Stop(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Stopped);

            return _info;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions Pause(bool waitForState = true)
        {
            _shell.Value.Pause(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Paused);

            return _info;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions Continue(bool waitForState = true)
        {
            _shell.Value.Continue(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Running);

            return _info;
        }

        /// <inheritdoc />
        public IWindowsServiceInfoExtensions ExecuteCommand(int commandCode)
        {
            _shell.Value.ExecuteCommand(_info.Name, commandCode);

            return this;
        }

        /// <inheritdoc />
        public bool Exists()
        {
            return _shell.Value.Exists(_info.Name);
        }

        /// <inheritdoc />
        public void Uninstall()
        {
            _shell.Value.Uninstall(_info.Name);
        }

        protected void InitializeBase(Model.WindowsServiceInfo windowsServiceInfo)
        {
            _info = windowsServiceInfo;
            _shell = new Lazy<WindowsServiceShell>(() => new WindowsServiceShell());
            _mapper = new Lazy<IWindowsServiceModelManager>(() => new WindowsServiceModelManager());
            _service = new Lazy<IWindowsServiceInfoUpdate>(
                () => new WindowsServiceInfoUpdate(windowsServiceInfo, _shell.Value, _mapper.Value));

            _exceptionFactory =
                new Lazy<IWindowsServiceExceptionFactory>(
                    () => new WindowsServiceExceptionFactory(new Win32ServiceMessages()));
        }
    }
}