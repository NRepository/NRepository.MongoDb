namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using NRepository.Core.Query;
    using NRepository.Core.Query.Specification;
    using NRepository.TestKit;
    using NUnit.Framework;

    [TestFixture]
    public class MongoRepositoryTests
    {
        private static MongoDatabase CreateEmptyMongoDatabase()
        {
            var mongoClient = new MongoClient();
            var server = mongoClient.GetServer();
            var database = server.GetDatabase("NRepository_MongoDb_Tests");
            database.Drop();
            database = server.GetDatabase("NRepository_MongoDb_Tests");

            return database;
        }

        //        [Test]
        public void Seed()
        {
            var repository = new MongoDbUnitOfWorkRepository(
                CreateEmptyMongoDatabase(),
                new MongoUnitOfWorkExampleEventHandlers());

            repository.Add(new SimpleAccount("Aimee", DateTime.Today.AddYears(-6), 30));
            repository.Add(new SimpleAccount("John", DateTime.Today.AddYears(-46), 72));
            repository.Add(new SimpleAccount("Isabelle", DateTime.Today.AddYears(-35), 60));
            repository.Add(new SimpleAccount("Ellie2", DateTime.Today.AddYears(-12), 50));

            var count = repository.Save();
            count.ShouldEqual(4);
        }

        //        [Test]
        public void CheckAddAndDeleteFoMongoCommandRepository()
        {
            var mongoDatabase = CreateEmptyMongoDatabase();
            var repository = new MongoDbRepository(mongoDatabase);

            var account = new SimpleAccount("xyz");
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
            repository.Add(account);
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(true);

            repository.Delete(account);
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
        }


        //      [Test]
        public void CheckAddAndDeleteFoMongoCommandRepositoryUsingGuidAsId()
        {
            var mongoDatabase = CreateEmptyMongoDatabase();
            var repository = new MongoDbRepository(mongoDatabase);

            var account = new MyGuidAccount("xyz");
            repository.GetEntities<MyGuidAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
            repository.Add(account);
            repository.GetEntities<MyGuidAccount>(p => p.Name == account.Name).Any().ShouldEqual(true);
            repository.Delete(account);
            repository.GetEntities<MyGuidAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
            repository.Add(new MyGuidAccount("xyz"));

        }

        //        [Test]
        public void CheckAddAndDeleteFoMongoUnitOfWorkCommandRepository()
        {
            var eventRecorder = new RecordedRepositoryEvents();

            var mongoDatabase = CreateEmptyMongoDatabase();
            var repository = new MongoDbUnitOfWorkRepository(mongoDatabase, eventRecorder);

            var account = new SimpleAccount("xyz");
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
            repository.Add(account);
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
            repository.Save();
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(true);
            repository.Delete(account);
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(true);
            repository.Save();
            repository.GetEntities<SimpleAccount>(p => p.Name == account.Name).Any().ShouldEqual(false);
        }

        //        [Test]
        public void ModifyAll()
        {
            var mongoDatabase = CreateEmptyMongoDatabase();
            var repository = new MongoDbRepository(mongoDatabase);

            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 1) { Weigth = 1 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 2) { Weigth = 2 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 3) { Weigth = 3 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 4) { Weigth = 4 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 5) { Weigth = 5 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 6) { Weigth = 6 });

            var lessThanTodayQuery = new ExpressionSpecificationQueryStrategy<MySimpleAccount>(p => p.DOB < DateTime.Today);
            repository.GetEntities<MySimpleAccount>(lessThanTodayQuery).Count().ShouldEqual(3);

            var result3 = repository.ModifyAll<MySimpleAccount>(
                    lessThanTodayQuery,
                    new MongoUpdateItem<MySimpleAccount>(p => p.DOB, DateTime.Today),
                    new MongoUpdateItem<MySimpleAccount>(p => p.Weigth, 100));

            result3.DocumentsAffected.ShouldEqual(3);

            repository.GetEntities<MySimpleAccount>(lessThanTodayQuery).Count().ShouldEqual(0);
            repository.GetEntities<MySimpleAccount>(p => p.DOB == DateTime.Today).Count().ShouldEqual(6);
            repository.GetEntities<MySimpleAccount>(p => p.Weigth == 100).Count().ShouldEqual(3);
        }

        //        [Test]
        public void Paging()
        {
            var mongoDatabase = CreateEmptyMongoDatabase();
            var repository = new MongoDbRepository(mongoDatabase);

            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 1) { Weigth = 1 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 2) { Weigth = 2 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today.AddDays(-1), 3) { Weigth = 3 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 4) { Weigth = 4 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 5) { Weigth = 5 });
            repository.Add(new MySimpleAccount("xyz", DateTime.Today, 6) { Weigth = 6 });

            var rowCountFunc = default(Func<int>);
            var docs = repository.GetEntities<MySimpleAccount>(new PagingQueryStrategy(2, 2, out rowCountFunc)).ToList();

            rowCountFunc().ShouldEqual(6);
            docs.Count().ShouldEqual(2);
        }

        public void CheckSaveInterceptor2()
        {
            var repository = new MongoDbRepository(CreateEmptyMongoDatabase());
            //BsonDocument nested = new BsonDocument {
            //{ "name", "JJK" },
            //{ "address", new BsonDocument {
            //    { "street", "123 Main St." },
            //    { "city", "Centerville" },
            //    { "state", "PA" },
            //    { "zip", 12345}
            //}}};

            var crappy = new SimpleAccount("JK2");
            crappy.SimpleAccount2 = new SimpleAccount("XXX");
            repository.Add(crappy);

            // Create Single Update!!!!!!!!!!!!


            var result3 = repository.ModifyAll<SimpleAccount>(
                new ExpressionSpecificationQueryStrategy<SimpleAccount>(p => p.Name == "JK2"),
                    new MongoUpdateItem<SimpleAccount>(p => p.DOB, DateTime.Today.AddDays(77777)),
                    new MongoUpdateItem<SimpleAccount>(p => p.SimpleAccount2.Name, "XCC"),
                    new MongoUpdateItem<SimpleAccount>(p => p.SimpleAccount2.Weigth, 1236));

            //            Update.Set("DOB", DateTime.Today.AddDays(99999));
        }
    }
}