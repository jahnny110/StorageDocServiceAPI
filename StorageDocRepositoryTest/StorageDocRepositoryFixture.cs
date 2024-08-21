using Microsoft.Extensions.Caching.Memory;

namespace StorageDocRepositoryTest
{
    public class StorageDocRepositoryFixture
    {
        public readonly StorageDocRepository.Repositories.IStorageDocRepository Repository;

        public readonly string JsonData = """
        {
            "id": "5",
            "tags": ["important", ".net"],
            "data": {
                "some": "data",
                "optional": "fields"
                }
            }
        """;

        public readonly string WrongJsonData = """
                            {
                                "json": "Data"
                            }
                            """;

        public StorageDocRepositoryFixture() 
        {
            Repository = new StorageDocRepository.Repositories.StorageDocRepository(
                new StorageDocMemory.StorageDocMemory(), new MemoryCache(new MemoryCacheOptions()));
        }
    }
}
