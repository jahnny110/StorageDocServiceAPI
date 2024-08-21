using log4net;
using Newtonsoft.Json;

namespace StorageDocServiceAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILog _logger;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment, ILog logger)
        {
            _next = next;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)                
                    throw;

                _logger.Error(ex);

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Message = _webHostEnvironment.IsProduction() ? "Service Unavailable" : ex.Message,
                    Exception = _webHostEnvironment.IsProduction() ? null : ex.ToString()
                };

                var body = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(body);
            }
        }
    }
}
