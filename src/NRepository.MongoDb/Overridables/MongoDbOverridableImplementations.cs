namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoDbOverridableImplementations : IMongoDbOverridables
    {
        private static readonly object SyncObject = new object();
        private static Dictionary<Type, PropertyInfo> _TypeToIdPropertyInfoMappings = new Dictionary<Type, PropertyInfo>();

        public BsonValue GetBsonIdValueFromEntity(object entity)
        {
            var entityType = entity.GetType();
            var propertyInfo = default(PropertyInfo);
            if (!_TypeToIdPropertyInfoMappings.TryGetValue(entityType, out propertyInfo))
            {
                lock (SyncObject)
                {
                    if (!_TypeToIdPropertyInfoMappings.ContainsKey(entityType))
                    {
                        var entityPi = default(PropertyInfo);

                        foreach (var pi in entityType.GetProperties())
                        {
                            if (pi.PropertyType == typeof(BsonIdAttribute) ||
                                pi.CustomAttributes.SingleOrDefault(p2 => p2.AttributeType == typeof(BsonIdAttribute)) != null)
                            {
                                entityPi = pi;
                                break;
                            }
                        }

                        if (entityPi == null)
                        {
                            foreach (var pi in entityType.GetProperties())
                            {
                                if (pi.Name.ToUpper().EndsWith("ID"))
                                {
                                    entityPi = pi;
                                    break;
                                }
                            }
                        }

                        if (entityPi != null)
                            _TypeToIdPropertyInfoMappings[entityType] = entityPi;
                    }
                }
            }

            if (propertyInfo == null && !_TypeToIdPropertyInfoMappings.TryGetValue(entityType, out propertyInfo))
                throw new NotSupportedException(string.Format("Unable to find BsonIdAttribute on type '{0}'. Add new IMongoDbOverridables.GetBsonIdValueFromEntity implemetation to handle this scenario.", entityType));

            var idValue = propertyInfo.GetValue(entity);
            var bsonValue = BsonTypeMapper.MapToBsonValue(idValue);
            return bsonValue;
        }
    }
}
