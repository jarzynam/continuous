namespace Continuous.Management.LocalUser
{
    public interface ILocalUserShell
    {
        void CreateUser(Model.LocalUser user);
        void RemoveUser(string userName);
        Model.LocalUser GetUser(string userName);
    }
}