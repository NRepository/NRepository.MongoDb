namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;

    public interface IMongoDbOverridables
    {
        BsonValue GetBsonIdValueFromEntity(object entity);
    }
}