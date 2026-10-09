using MongoDB.Bson.Serialization.Attributes;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class SubCommentDto
    {
        [BsonElement("subCommentId")]
        public string SubCommentId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; }

        [BsonElement("lastUpdatedAt")]
        public string LastUpdatedAt { get; set; }
    }
}