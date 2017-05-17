using System;
using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Shell.Extensions.WindowsServiceInfo
{
    internal class WindowsServiceInfoUpdate : IWindowsServiceInfoUpdate
    {
        private readonly IWindowsServiceShell _shell;
        private readonly Model.WindowsServiceInfo _service;
        private readonly IWindowsServiceModelManager _manager;

        private readonly ConfigurationCache _cache;
     
        internal WindowsServiceInfoUpdate(Model.WindowsServiceInfo service, IWindowsServiceShell windowsShell, IWindowsServiceModelManager manager)
        {
            _shell = windowsShell;
            _service = service;
            _manager = manager;

            _cache =  new ConfigurationCache();
        }

        public IWindowsServiceInfoUpdate Path(string newPath)
        {
            _cache.Path = newPath;

            return this;
        }

        public IWindowsServiceInfoUpdate Description(string newDescription)
        {
            _cache.Description = newDescription;

            return this;
        }

        public IWindowsServiceInfoUpdate DisplayName(string newName)
        {
            _cache.DisplayName = newName;

            return this;
        }

        public IWindowsServiceInfoUpdate AccountName(string newName)
        {
            _cache.AccountName = newName;

            return this;
        }

        public IWindowsServiceInfoUpdate AccountDomain(string newDomain)
        {
            _cache.AccountDomain = newDomain;

            return this;
        }

        public IWindowsServiceInfoUpdate AccountPassword(string newPassword)
        {
            _cache.AccountPassword = newPassword;

            return this;
        }

        public IWindowsServiceInfoUpdate Type(WindowsServiceType newType)
        {

            _cache.Type = newType;

            return this;
        }

        public IWindowsServiceInfoUpdate ErrorControl(WindowsServiceErrorControl newErrorControl)
        {
            _cache.ErrorControl = newErrorControl;

            return this;
        }

        public IWindowsServiceInfoUpdate StartMode(WindowsServiceStartMode newStartMode)
        {
            _cache.StartMode = newStartMode;

            return this;
        }

        public IWindowsServiceInfoUpdate InteractWithDesktop(bool newInteracWithDesktop)
        {
            _cache.InteractWithDesktop = newInteracWithDesktop;

            return this;
        }

        public IWindowsServiceInfoUpdate RollbackOnError()
        {
            _cache.RollbackOnError = true;
            
            return this;
        }

        public Model.WindowsServiceInfo Cancel()
        {
            _cache.Clear();

            return _service;
        }

        public Model.WindowsServiceInfo Apply()
        {
            try
            {
                Update();
            }
            catch (Exception)
            {
                if (_cache.RollbackOnError) Rollback();

                throw;
            }
            finally
            {
                _cache.Clear();
            }

            return _service.Refresh();
        }

        private void Update()
        {
            _shell.Update(_service.Name, _cache);

            if (_cache.HasUserChanged())
            {
                _shell.ChangeAccount(_cache.AccountName, _cache.AccountPassword, _cache.AccountDomain);
            }
        }

        private void Rollback()
        {
            var config = _manager.CreateBackupConfig(_service, _cache);

            _shell.Update(_service.Name, config);

            if (_cache.HasUserChanged())
            {
                _shell.ChangeAccount(_service.AccountName, null, _service.AccountDomain);
            }
        }
    }
}