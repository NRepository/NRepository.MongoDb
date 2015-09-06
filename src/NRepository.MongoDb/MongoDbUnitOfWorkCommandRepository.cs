namespace NRepository.MongoDb
{
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NRepository.Core.Command;
    using NRepository.MongoDb.Events;
    using System;

    public class MongoDbUnitOfWorkCommandRepository : BatchCommandRepositoryBase
    {
        private readonly ICommandInterceptors _CommandInterceptors;

        public MongoDbUnitOfWorkCommandRepository(
            MongoDatabase mongoDatabase)
            : this(mongoDatabase, new DefaultCommandEventsHandlers(), new CommandInterceptors())
        {
        }

        public MongoDbUnitOfWorkCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandInterceptors commandInterceptor)
            : this(mongoDatabase, new DefaultCommandEventsHandlers(), commandInterceptor)
        {
        }

        public MongoDbUnitOfWorkCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandEventHandlers commandEvents)
            : this(mongoDatabase, commandEvents, new CommandInterceptors())
        {
        }

        public MongoDbUnitOfWorkCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandEventHandlers commandEvents,
            ICommandInterceptors commandInterceptor)
            : base(commandEvents)
        {
            ObjectContext = MongoDatabase = mongoDatabase;
            _CommandInterceptors = commandInterceptor;
        }

        public MongoDatabase MongoDatabase
        {
            get;
            private set;
        }

        protected override void AddEntityActioned<T>(T entity, IAddCommandInterceptor addCommandInterceptor)
        {
            addCommandInterceptor.Add(
                this,
                new Action<T>(p =>
                {
                    var collectionName = CollectionHelpers.CollectionNameFromEntity(entity);
                    var result = MongoDatabase.GetCollection<T>(collectionName).Insert(entity);
                    RaiseEvent(new MongoDbEntityAddedEvent(this, entity, result));
                }),
                entity);
        }

        protected override void ModifyEntityActioned<T>(T entity, IModifyCommandInterceptor modifyCommandInterceptor)
        {
            modifyCommandInterceptor.Modify(
                this,
                new Action<T>(p =>
                {
                    var collectionName = CollectionHelpers.CollectionNameFromEntity(entity);
                    var result = MongoDatabase.GetCollection<T>(collectionName).Save(entity);
                    RaiseEvent(new MongoDbEntityModifiedEvent(this, entity, result));
                }),
                entity);
        }

        protected override void DeleteEntityActioned<T>(T entity, IDeleteCommandInterceptor deleteCommandInterceptor)
        {
            deleteCommandInterceptor.Delete(
                this,
                new Action<T>(p =>
                {
                    var bsonValue = MongoDbOverridables.Instance.GetBsonIdValueFromEntity(entity);
                    var mongoQuery = Query.EQ("_id", bsonValue);

                    var collectionName = CollectionHelpers.CollectionNameFromEntity(entity);
                    var result = MongoDatabase.GetCollection<T>(collectionName).Remove(mongoQuery, RemoveFlags.Single);
                    RaiseEvent(new MongoDbEntityDeletedEvent(this, entity, result));
                }),
                entity);
        }
    }
}