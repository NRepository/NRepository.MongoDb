namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public interface IMongoEntity
    {
        [BsonId]
        ObjectId Id { get; set; }
    }
}
