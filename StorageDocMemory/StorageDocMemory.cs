using StorageDocRepository;
using StorageDocRepository.Interfaces;
using System.Reflection.Metadata;

namespace StorageDocMemory
{
    public class StorageDocMemory : IStorageDoc
    {
        private Dictionary<string, StorageDocument> _dicStorage;
        public StorageDocMemory()
        {
            _dicStorage = new Dictionary<string, StorageDocument>();
        }

        public Task<bool> DeleteDocAsync(string id)
        {
            if (_dicStorage.ContainsKey(id))
            {
                _dicStorage.Remove(id);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<StorageDocument?> GetDocAsync(string id)
        {
            _dicStorage.TryGetValue(id, out var value);
            return Task.FromResult(value);
        }

        public Task<bool> SaveDocAsync(StorageDocument doc)
        {
            if (!_dicStorage.ContainsKey(doc.Id))
            {
                _dicStorage.Add(doc.Id, doc);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> UpdateDocAsync(string id, StorageDocument doc)
        {
            if (id == doc.Id &&
                _dicStorage.ContainsKey(doc.Id))
            {
                _dicStorage[doc.Id] = doc;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
