namespace NRepository.MongoDb.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core.Query.Specification;
    using MongoDB.Bson;

    public class FindByIdMongoSpecificationStrategy<T> : SpecificationQueryStrategy<T> where T : class, IMongoEntity
    {
        public FindByIdMongoSpecificationStrategy(ObjectId id)
        {
            Id = id;
        }

        public ObjectId Id
        {
            get;
            private set;
        }

        public override Expression<Func<T, bool>> SatisfiedBy(object additionalQueryData)
        {
            return p => p.Id == Id;
        }
    }
}
