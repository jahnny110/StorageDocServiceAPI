using Newtonsoft.Json;
using StorageDocRepository;

namespace StorageDocTest
{
    [TestCaseOrderer("XunitUtils.AlphabeticalOrderer", "XunitUtils")]
    public class StorageDocTest : IClassFixture<StorageDocFixture>
    {
        private readonly StorageDocFixture _storFix;

        public StorageDocTest(StorageDocFixture storFix)
        {
            _storFix = storFix;
        }

        private StorageDocument? GetCopyOfDocument(StorageDocument doc)
        {
            var serializedDoc = JsonConvert.SerializeObject(doc);
            if (serializedDoc is null)
                return null;

            return JsonConvert.DeserializeObject<StorageDocument>(serializedDoc);
        }

        [Fact]
        public async void _0SaveDocAsync_Success()
        {
            var ret = await _storFix.StorageDoc.SaveDocAsync(_storFix.Document);
            Assert.True(ret);
        }

        [Fact]
        public async void _1SaveDocAsync_Failed()
        {
            var newDocument = GetCopyOfDocument(_storFix.Document);

            var ret = await _storFix.StorageDoc.SaveDocAsync(newDocument!);
            Assert.False(ret);
        }

        [Fact]
        public async void _2UpdateDocAsync_Success()
        {
            var newDocument = GetCopyOfDocument(_storFix.Document);
            newDocument!.JsonData = "New Doc";
            var ret = await _storFix.StorageDoc.UpdateDocAsync("5", newDocument!);

            if (!ret)
                Assert.Fail();
            else
            {
                var retDoc = await _storFix.StorageDoc.GetDocAsync("5");
                Assert.True(retDoc != null && retDoc.JsonData == "New Doc");
            }
        }

        [Fact]
        public async void _3UpdateDocAsync_Failed_IdParam()
        {
            var newDocument = GetCopyOfDocument(_storFix.Document);
            var ret = await _storFix.StorageDoc.UpdateDocAsync("XYZ", newDocument!);
            Assert.False(ret);
        }

        [Fact]
        public async void _4UpdateDocAsync_Failed_IdDoc()
        {
            var newDocument = GetCopyOfDocument(_storFix.Document);
            newDocument!.Id = "XYZ";
            var ret = await _storFix.StorageDoc.UpdateDocAsync("5", newDocument!);
            Assert.False(ret);
        }

        [Fact]
        public async void _5UpdateDocAsync_Failed_IdDocAndIdParam()
        {
            var newDocument = GetCopyOfDocument(_storFix.Document);
            newDocument!.Id = "XYZ";
            var ret = await _storFix.StorageDoc.UpdateDocAsync("XYZ", newDocument!);
            Assert.False(ret);
        }

        [Fact]
        public async void _6GetDocAsync_Success()
        {
            var retDoc = await _storFix.StorageDoc.GetDocAsync("5");
            Assert.True(retDoc is not null);
        }

        [Fact]
        public async void _7GetDocAsync_Failed()
        {
            var retDoc = await _storFix.StorageDoc.GetDocAsync("XYZ");
            Assert.True(retDoc is null);
        }

        [Fact]
        public async void _8DeleteDocAsync_Failed()
        {
            var ret = await _storFix.StorageDoc.DeleteDocAsync("XYZ");
            Assert.False(ret);
        }

        [Fact]
        public async void _9DeleteDocAsync_Success()
        {
            var ret = await _storFix.StorageDoc.DeleteDocAsync("5");
            Assert.True(ret);
        }
    }
}