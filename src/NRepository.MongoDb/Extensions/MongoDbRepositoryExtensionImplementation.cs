namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NRepository.Core.Command;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NRepository.MongoDb.Events;
    using NRepository.MongoDb.Interceptors;

    public class MongoDbRepositoryExtensionImplementation : IRepositoryExtensions
    {
        public WriteConcernResult AddWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var interceptor = new WriteConcernAddCommandInterceptor(writeConcern ?? new WriteConcern());
            repository.Add(entity, interceptor);
            return interceptor.WriteConcernResult;
        }

        public WriteConcernResult DeletewithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var interceptor = new WriteConcernDeleteCommandInterceptor(writeConcern ?? new WriteConcern());
            repository.Delete(entity, interceptor);
            return interceptor.WriteConcernResult;
        }

        public WriteConcernResult ModifyWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var interceptor = new WriteConcernModifyCommandInterceptor(writeConcern ?? new WriteConcern());
            repository.Modify(entity, interceptor);
            return interceptor.WriteConcernResult;
        }

        public WriteConcernResult UpsertWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "repository is null.");

            var interceptor = new WriteConcernAddCommandInterceptor(writeConcern ?? new WriteConcern());
            repository.Add(entity, interceptor);
            return interceptor.WriteConcernResult;
        }

        public WriteConcernResult ModifyAll<TEntity>(
            ICommandRepository commandRepository,
            ISpecificationQueryStrategy<TEntity> specificationStrategy,
            WriteConcern writeConcern,
            params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class
        {
            if (writeConcern == null)
                throw new ArgumentNullException("writeConcern", "writeConcern is null.");

            if (specificationStrategy == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            if (updateItems == null)
                throw new ArgumentNullException("mongoUpdate", "mongoUpdate is null.");

            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
            if (mongoDatabase == null)
                throw new NotSupportedException("Load can only be used with a DbContext");

            var updateBuilder = new UpdateBuilder();
            foreach (var item in updateItems)
            {
                var value = BsonTypeMapper.MapToBsonValue(item.Value);
                updateBuilder.Set(item.Key, value);
            }

            var collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).FullName);
            var query = collection.AsQueryable().AddQueryStrategy(specificationStrategy);
            var mongoQuery = ((MongoQueryable<TEntity>)query).GetMongoQuery();

            var result = collection.Update(mongoQuery, updateBuilder, UpdateFlags.Multi, writeConcern);

            var modifyAllEvent = new MongoDbEntityModifedAllEvent<TEntity>(commandRepository, specificationStrategy, updateBuilder, writeConcern);
            commandRepository.RaiseEvent(modifyAllEvent);

            return result;

        }

        public WriteConcernResult ModifyAll<TEntity>(
            ICommandRepository commandRepository,
            ISpecificationQueryStrategy<TEntity> specificationStrategy,
            WriteConcern writeConcern,
            IMongoUpdate mongoUpdate) where TEntity : class
        {
            if (writeConcern == null)
                throw new ArgumentNullException("writeConcern", "writeConcern is null.");

            if (specificationStrategy == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            if (mongoUpdate == null)
                throw new ArgumentNullException("mongoUpdate", "mongoUpdate is null.");

            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
            if (mongoDatabase == null)
                throw new NotSupportedException("ModifyAll can only be used with a MongoDatabase context");

            var collection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).FullName);
            var query = collection.AsQueryable().AddQueryStrategy(specificationStrategy);
            var mongoQuery = ((MongoQueryable<TEntity>)query).GetMongoQuery();

            // Call Mongo
            var result = collection.Update(mongoQuery, mongoUpdate, UpdateFlags.Multi, writeConcern);

            var modifyAllEvent = new MongoDbEntityModifedAllEvent<TEntity>(commandRepository, specificationStrategy, mongoUpdate, writeConcern);
            commandRepository.RaiseEvent(modifyAllEvent);

            return result;
        }

    //    public WriteConcernResult UpdateProperties( ICommandRepository commandRepository,
    }
}
