namespace NRepository.MongoDb.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.Core.Command;
    using NRepository.Core.Events;

    public class MongoDbRepositorySavedEvent : RepositorySavedEvent
    {
        public MongoDbRepositorySavedEvent(ICommandRepository commandRepository) 
            : base(commandRepository)
        {
        }
    }
}
