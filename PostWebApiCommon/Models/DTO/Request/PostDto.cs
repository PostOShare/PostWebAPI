using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class PostDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("partitionKey")]
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
