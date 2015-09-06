//namespace NRepository.MongoDb.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using MongoDB.Driver;
//    using NRepository.Core.Command;
//    using NRepository.MongoDb.Events;

//    public class MongoDbResultAddCommandInterceptor : IAddCommandInterceptor
//    {
//        public void Add<T>(ICommandRepository commandRepository, Action<T> modifyAction, T entity) where T : class
//        {
//            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
//            if (mongoDatabase == null)
//                throw new NotSupportedException("Add can only be used with a MongoDatabase context");

//            var result = mongoDatabase.GetCollection<T>(typeof(T).FullName).Save(entity);
//            WriteConcernResult = result;

//            commandRepository.RaiseEvent(new MongoDbEntityAddedEvent(commandRepository, entity, result));
//        }

//        public WriteConcernResult WriteConcernResult
//        {
//            get;
//            private set;
//        }
//    }
//}
