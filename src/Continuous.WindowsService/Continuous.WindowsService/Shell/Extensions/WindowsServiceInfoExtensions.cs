using System;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo;

namespace Continuous.WindowsService.Shell.Extensions
{

    /// <summary>
    /// Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public class WindowsServiceInfoExtensions 
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
        public IWindowsServiceInfoUpdate Update()
        {
            return _service.Value;
        }

        /// <summary>
        /// Refresh windows service
        /// </summary>
        /// <returns></returns>
        public Model.WindowsServiceInfo Refresh()
        {
            var source = _shell.Value.Get(_info.Name);

            _mapper.Value.CopyProperties(_info, source);

            return _info;
        }

        public Model.WindowsServiceInfo WaitForState(WindowsServiceState state)
        {
            _shell.Value.WaitForState(_info.Name, state);

            return Refresh();
        }

        public Model.WindowsServiceInfo Start(bool waitForState = true)
        {
            _shell.Value.Start(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Running);

            return _info;
        }

        public Model.WindowsServiceInfo Stop(bool waitForState = true)
        {
            _shell.Value.Stop(_info.Name);

            if (waitForState)
                WaitForState(WindowsServiceState.Stopped);

            return _info;
        }
    }
}
