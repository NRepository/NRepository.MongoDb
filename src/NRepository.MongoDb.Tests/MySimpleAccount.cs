namespace NRepository.MongoDb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MySimpleAccount : MongoEntity
    {
        public MySimpleAccount(string name, DateTime dateOfBirth, int weight)
        {
            Name = name;
            DOB = dateOfBirth;
            Weigth = weight;
        }

        public MySimpleAccount(string name, DateTime dateOfBirth)
        {
            Name = name;
            DOB = dateOfBirth;
        }

        public MySimpleAccount(string name)
        {
            Name = name;
            DOB = DateTime.Today;
        }

        public MySimpleAccount SimpleAccount2 { get; set; }

        public string Name { get; set; }

        public int? Weigth { get; set; }

        //        [BsonDateTimeOptionsAttribute(DateOnly = true)]
        public DateTime DOB { get; set; }
    }
}
