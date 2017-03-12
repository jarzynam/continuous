using Continuous.Management.Users.Model;

namespace Continuous.Management.Users
{
    public interface IUserShell
    {
        void Create(User user);
        void Remove(string userName);
        User Get(string userName);
    }
}