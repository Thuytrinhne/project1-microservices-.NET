using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User.API.Models
{
    public class EmailVerification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("OTP")]
        public string OTP { get; set; }

        [BsonElement("ExpiryTime")]
        public DateTime ExpiryTime { get; set; }
    }
}
