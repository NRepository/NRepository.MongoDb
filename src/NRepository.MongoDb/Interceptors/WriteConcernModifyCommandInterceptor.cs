namespace NRepository.MongoDb.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core;
    using NRepository.Core.Command;
    using NRepository.MongoDb.Events;

    public class WriteConcernModifyCommandInterceptor : IModifyCommandInterceptor
    {
        public WriteConcernModifyCommandInterceptor(string collectionName)
            : this(new WriteConcern())
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            CollectionName = collectionName;
        }

        public WriteConcernModifyCommandInterceptor(WriteConcern writeConcern, string collectionName)
            : this(writeConcern)
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            CollectionName = collectionName;
        }

        public WriteConcernModifyCommandInterceptor(WriteConcern writeConcern)
        {
            if (writeConcern == null)
                throw new ArgumentNullException("writeConcern", "writeConcern is null.");

            WriteConcern = writeConcern;
        }

        public WriteConcernModifyCommandInterceptor()
            : this(new WriteConcern())
        {
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

        public bool CheckElementNames
        {
            get;
            set;
        }

        public void Modify<T>(ICommandRepository commandRepository, Action<T> modifyAction, T entity) where T : class
        {
            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
            if (mongoDatabase == null)
                throw new NotSupportedException("Modify can only be used with a MongoDatabase context");

            var mongoInsertOptions = new MongoInsertOptions
            {
                WriteConcern = WriteConcern,
                CheckElementNames = CheckElementNames
            };

            WriteConcernResult = mongoDatabase.GetCollection<T>(typeof(T).FullName).Save(entity, mongoInsertOptions);

            var evnt = new MongoDbEntityModifiedEvent(commandRepository, entity, WriteConcernResult);
            commandRepository.RaiseEvent(evnt);
        }
    }
}