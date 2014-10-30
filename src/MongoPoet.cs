using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace MongoPoet
{
    public class MongoPoet<T>
    {
        public MongoDatabase MongoDb { get; set; }
        public MongoCollection<T> Collection { get; set; }

        private IQueryable<T> _query;

        public IQueryable<T> Query
        {
            get 
            {
                if (_query == null)
                    _query = Collection.AsQueryable<T>();
                return _query; 
            }
            set { _query = value; }
        }

        public IMongoQuery MongoQuery
        {
            get
            {
                return ((MongoQueryable<T>)Query).GetMongoQuery();
            }
        }
        
        public MongoPoet(MongoDatabase mongoDb, MongoCollection<T> collection)
        {
            MongoDb = mongoDb;
            Collection = collection;
        }


        public MongoPoet<T> Where(Expression<Func<T, bool>> predicate)
        {

            Query = Query.Where(predicate);            
            return this;
        }

        public MongoPoet<T> Skip(int count)
        {
            Query = Query.Skip(count);
            return this;
        }

        public MongoPoet<T> Take(int count)
        {
            Query = Query.Take(count);
            return this;
        }

        public WriteConcernResult Insert(T entity)
        {
            return Collection.Insert(entity);
        }

        public WriteConcernResult Remove()
        {
            return Collection.Remove(MongoQuery);
        }

        public WriteConcernResult Remove(object id)
        {
            return Collection.Remove(MongoDB.Driver.Builders.Query.EQ("_id", BsonValue.Create(id)));

        }

        public WriteConcernResult RemoveAll()
        {
            return Collection.RemoveAll();
        }

        public WriteConcernResult Update(T entity, UpdateFlags flags = UpdateFlags.Upsert)
        {
            return Collection.Update(MongoQuery, MongoDB.Driver.Builders.Update.Replace(entity), flags);
        }

        public T FindOneByObjectId<T>(ObjectId objectId)
        {
            return Collection.FindOneByIdAs<T>(objectId);
        }

        public T Single()
        {
            return Query.Single();
        }

        public T SingleOrDefault()
        {
            return Query.SingleOrDefault();
        }

        public T First()
        {
            return Query.First();
        }

        public T FirstOrDefault()
        {
            return Query.FirstOrDefault();
        }
    }
}
