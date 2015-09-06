namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core;
    using NRepository.Core.Query;
    using NRepository.Core.Command;

    public class MongoDbUnitOfWorkRepository : RepositoryBase
        {
            public MongoDbUnitOfWorkRepository(MongoDatabase mongoDatabase)
                : this(mongoDatabase, new DefaultRepositoryEventsHandlers(), new DefaultRepositoryInterceptors())
            {
            }

            public MongoDbUnitOfWorkRepository(MongoDatabase mongoDatabase, IRepositoryEventsHandlers queryEventHandlers)
                : this(mongoDatabase, queryEventHandlers, new DefaultRepositoryInterceptors())
            {
            }

            public MongoDbUnitOfWorkRepository(MongoDatabase mongoDatabase, IRepositoryInterceptors repositoryInterceptors)
                : this(mongoDatabase, new DefaultRepositoryEventsHandlers(), repositoryInterceptors)
            {
            }

            public MongoDbUnitOfWorkRepository(
                MongoDatabase mongoDatabase,
                IRepositoryEventsHandlers eventHandlers,
                IRepositoryInterceptors repositoryInterceptors)
                : this(
                    mongoDatabase,
                    new MongoDbQueryRepository(mongoDatabase, eventHandlers, repositoryInterceptors.QueryInterceptor),
                    new MongoDbUnitOfWorkCommandRepository(mongoDatabase, eventHandlers, repositoryInterceptors))
            {
            }

            public MongoDbUnitOfWorkRepository(MongoDatabase mongoDatabase, IQueryRepository queryRepository, ICommandRepository commandRepository)
                : base(queryRepository, commandRepository)
            {
                ObjectContext = mongoDatabase;
            }

            public MongoDatabase MongoDatabase
            {
                get { return (MongoDatabase)ObjectContext; }
            }
        }
}
