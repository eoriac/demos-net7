namespace DemoSesion3.Services
{
    public interface IPasswordHash
    {
        string Hash(string password);

        bool Check(string hash, string password);
    }
}