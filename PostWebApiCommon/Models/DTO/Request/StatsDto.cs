using MongoDB.Bson.Serialization.Attributes;

namespace PostWebApiCommon.Models.DTO.Request
{
    public class StatsDto
    {
        [BsonElement("thumbsUpReactions")]
        public int ThumbsUpReactions { get; set; }

        [BsonElement("hahaReactions")]
        public int HahaReactions { get; set; }

        [BsonElement("angryReactions")]
        public int AngryReactions { get; set; }

        [BsonElement("loveReactions")]
        public int LoveReactions { get; set; }

        [BsonElement("comments")]
        public int Comments { get; set; }

        [BsonElement("share")]
        public int Share { get; set; }
    }
}