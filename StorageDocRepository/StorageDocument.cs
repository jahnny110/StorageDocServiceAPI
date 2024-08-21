using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.ComponentModel.DataAnnotations;

namespace StorageDocRepository
{
    //[DataContract]
    public class StorageDocument
    {
        [JsonProperty("id", Required = Required.Always)]
        [MinLength(1)]
        public required string Id { get; set; }

        [JsonProperty("tags", Required = Required.AllowNull)]
        public required List<string> Tags { get; set; }

        [JsonIgnore]
        public required string JsonData { get; set; }


        private static readonly string _schemaJson = """
              {
              "type": "object",
              "properties": {
                "id": {
                  "type": "string",
                  "minLength": 1
                },
                "tags": {
                  "type": [
                    "array",
                    "null"
                  ],
                  "items": {
                    "type": [
                      "string"
                    ]
                  }
                },
                "data": {}
              },
              "required": [
                "id",
                "tags",
                "data"
              ]
            }
            """;
        public static bool IsValid(string jsonData)
        {
            JSchema schema = JSchema.Parse(_schemaJson);
            JObject user = JObject.Parse(jsonData);
            return user.IsValid(schema);
        }
    }
}
