namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NRepository.MongoDb.Annotations;

    internal static class CollectionHelpers
    {
        private static object SyncObject = new object();
        private static Dictionary<Type, string> _CollectionNames = new Dictionary<Type, string>();

        public static string CollectionNameFromType<T>()
        {
            return CollectionNameFromType(typeof(T));
        }

        public static string CollectionNameFromEntity<T>(T entity)
        {
            return CollectionNameFromType(entity.GetType());
        }

        public static string CollectionNameFromType(Type type)
        {
            var collectionName = default(string);
            if (_CollectionNames.TryGetValue(type, out collectionName))
                return collectionName;

            lock (SyncObject)
            {
                if (_CollectionNames.TryGetValue(type, out collectionName))
                    return collectionName;

                var attribute = type.GetCustomAttributes(typeof(CollectionNameAttribute), true).FirstOrDefault() as CollectionNameAttribute;
                if (attribute != null)
                    collectionName = attribute.CollectionName;
                else
                    collectionName = type.FullName;

                _CollectionNames.Add(type, collectionName);
            }

            return collectionName;
        }
    }
}
