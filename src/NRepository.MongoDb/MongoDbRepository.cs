namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core;
    using NRepository.Core.Command;
    using NRepository.Core.Query;

    public class MongoDbRepository : RepositoryBase
    {
        public MongoDbRepository(MongoDatabase mongoDatabase)
            : this(mongoDatabase, new DefaultRepositoryEventsHandlers(), new DefaultRepositoryInterceptors())
        {
        }

        public MongoDbRepository(MongoDatabase mongoDatabase, IRepositoryEventsHandlers queryEventHandlers)
            : this(mongoDatabase, queryEventHandlers, new DefaultRepositoryInterceptors())
        {

        }

        public MongoDbRepository(MongoDatabase mongoDatabase, IRepositoryInterceptors repositoryInterceptors)
            : this(mongoDatabase, new DefaultRepositoryEventsHandlers(), repositoryInterceptors)
        {
        }

        public MongoDbRepository(
            MongoDatabase mongoDatabase,
            IRepositoryEventsHandlers eventHandlers,
            IRepositoryInterceptors repositoryInterceptors)
            : this(
                mongoDatabase,
                new MongoDbQueryRepository(mongoDatabase, eventHandlers, repositoryInterceptors.QueryInterceptor),
                new MongoDbCommandRepository(mongoDatabase, eventHandlers, repositoryInterceptors))
        {
        }

        public MongoDbRepository(MongoDatabase mongoDatabase, IQueryRepository queryRepository, ICommandRepository commandRepository)
            : base(queryRepository, commandRepository)
        {
            ObjectContext = mongoDatabase;
        }

        internal new MongoDbQueryRepository QueryRepository
        {
            get { return (MongoDbQueryRepository)base.QueryRepository; }
        }

        public MongoDatabase MongoDatabase
        {
            get { return (MongoDatabase)ObjectContext; }
        }
    }
}
