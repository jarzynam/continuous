namespace Continuous.Management.LocalUser
{
    public interface ILocalUserShell
    {
        void Create(Model.LocalUser user);
        void Remove(string userName);
        Model.LocalUser Get(string userName);
    }
}