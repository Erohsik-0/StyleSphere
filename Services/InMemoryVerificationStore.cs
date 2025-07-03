namespace StyleSphere.Services
{
    public class InMemoryVerificationStore : IVerificationStore
    {

        private readonly Dictionary<string, string> _store = new();

        public void StoreCode(string key, string code) => _store[key] = code;

        public string? GetCode(string key) => _store.TryGetValue(key, out var code) ? code : null;

        public void RemoveCode(string key)
        {
            if (_store.ContainsKey(key))
            {
                _store.Remove(key);
            }
        }

    }
}
