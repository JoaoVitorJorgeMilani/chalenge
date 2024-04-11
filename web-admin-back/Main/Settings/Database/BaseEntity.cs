using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Main.Settings.Database
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = ObjectId.GenerateNewId();
            CreateDate = DateTime.Now;
            UpdateDate = CreateDate;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public DateTime CreateDate { get; protected set; }

        public DateTime UpdateDate { get; internal set; }

        public void OnUpdate()
        {
            UpdateDate = DateTime.Now;
        }
    }
}
