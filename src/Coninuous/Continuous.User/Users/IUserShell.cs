namespace Continuous.User.Users
{
    public interface IUserShell
    {
        void Create(Model.User user);
        void Remove(string userName);
        Model.User Get(string userName);
    }
}