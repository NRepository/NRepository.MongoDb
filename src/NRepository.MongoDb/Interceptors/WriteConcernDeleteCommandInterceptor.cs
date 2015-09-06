namespace NRepository.MongoDb.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core;
    using NRepository.Core.Command;
    using NRepository.MongoDb.Events;
    using MongoDB.Driver.Builders;

    public class WriteConcernDeleteCommandInterceptor : IDeleteCommandInterceptor
    {
        public WriteConcernDeleteCommandInterceptor(string collectionName)
            : this(new WriteConcern())
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            CollectionName = collectionName;
        }

        public WriteConcernDeleteCommandInterceptor(WriteConcern writeConcern, string collectionName)
            : this(writeConcern)
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            CollectionName = collectionName;
        }

        public WriteConcernDeleteCommandInterceptor(WriteConcern writeConcern)
        {
            if (writeConcern == null)
                throw new ArgumentNullException("writeConcern", "writeConcern is null.");

            WriteConcern = writeConcern;
        }

        public WriteConcernDeleteCommandInterceptor()
            : this(new WriteConcern())
        {
        }

        public RemoveFlags RemoveFlags
        {
            get;
            set;
        }

        public string CollectionName
        {
            get;
            private set;
        }

        public WriteConcern WriteConcern
        {
            get;
            private set;
        }

        public WriteConcernResult WriteConcernResult
        {
            get;
            private set;
        }

        public void Delete<T>(ICommandRepository commandRepository, Action<T> modifyAction, T entity) where T : class
        {
            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
            if (mongoDatabase == null)
                throw new NotSupportedException("Delete can only be used with a MongoDatabase context");

            var collectionName = CollectionName ?? typeof(T).FullName;

            var bsonValue = MongoDbOverridables.Instance.GetBsonIdValueFromEntity(entity);
            var mongoQuery = Query.EQ("_id", bsonValue);
            WriteConcernResult = mongoDatabase.GetCollection<T>(typeof(T).FullName).Remove(mongoQuery, RemoveFlags, WriteConcern);

            var evnt = new MongoDbEntityDeletedEvent(commandRepository, entity, WriteConcernResult);
            commandRepository.RaiseEvent(evnt);
        }

    }
}
