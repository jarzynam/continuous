using System;
using Continuous.User.Users.Extensions.LocalUserInfo;

namespace Continuous.User.Users.Extensions
{
    /// <summary>
    ///     Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public interface ILocalUserInfoExtensions
    {
        /// <summary>
        ///     Start local user modification proccess. To make changes perform Apply() method.
        /// </summary>
        /// <returns></returns>
        ILocalUserInfoUpdate Change();

        /// <summary>
        ///     Refresh all data
        /// </summary>
        /// <returns></returns>
        ILocalUserInfoExtensions Refresh();

        /// <summary>
        ///     Check if user has not been removed
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        ///     Remove this user
        /// </summary>
        void Remove();
    }

    /// <summary>
    ///     Extended WindowsServiceInfo with managing services functionality
    /// </summary>
    public class LocalUserInfoExtensions : ILocalUserInfoExtensions
    {
        private Model.LocalUserInfo _info;
        private Lazy<ILocalUserInfoUpdate> _service;
        private Lazy<IUserShell> _shell;
        private Lazy<IUserExceptionFactory> _exceptionFactory;
        private Lazy<UserMapper> _mapper;

        protected LocalUserInfoExtensions()
        {
        }

        /// <inheritdoc />
        public ILocalUserInfoUpdate Change()
        {
            return _service.Value;
        }

        /// <inheritdoc />
        public ILocalUserInfoExtensions Refresh()
        {
            var source = _shell.Value.GetLocalUser(_info.Name);

            if(source == null)
                _exceptionFactory.Value.UserNotFoundException(_info.Name);

            _mapper.Value.CopyProperties(_info, source);

            return _info;
        }

        /// <inheritdoc />
        public bool Exists()
        {
            return _shell.Value.Exists(_info.Name);
        }

        /// <inheritdoc />
        public void Remove()
        {
            _shell.Value.Remove(_info.Name);
        }

        protected void InitializeBase(Model.LocalUserInfo localUserInfo)
        {
            _info = localUserInfo;
            _shell = new Lazy<IUserShell>(() => new UserShell());
            _mapper = new Lazy<UserMapper>(() => new UserMapper());
            _service = new Lazy<ILocalUserInfoUpdate>(
                () => new LocalUserInfoUpdate(localUserInfo, _shell.Value));

            _exceptionFactory =
                new Lazy<IUserExceptionFactory>(
                    () => new WindowsServiceExceptionFactory());
        }
    }
}