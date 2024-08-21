using MessagePack;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace StorageDocServiceApiTests
{
    /// <summary>
    /// Integration tests
    /// </summary>
    [TestCaseOrderer("XunitUtils.AlphabeticalOrderer", "XunitUtils")]
    public class StorageDocServiceControllerTest : IClassFixture<StorageDocServiceFactory<StorageDocServiceAPI.Program>>
    {
        //private readonly Mock<IStorageDocRepository> _storageDocRepositoryMock = new();
        private readonly StorageDocServiceFactory<StorageDocServiceAPI.Program> _webAppFactory;

        public StorageDocServiceControllerTest(StorageDocServiceFactory<StorageDocServiceAPI.Program> webAppFactory)
        {
            _webAppFactory = webAppFactory;
        }

        [Fact]
        public async void _00PostDoc_Success()
        {
            var client = _webAppFactory.CreateClient();
            using StringContent jsonContent = new(_webAppFactory.JsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Documents/", jsonContent);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void _01PostDoc_Failed()
        {
            var client = _webAppFactory.CreateClient();
            using StringContent jsonContent = new(_webAppFactory.WrongJsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/Documents/", jsonContent);

            if (response.IsSuccessStatusCode)
                Assert.Fail();
            else
                Assert.True(response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async void _02PutDoc_Success()
        {
            var client = _webAppFactory.CreateClient();
            using StringContent jsonContent = new(_webAppFactory.JsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/Documents/5", jsonContent);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void _03PutDoc_Failed()
        {
            var client = _webAppFactory.CreateClient();
            using StringContent jsonContent = new(_webAppFactory.WrongJsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/Documents/5", jsonContent);

            if (response.IsSuccessStatusCode)
                Assert.Fail();
            else
                Assert.True(response.StatusCode == HttpStatusCode.Conflict);
        }

        [Fact]
        public async void _04GetDocJson()
        {
            var client = _webAppFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/Documents/5");

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var jsonLoc = JsonEncodedText.Encode(_webAppFactory.JsonData);
                var jsonRes = JsonEncodedText.Encode(jsonResponse);

                Assert.True(jsonRes.Equals(jsonLoc));
            }
        }

        [Fact]
        public async void _05GetDocXml()
        {
            var client = _webAppFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            var response = await client.GetAsync("/Documents/5");

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            else
            {
                var xmlResponse = await response.Content.ReadAsStringAsync();

                XDocument jsonLoc = JsonConvert.DeserializeXNode(_webAppFactory.JsonData, "Document")!;
                XDocument jsonRes = XDocument.Parse(xmlResponse);

                Assert.True(jsonLoc.ToString() == jsonRes.ToString());
            }
        }

        [Fact]
        public async void _06GetDocMsgpack()
        {
            var client = _webAppFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-msgpack"));
            var response = await client.GetAsync("/Documents/5");

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            else
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();

                var jsonRes = MessagePackSerializer.ConvertToJson(bytes);
                var jsonLoc = MessagePackSerializer.ConvertToJson(MessagePackSerializer.ConvertFromJson(_webAppFactory.JsonData));

                Assert.True(jsonRes == jsonLoc);
            }
        }

        [Fact]
        public async void _07GetDocJson_NotAcceptable()
        {
            var client = _webAppFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/XYZ"));
            var response = await client.GetAsync("/Documents/5");

            if (response.IsSuccessStatusCode)
                Assert.Fail();
            else
                Assert.True(response.StatusCode == HttpStatusCode.NotAcceptable);
        }

        [Fact]
        public async void _08GetDocJson_NotFound()
        {
            var client = _webAppFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("/Documents/XYZ");

            if (response.IsSuccessStatusCode)
                Assert.Fail();
            else
                Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async void _09DeleteDoc_Failed()
        {
            var client = _webAppFactory.CreateClient();
            var response = await client.DeleteAsync("/Documents/XYZ");

            if (response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.NotFound)
            {
                Assert.Fail();
            }
            else
            {
                //Checking if doc exist after delete
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync("/Documents/XYZ");

                Assert.False(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async void _10DeleteDoc_Success()
        {
            var client = _webAppFactory.CreateClient();
            var response = await client.DeleteAsync("/Documents/5");

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail();
            }
            else
            {
                //Checking if doc exist after delete
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync("/Documents/5");

                Assert.False(response.IsSuccessStatusCode);
            }
        }
    }
}