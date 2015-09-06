//namespace NRepository.MongoDb.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using MongoDB.Driver;
//    using NRepository.Core.Command;
//    using NRepository.MongoDb.Events;

//    public class MongoDbResultModifyCommandInterceptor : IModifyCommandInterceptor
//    {
//        public void Modify<T>(ICommandRepository commandRepository, Action<T> modifyAction, T entity) where T : class
//        {
//            var mongoDatabase = commandRepository.ObjectContext as MongoDatabase;
//            if (mongoDatabase == null)
//                throw new NotSupportedException("Modify action can only be used with a MongoDatabase context");
//            
//            var result = mongoDatabase.GetCollection<T>(typeof(T).FullName).Save(entity);
//            WriteConcernResult = result;

//            commandRepository.RaiseEvent(new MongoDbEntityModifiedEvent(commandRepository, entity, result));
//        }

//        public WriteConcernResult WriteConcernResult
//        {
//            get;
//            private set;
//        }

//    }
//}