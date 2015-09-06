namespace NRepository.MongoDb.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Events;
    using NRepository.Core.Command;
    using MongoDB.Driver;
    using NRepository.Core.Query.Specification;

    public class MongoDbEntityModifedAllEvent<TEntity> : EntityModifiedEvent where TEntity : class
    {
        public MongoDbEntityModifedAllEvent(ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, IMongoUpdate mongoUpdate, WriteConcern writeConcern)
            : base(commandRepository, null)
        {
            MongoUpdate = mongoUpdate;
            SpecificationStrategy = specificationStrategy;
            WriteConcern = writeConcern;
        }

        public MongoDbEntityModifedAllEvent(ICommandRepository commandRepository, ISpecificationQueryStrategy<TEntity> specificationStrategy, IEnumerable<MongoUpdateItem<TEntity>> mongoUpdateItems, WriteConcern writeConcern)
            : base(commandRepository, null)
        {
            MongoUpdateItems = mongoUpdateItems;
            SpecificationStrategy = specificationStrategy;
            WriteConcern = writeConcern;
        }

        public bool UsesMongoUpdateItems
        {
            get { return MongoUpdateItems != null; }
        }

        public ISpecificationQueryStrategy<TEntity> SpecificationStrategy
        {
            get;
            private set;
        }

        public WriteConcern WriteConcern
        {
            get;
            private set;
        }

        public IMongoUpdate MongoUpdate
        {
            get;
            private set;
        }

        public IEnumerable<MongoUpdateItem<TEntity>> MongoUpdateItems
        {
            get;
            private set;
        }
    }
}
