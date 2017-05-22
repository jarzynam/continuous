using System;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo;

namespace Continuous.WindowsService.Shell.Extensions
{
    public interface IWindowsServiceInfoExtensions
    {
        /// <summary>
        /// Start windows service modification proccess. To make changes perform Apply() method.
        /// </summary>
        /// <returns></returns>
        IWindowsServiceInfoUpdate Change();

        /// <summary>
        /// Refresh windows service
        /// </summary>
        /// <returns></returns>
        IWindowsServiceInfoExtensions Refresh();

        IWindowsServiceInfoExtensions WaitForState(WindowsServiceState state);
        IWindowsServiceInfoExtensions Start(bool waitForState = true);
        IWindowsServiceInfoExtensions Stop(bool waitForState = true);
    }

    /// <summary>
    /// Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public class WindowsServiceInfoExtensions : IWindowsServiceInfoExtensions
    {
        private Lazy<WindowsServiceShell> _shell;
        private Lazy<IWindowsServiceModelManager> _mapper;
        private Lazy<IWindowsServiceInfoUpdate> _service;
        private Model.WindowsServiceInfo _info;

        protected WindowsServiceInfoExtensions()
        {
            
        }

        protected void InitializeBase(Model.WindowsServiceInfo windowsServiceInfo)
        {
            _info = windowsServiceInfo;
            _shell = new Lazy<WindowsServiceShell>(() => new WindowsServiceShell());
            _mapper = new Lazy<IWindowsServiceModelManager>(() => new WindowsServiceModelManager());
            _service = new Lazy<IWindowsServiceInfoUpdate>(() => new WindowsServiceInfoUpdate(windowsServiceInfo, _shell.Value, _mapper.Value));
        }

        /// <summary>
        /// Start windows service modification proccess. To make changes perform Apply() method.
        /// </summary>
        /// <returns></returns>
        public IWindowsServiceInfoUpdate Change()
        {
            return _service.Value;
        }

        /// <summary>
        /// Refresh windows service
        /// </summary>
        /// <returns></returns>
        public IWindowsServiceInfoExtensions Refresh()
        {
            var source = _shell.Value.Get(_info.Name);

            _mapper.Value.CopyProperties(_info, source);

            return _info;
        }

        public IWindowsServiceInfoExtensions WaitForState(WindowsServiceState state)
        {
            _shell.Value.WaitForState(_info.Name, state);

            return Refresh();
        }

        public IWindowsServiceInfoExtensions Start(bool waitForState = true)
        {
            _shell.Value.Start(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Running);

            return _info;
        }

        public IWindowsServiceInfoExtensions Stop(bool waitForState = true)
        {
            _shell.Value.Stop(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Stopped);

            return _info;
        }
    }
}
