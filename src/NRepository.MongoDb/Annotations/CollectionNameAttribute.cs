namespace NRepository.MongoDb.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(Type type)
            : this(type.FullName)
        {
        }
        
        public CollectionNameAttribute(string collectionName)
        {
            if (String.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("collectionName is null or empty.", "collectionName");

            CollectionName = collectionName;
        }

        public string CollectionName
        {
            get;
            private set;
        }
    }
}
