using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StorageDocRepository;
using System.Xml.Linq;

namespace StorageDocServiceAPI.MIME_ActionResults
{
    public class XmlActionResult : BaseMimeActionResult
    {
        public XmlActionResult(StorageDocument doc) : base(doc)
        { }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (_doc == null || string.IsNullOrWhiteSpace(_doc.JsonData))
                return NotFoundResult(response);

            XDocument convertedXml = JsonConvert.DeserializeXNode(_doc.JsonData, "Document", writeArrayAttribute: true)!;
            if (convertedXml is null)
                return NotFoundResult(response);
                        
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = ContentTypeXml;
            response.Headers.ContentEncoding = "UTF-8";

            var xmlRet = convertedXml.ToString();
            return response.WriteAsync(xmlRet);
        }
    }
}
