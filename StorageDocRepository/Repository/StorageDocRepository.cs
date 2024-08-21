using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StorageDocRepository.Interfaces;

namespace StorageDocRepository.Repositories
{
    public class StorageDocRepository : IStorageDocRepository
    {
        private IStorageDoc _storageDoc;

        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        public StorageDocRepository(IStorageDoc storageDoc, IMemoryCache memoryCache)
        {
            _storageDoc = storageDoc;
            _memoryCache = memoryCache;
        }

        private StorageDocument? DeserializeJsonToStorageDoc(string jsonDocStr)
        {
            if (StorageDocument.IsValid(jsonDocStr))
            {
                var doc = JsonConvert.DeserializeObject<StorageDocument>(jsonDocStr);
                if (doc == null)
                    return null;

                doc.JsonData = jsonDocStr;
                doc.Id = doc.Id.Trim().ToLower();

                return doc;
            }
            return null;
        }

        public async Task<bool> CreateAsync(string jsonDocStr)
        {
            var doc = DeserializeJsonToStorageDoc(jsonDocStr);
            if (doc == null)
                return false;

            var ret = await _storageDoc.SaveDocAsync(doc);
            if (ret)
                _memoryCache.Set(doc.Id, doc, _cacheEntryOptions);

            return ret;
        }

        public async Task<bool> UpdateAsync(string id, string jsonDocStr)
        {
            var doc = DeserializeJsonToStorageDoc(jsonDocStr);
            if (doc == null)
                return false;

            if (string.IsNullOrWhiteSpace(id))
                return false;
            id = id.Trim().ToLower();

            var ret = await _storageDoc.UpdateDocAsync(id, doc);
            if (ret)
                _memoryCache.Set(doc.Id, doc, _cacheEntryOptions);

            return ret;
        }

        public async Task<StorageDocument?> RetrieveAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            id = id.Trim().ToLower();

            if (_memoryCache.TryGetValue(id, out StorageDocument? fromCache))
                return fromCache;

            return await _storageDoc.GetDocAsync(id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var ret = await _storageDoc.DeleteDocAsync(id);
            if (ret) //&& _memoryCache.Rem)
                _memoryCache.Remove(id);

            return ret;
        }
    }
}
