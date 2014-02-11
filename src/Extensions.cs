using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace MongoPoet
{
    public static class Extensions
    {
        static public string CollectionName(Type type)
        {
            string name = String.Empty;

            var attributes = type.GetCustomAttributes(typeof(CollectionNameAttribute), false);

            if (attributes.Any())
            {
                CollectionNameAttribute colname = attributes.First() as CollectionNameAttribute;
                name = colname.Name;
            }

            if (name == String.Empty)
                return type.Name.ToLower();

            return name;
        }

        static public MongoPoet<T> From<T>(this MongoDatabase mongoDb)
        {
            return new MongoPoet<T>(mongoDb, mongoDb.GetCollection<T>(CollectionName(typeof(T)))); 
        }
    }
}
