using MessagePack;
using Microsoft.AspNetCore.Mvc;
using StorageDocRepository;

namespace StorageDocServiceAPI.MIME_ActionResults
{
    public class MessagePackActionResult : BaseMimeActionResult
    {
        public MessagePackActionResult(StorageDocument doc)
            : base(doc)
        { }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (_doc == null || string.IsNullOrWhiteSpace(_doc.JsonData))
                return NotFoundResult(response);


            var mp = MessagePackSerializer.ConvertFromJson(_doc.JsonData);
            if (mp is null || mp.Length == 0)
                return NotFoundResult(response);

            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = ContentTypeMsgPack;
            return response.Body.WriteAsync(mp, 0, mp.Length);
        }
    }
}
