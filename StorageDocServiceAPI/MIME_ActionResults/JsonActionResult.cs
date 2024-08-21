using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageDocRepository;

namespace StorageDocServiceAPI.MIME_ActionResults
{
    public class JsonActionResult : BaseMimeActionResult
    {
        public JsonActionResult(StorageDocument doc) : base(doc)
        { }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (_doc == null || string.IsNullOrWhiteSpace(_doc.JsonData))
                return NotFoundResult(response);

            response.ContentType = ContentTypeJson;
            response.StatusCode = StatusCodes.Status200OK;
            response.Headers.ContentEncoding = "UTF-8";
            return response.WriteAsync(_doc.JsonData);
        }
    }
}
