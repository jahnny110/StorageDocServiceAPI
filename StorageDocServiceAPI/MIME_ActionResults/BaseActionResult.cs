using Microsoft.AspNetCore.Mvc;
using System.Net;
using StorageDocRepository;

namespace StorageDocServiceAPI.MIME_ActionResults
{
    public abstract class BaseMimeActionResult : IActionResult
    {
        public const string ContentTypeJson = "application/json";
        public const string ContentTypeXml = "application/xml";
        public const string ContentTypeMsgPack = "application/x-msgpack";
        
        internal readonly StorageDocument _doc;

        public BaseMimeActionResult(StorageDocument doc)
        {
            _doc = doc;
        }

        public abstract Task ExecuteResultAsync(ActionContext context);

        internal Task NotFoundResult(HttpResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return Task.FromResult(response);
        }
    }
}
