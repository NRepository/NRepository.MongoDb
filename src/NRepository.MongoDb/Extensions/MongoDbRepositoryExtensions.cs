namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core;
    using NRepository.Core.Command;
    using NRepository.Core.Query.Specification;

    public static class MongoDbRepositoryExtensions
    {
        private static IRepositoryExtensions _DefaultImplementation = new MongoDbRepositoryExtensionImplementation();

        public static void SetDefaultImplementation(IRepositoryExtensions defaultImplementation)
        {
            _DefaultImplementation = defaultImplementation;
        }

        public static WriteConcernResult AddWithConcern<TEntity>(this ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.AddWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult DeletewithConcern<TEntity>(this ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.DeletewithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult ModifyWithConcern<TEntity>(this ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.ModifyWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult UpsertWithConcern<TEntity>(this ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.UpsertWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, writeConcern, updateItems);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, new WriteConcern(), updateItems);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, IMongoUpdate mongoUpdate) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, writeConcern, mongoUpdate);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, IMongoUpdate mongoUpdate) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, new WriteConcern(), mongoUpdate);
        }

        public static WriteConcernResult AddWithConcern<TEntity>(this IRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.AddWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult DeletewithConcern<TEntity>(this IRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.DeletewithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult ModifyWithConcern<TEntity>(this IRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.ModifyWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult UpsertWithConcern<TEntity>(this IRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class
        {
            return _DefaultImplementation.UpsertWithConcern(repository, entity, writeConcern);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this IRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, writeConcern, updateItems);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this IRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, new WriteConcern(), updateItems);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this IRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, IMongoUpdate mongoUpdate) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, writeConcern, mongoUpdate);
        }

        public static WriteConcernResult ModifyAll<TEntity>(this IRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, IMongoUpdate mongoUpdate) where TEntity : class
        {
            return _DefaultImplementation.ModifyAll<TEntity>(commandRepository, specificationStrategy, new WriteConcern(), mongoUpdate);
        }
    }
}