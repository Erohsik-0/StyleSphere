namespace StyleSphere.Services
{
    public interface IVerificationStore
    {

        void StoreCode(string key , string code);
        string? GetCode(string key);
        void RemoveCode(string key);
    }
}
