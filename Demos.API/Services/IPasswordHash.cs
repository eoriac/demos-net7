namespace Demo.API.Services
{
    public interface IPasswordHash
    {
        string Hash(string password);

        bool Check(string hash, string password);
    }
}