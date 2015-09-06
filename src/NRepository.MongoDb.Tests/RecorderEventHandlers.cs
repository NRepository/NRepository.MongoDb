namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core.Query.Specification;
    using NUnit.Framework;
    using NRepository.Core.Query;
    using NRepository.Core.Command;
    using NRepository.Core.Events;
using NRepository.Core;

    //public class RecorderEventHandlers : CommandEventsHandlers
    //{
    //    public RecorderEventHandlers()
    //    {
    //        EntityAddedEventHandler = new RecorderEventEntityAdded();
    //        EntityDeletedEventHandler = new RecorderEventEntityDeleted();
    //        EntityModifiedEventHandler = new RecorderEventEntityModified();
    //        RepositorySavedEventHandler = new RecorderEventSaved();
    //    }

    //    public List<EntityAddedEvent> AddedEvents
    //    {
    //        get { return ((RecorderEventEntityAdded)EntityAddedEventHandler).AddedEvents; }
    //    }

    //    public List<EntityDeletedEvent> DeletedEvents
    //    {
    //        get { return ((RecorderEventEntityDeleted)EntityDeletedEventHandler).DeletedEvents; }
    //    }

    //    public List<EntityModifiedEvent> ModifiedEvents
    //    {
    //        get { return ((RecorderEventEntityModified)EntityModifiedEventHandler).ModifiedEvents; }
    //    }

    //    public List<RepositorySavedEvent> SavedEvents
    //    {
    //        get { return ((RecorderEventSaved)RepositorySavedEventHandler).SavedEvents; }
    //    }
    //}


}
