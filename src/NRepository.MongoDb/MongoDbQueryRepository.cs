namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using NRepository.Core.Query;

    public class MongoDbQueryRepository : QueryRepositoryBase
    {
        public MongoDbQueryRepository(MongoDatabase mongoDatabase)
            : this(mongoDatabase, new DefaultQueryEventHandlers(), new DefaultQueryInterceptor())
        {
        }

        public MongoDbQueryRepository(MongoDatabase mongoDatabase, IQueryEventHandler queryEventHandlers)
            : this(mongoDatabase, queryEventHandlers, new DefaultQueryInterceptor())
        {
        }

        public MongoDbQueryRepository(MongoDatabase mongoDatabase, IQueryInterceptor queryInterceptor)
            : this(mongoDatabase, new DefaultQueryEventHandlers(), queryInterceptor)
        {
        }

        public MongoDbQueryRepository(MongoDatabase mongoDatabase, IQueryEventHandler queryEventHandlers, IQueryInterceptor queryInterceptor)
            : base(queryEventHandlers, queryInterceptor)
        {
            if (mongoDatabase == null)
                throw new ArgumentNullException("MongoDatabase", "MongoDatabase is null.");

            ObjectContext = mongoDatabase;
        }

        public override IQueryable<T> GetQueryableEntities<T>(object additionalQueryData)
        {
            var collectionName = CollectionHelpers.CollectionNameFromType<T>();
            var set = ((MongoDatabase)ObjectContext).GetCollection<T>(
                additionalQueryData == null ?
                collectionName :
                additionalQueryData.ToString()).AsQueryable<T>();

            var retVal = QueryInterceptor.Query<T>(this, set, additionalQueryData);
            return retVal;
        }
    }
}
