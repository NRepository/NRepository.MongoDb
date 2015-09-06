namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.MongoDb.Annotations;

    [CollectionName("MyGreatAccount")]
    public class MyGuidAccount
    {
        public MyGuidAccount(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
