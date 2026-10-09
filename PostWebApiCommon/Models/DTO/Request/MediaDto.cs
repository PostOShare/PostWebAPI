using MongoDB.Bson.Serialization.Attributes;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class MediaDto
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; }

        [BsonElement("visibility")]
        public string Visibility { get; set; }
    }
}