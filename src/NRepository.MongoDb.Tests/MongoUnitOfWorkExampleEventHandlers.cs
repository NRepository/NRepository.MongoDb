namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core;
    using NRepository.Core.Events;
    using NRepository.MongoDb.Events;

    public class MongoUnitOfWorkExampleEventHandlers : RepositoryEventsHandlers
    {
        public class MongoEntityAddedHandler : IRepositorySubscribe<EntityAddedEvent>
        {
            public void Handle(EntityAddedEvent repositoryEvent)
            {
                var mongoEvent = repositoryEvent as MongoDbEntityAddedEvent;

                // It's a Add to batch list
                if (mongoEvent == null)
                    return;

                // This is the add called via save
                if (mongoEvent != null)
                    return;
            }
        }

        public class MongoEntityModifiedHandler : IRepositorySubscribe<EntityModifiedEvent>
        {
            public void Handle(EntityModifiedEvent repositoryEvent)
            {
                var mongoEvent = repositoryEvent as MongoDbEntityModifiedEvent;

                // It's a modify (added to batch)
                if (mongoEvent == null)
                    return;

                // This is the modify called via save
                if (mongoEvent != null)
                    return;
            }
        }

        public class MongoEntityDeletedHandler : IRepositorySubscribe<EntityDeletedEvent>
        {
            public void Handle(EntityDeletedEvent repositoryEvent)
            {
                var mongoEvent = repositoryEvent as MongoDbEntityDeletedEvent;

                // It's a delete (added to batch)
                if (mongoEvent == null)
                    return;

                // This is the delete called via save
                if (mongoEvent != null)
                    return;
            }
        }

        public MongoUnitOfWorkExampleEventHandlers()
        {
            EntityAddedEventHandler = new MongoEntityAddedHandler();
            EntityDeletedEventHandler = new MongoEntityDeletedHandler();
            EntityModifiedEventHandler = new MongoEntityModifiedHandler();
        }
    }
}
