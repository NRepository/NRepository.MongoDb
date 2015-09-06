namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core.Command;
    using NRepository.Core.Query.Specification;

    public interface IRepositoryExtensions
    {
        WriteConcernResult AddWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class;
        WriteConcernResult DeletewithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class;
        WriteConcernResult ModifyWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class;
        WriteConcernResult UpsertWithConcern<TEntity>(ICommandRepository repository, TEntity entity, WriteConcern writeConcern = null) where TEntity : class;
        WriteConcernResult ModifyAll<TEntity>(ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, params MongoUpdateItem<TEntity>[] updateItems) where TEntity : class;
        WriteConcernResult ModifyAll<TEntity>(ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, WriteConcern writeConcern, IMongoUpdate mongoUpdate) where TEntity : class;
    }
}
