using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace StorageDocServiceApiTests
{
    public class StorageDocServiceFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
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

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            //builder.ConfigureServices(services =>
            //{
            //    services.

            //});
        }        
    }
}
