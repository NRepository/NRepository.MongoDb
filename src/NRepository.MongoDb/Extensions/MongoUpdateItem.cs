namespace NRepository.MongoDb
{
    using NRepository.Core;
    using System;
    using System.Linq.Expressions;

    public class MongoUpdateItem<TEntity> where TEntity : class
    {
        public MongoUpdateItem(Expression<Func<TEntity, object>> property, object value)
        {
            Key = PropertyInfo<TEntity>.GetMemberName(property);
            Value = value;
        }

        public string Key
        {
            get;
            private set;
        }

        public object Value
        {
            get;
            private set;
        }
    }
}
