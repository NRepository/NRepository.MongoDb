namespace NRepository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MongoDbOverridables
    {
        public static IMongoDbOverridables Instance = new MongoDbOverridableImplementations();

        public static void SetImplementation(IMongoDbOverridables newImplementation)
        {
            Instance = newImplementation;
        }
    }
}
