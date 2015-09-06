namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using NRepository.MongoDb.Annotations;

    [CollectionName("SimpleAccount")]
    public class SimpleAccount //: MongoEntity
    {
        public ObjectId Id { get; set; }

        public SimpleAccount(string name, DateTime dateOfBirth, int weight)
        {
            Name = name;
            DOB = dateOfBirth;
            Weigth = weight;
        }

        public SimpleAccount(string name, DateTime dateOfBirth)
        {
            Name = name;
            DOB = dateOfBirth;
        }

        public SimpleAccount(string name)
        {
            Name = name;
            DOB = DateTime.Today;
        }

        public SimpleAccount SimpleAccount2 { get; set; }


        public string Name { get; set; }

        public int? Weigth { get; set; }

        public DateTime DOB { get; set; }
    }
}
