using Continuous.User.Users.Model;

namespace Continuous.User.Users
{
    public interface IUserShell
    {
        void Create(UserModel userModel);
        void Remove(string userName);
        UserModel Get(string userName);
    }
}