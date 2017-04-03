namespace Continuous.User.Users
{
    public interface IUserShell
    {
        void Create(Model.UserModel userModel);
        void Remove(string userName);
        Model.UserModel Get(string userName);
    }
}