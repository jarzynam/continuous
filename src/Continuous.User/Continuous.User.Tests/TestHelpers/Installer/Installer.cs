using System;
using System.Collections.Generic;
using System.Threading;

namespace Continuous.User.Tests.TestHelpers.Installer
{
    public abstract class Installer : IDisposable
    {
        private readonly List<string> _installedInstances;
        private readonly ReaderWriterLockSlim _lock;

        protected Installer()
        {
            _lock = new ReaderWriterLockSlim();
            _installedInstances = new List<string>();
        }

        protected abstract void Uninstall(string instanceName);

        protected void AddInstance(string instanceName)
        {
            _lock.EnterWriteLock();
            try
            {
                _installedInstances.Add(instanceName);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected void RemoveInstance(string instanceName)
        {
            _lock.EnterWriteLock();

            try
            {
                _installedInstances.Remove(instanceName);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            _lock.EnterWriteLock();

            try
            {
                foreach (var instance in _installedInstances)
                {
                   Uninstall(instance);
                }

                _installedInstances.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
