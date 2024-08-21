using StorageDocRepository;
using StorageDocRepository.Interfaces;

namespace StorageDocTest
{
    public class StorageDocFixture
    {
        public readonly IStorageDoc StorageDoc;
        public readonly StorageDocument Document;

        public StorageDocFixture()
        {
            StorageDoc = new StorageDocMemory.StorageDocMemory();
            Document = new StorageDocument() 
            {
                Id = "5",
                JsonData = "JsonData",
                Tags = new List<string> {"AA", "BB"}
            };
        }
    }
}
