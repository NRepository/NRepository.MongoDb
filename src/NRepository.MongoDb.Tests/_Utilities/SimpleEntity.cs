namespace NRepository.MongoDb.Tests._Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SimpleEntity
    {
        public static IQueryable<NRepository.MongoDb.Tests._Utilities.SimpleEntity> CreateSimpleEntities()
        {
            return new[]
            {
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(1,3),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(2,2),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(3,1),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(4,1),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(5,2),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(6,3),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(7,3),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(8,2),
                new NRepository.MongoDb.Tests._Utilities.SimpleEntity(9,1),
            }.AsQueryable();
        }

        public SimpleEntity(int id, int groupId)
        {
            GroupId = groupId;
            Id = id;
        }

        public int Id
        {
            get;
            set;
        }

        public int GroupId
        {
            get;
            private set;
        }
    }

}
