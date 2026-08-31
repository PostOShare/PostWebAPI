using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class PostDto
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("partitionKey")]
        [Required(ErrorMessage = "partitionKey is required for Cosmos DB compatibility.")]
        public string PartitionKey { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("media")]
        public List<MediaDto> Media { get; set; }

        [BsonElement("createdAt")]
        public string CreatedAt { get; set; }

        [BsonElement("lastUpdatedAt")]
        public string LastUpdatedAt { get; set; }

        [BsonElement("agent")]
        public string Agent { get; set; }

        [BsonElement("visibility")]
        public string Visibility { get; set; }

        [BsonElement("stats")]
        public StatsDto Stats { get; set; }

        [BsonElement("comments")]
        public List<CommentDto> Comments { get; set; } = new();
    }
}
