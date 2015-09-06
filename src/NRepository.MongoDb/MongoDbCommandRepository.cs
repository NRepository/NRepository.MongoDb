namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NRepository.Core.Command;
    using NRepository.MongoDb.Events;

    public class MongoDbCommandRepository :
        CommandRepositoryBase
    {
        private readonly ICommandInterceptors _CommandInterceptors;

        public MongoDbCommandRepository(MongoDatabase mongoDatabase)
            : this(mongoDatabase, new DefaultCommandEventsHandlers(), new CommandInterceptors())
        {
        }

        public MongoDbCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandInterceptors commandInterceptor)
            : this(mongoDatabase, new DefaultCommandEventsHandlers(), commandInterceptor)
        {
        }

        public MongoDbCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandEventHandlers commandEvents)
            : this(mongoDatabase, commandEvents, new CommandInterceptors())
        {
        }

        public MongoDbCommandRepository(
            MongoDatabase mongoDatabase,
            ICommandEventHandlers commandEvents,
            ICommandInterceptors commandInterceptor)
            : base(commandEvents)
        {
            ObjectContext =  mongoDatabase;
            _CommandInterceptors = commandInterceptor;
        }

        public MongoDatabase MongoDatabase
        {
            get { return (MongoDatabase)ObjectContext; }
        }

        public override void Add<T>(T entity)
        {
            _CommandInterceptors.AddCommandInterceptor.Add(
                this,
                new Action<T>(p =>
                {
                    var collectionName = CollectionHelpers.CollectionNameFromEntity(entity);
                    var result = MongoDatabase.GetCollection<T>(collectionName).Insert(entity);
                    RaiseEvent(new MongoDbEntityAddedEvent(this, entity, result));
                }),
                entity);
        }

        public override void Modify<T>(T entity)
        {
            _CommandInterceptors.ModifyCommandInterceptor.Modify(
                this,
                new Action<T>(p =>
                {
                    var collectionName = CollectionHelpers.CollectionNameFromEntity(entity);
                    var result = MongoDatabase.GetCollection<T>(collectionName).Save(entity);
                    RaiseEvent(new MongoDbEntityModifiedEvent(this, entity, result));
                }),
                entity);
        }

        public override void Delete<T>(T entity)
        {
            _CommandInterceptors.DeleteCommandInterceptor.Delete(
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

        public override int Save()
        {
            throw new NotSupportedException("In this implementation all work it automatically 'committed' when performing  'updates'. To defere the database saves use the MongoDbUnitOfWorkCommandRepository instead.");
        }
    }
}