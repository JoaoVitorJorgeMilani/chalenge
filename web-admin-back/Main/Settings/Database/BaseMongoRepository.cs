using MongoDB.Driver;

namespace Main.Settings.Database
{
   public class BaseMongoRepository<T> where T : BaseEntity
   {
      private IMongoCollection<T> collection { get; set; }

      public BaseMongoRepository(IMongoDatabase mongoDatabase, string collectionName)
      {
         collection = mongoDatabase.GetCollection<T>(collectionName);
      }

      public virtual bool Save(T document)
      {
         try
         {
            document.UpdateDate = DateTime.Now;
            collection.InsertOne(document);
            return true;
         }
         catch (MongoWriteException)
         {
            return false;
         }
      }

      public virtual UpdateResult UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update)
      {
         update = update.Set("UpdateDate", DateTime.Now);
         return collection.UpdateMany(filter, update);
      }

      public virtual UpdateResult UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update)
      {
         update = update.Set("UpdateDate", DateTime.Now);
         return collection.UpdateOne(filter, update);
      }

      public virtual Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
      {
         update = update.Set("UpdateDate", DateTime.Now);
         return collection.UpdateOneAsync(filter, update);
      }

      public virtual async Task<T> UpdateOneAndGetAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
      {
         update = update.Set("UpdateDate", DateTime.Now);
         var options = new FindOneAndUpdateOptions<T>
         {
            ReturnDocument = ReturnDocument.After
         };
         return await collection.FindOneAndUpdateAsync(filter, update, options);
      }

      public virtual T FindOneAndUpdate(FilterDefinition<T> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options)
      {
         update = update.Set("UpdateDate", DateTime.Now);
         return collection.FindOneAndUpdate(filter, update, options);
      }

      public virtual T FindFirstOrDefault(FilterDefinition<T> filter)
      {
         return collection.Find(filter).FirstOrDefault();
      }

      public virtual Task<T> FindFirstAsync(FilterDefinition<T> filter)
      {
         return collection.Find(filter).FirstAsync();
      }

      public virtual void InsertOne(T entity)
      {
         collection.InsertOne(entity);
      }

      public virtual IFindFluent<T, T> Find(FilterDefinition<T> filter)
      {
         return collection.Find(filter);
      }

      public virtual Task<IAsyncCursor<T>> FindAsync(FilterDefinition<T> filter)
      {
         return collection.FindAsync(filter);
      }

      public virtual DeleteResult DeleteOne(FilterDefinition<T> filter)
      {
         return collection.DeleteOne(filter);
      }
   }

}
