namespace StorageDocRepositoryTest
{
    [TestCaseOrderer("XunitUtils.AlphabeticalOrderer", "XunitUtils")]
    public class StorageDocRepositoryTest : IClassFixture<StorageDocRepositoryFixture>
    {
        //private readonly Mock<IStorageDoc> _storageDocMock = new();
        private readonly StorageDocRepositoryFixture _repoFix;

        public StorageDocRepositoryTest(StorageDocRepositoryFixture repFixture)
        {
            _repoFix = repFixture;
        }

        [Fact]
        public async void _0CreateAsync_Success()
        {
            var ret = await _repoFix.Repository.CreateAsync(_repoFix.JsonData);
            Assert.True(ret);
        }

        [Fact]
        public async void _1CreateAsync_Faild()
        {
            var ret = await _repoFix.Repository.CreateAsync(_repoFix.WrongJsonData);
            Assert.False(ret);
        }

        [Fact]
        public async void _2RetrieveAsync_Success()
        {
            var retDoc = await _repoFix.Repository.RetrieveAsync("5");
            Assert.True(retDoc is not null && retDoc.Id == "5");
        }

        [Fact]
        public async void _3RetrieveAsync_Faild()
        {
            var retDoc = await _repoFix.Repository.RetrieveAsync("XYZ");
            Assert.True(retDoc is null);
        }

        [Fact]
        public async void _4UpdateAsync_Success()
        {
            var ret = await _repoFix.Repository.UpdateAsync("5", _repoFix.JsonData);
            Assert.True(ret);
        }

        [Fact]
        public async void _5UpdateAsync_Faid_ID()
        {
            var ret = await _repoFix.Repository.UpdateAsync("XYZ", _repoFix.JsonData);
            Assert.False(ret);
        }

        [Fact]
        public async void _6UpdateAsync_Faid_JsonData()
        {
            var ret = await _repoFix.Repository.UpdateAsync("5", _repoFix.WrongJsonData);
            Assert.False(ret);
        }

        [Fact]
        public async void _7DeleteAsync_Faid()
        {
            var ret = await _repoFix.Repository.DeleteAsync("XYZ");
            Assert.False(ret);
        }

        [Fact]
        public async void _8DeleteAsync_Success()
        {
            var ret = await _repoFix.Repository.DeleteAsync("5");
            Assert.True(ret);
        }
    }
}