using MongoDB.Bson.Serialization.Attributes;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class CommentDto
    {
        [BsonElement("commentId")]
        public string CommentId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; }

        [BsonElement("lastUpdatedAt")]
        public string LastUpdatedAt { get; set; }

        [BsonElement("subComments")]
        public List<SubCommentDto> SubComments { get; set; } = new();
    }
}