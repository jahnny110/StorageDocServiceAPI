using Microsoft.Extensions.Caching.Memory;
using StorageDocRepository.Interfaces;
using StorageDocRepository.Repositories;
using StorageDocServiceAPI.Middlewares;

namespace StorageDocServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            builder.Services.AddSingleton<IStorageDoc, StorageDocMemory.StorageDocMemory>();
            builder.Services.AddSingleton<IStorageDocRepository, StorageDocRepository.Repositories.StorageDocRepository>();

            builder.Services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            });

            builder.Services.AddLog4net();

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
