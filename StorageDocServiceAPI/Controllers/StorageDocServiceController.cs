using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StorageDocRepository;
using StorageDocRepository.Repositories;
using StorageDocServiceAPI.MIME_ActionResults;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StorageDocServiceAPI.Controllers
{
    [Route("Documents")]
    [ApiController]
    public class StorageDocServiceController : ControllerBase
    {
        private readonly IStorageDocRepository _storageDocRep;

        public StorageDocServiceController(IStorageDocRepository storageDocRep)
        {
            _storageDocRep = storageDocRep;
        }

        // GET Document/<StorageDocServiceController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Consumes(BaseMimeActionResult.ContentTypeJson, BaseMimeActionResult.ContentTypeXml, BaseMimeActionResult.ContentTypeMsgPack)]
        public async Task<IActionResult> Get(string id)
        {
            var doc = await _storageDocRep.RetrieveAsync(id);
            return doc is null ? NotFound() : ResolveMimeTypeResult(doc);
        }

        private IActionResult ResolveMimeTypeResult(StorageDocument doc)
        {
            var acceptValues = Request.Headers.Accept;

            if (StringValues.IsNullOrEmpty(acceptValues))
                return NotFound();

            var acceptStr = acceptValues.ToString();

            if (acceptStr.Contains(BaseMimeActionResult.ContentTypeJson))
                return new JsonActionResult(doc);
            if (acceptStr.Contains(BaseMimeActionResult.ContentTypeXml))
                return new XmlActionResult(doc);
            if (acceptStr.Contains(BaseMimeActionResult.ContentTypeMsgPack))
                return new MessagePackActionResult(doc);

            return NotFound();
        }

        // POST Document/<StorageDocServiceController>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Post([FromBody] JsonElement jsonDoc)
        {
            var jsonStr = jsonDoc.ToString();
            var retCode = await _storageDocRep.CreateAsync(jsonStr);
            return retCode ? NoContent() : Conflict("Wrong document");
        }

        // PUT Document/<StorageDocServiceController>/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(string id, [FromBody] JsonElement jsonDoc)
        {
            var jsonStr = jsonDoc.ToString();
            var retCode = await _storageDocRep.UpdateAsync(id, jsonStr);
            return retCode ? NoContent() : Conflict("Wrong document or its ID");
        }

        //DELETE Document/<StorageDocServiceController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var doc = await _storageDocRep.RetrieveAsync(id);
            if (doc is null)
                return NotFound();

            var ret = await _storageDocRep.DeleteAsync(id);
            if (ret)
                return NoContent();

            return BadRequest("Document was found but failed to delete.");
        }
    }
}
