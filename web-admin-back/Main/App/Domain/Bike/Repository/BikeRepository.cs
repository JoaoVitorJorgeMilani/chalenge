using Main.Settings.Database;
using Main.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.Bike
{
    public class BikeRepository : BaseMongoRepository<Bike>, IBikeRepository
    {
        private readonly IEncryptor encryptor;

        public BikeRepository(IMongoDatabase mongoDatabase, IEncryptor _encryptor) : base(mongoDatabase, "Bike")
        {
            encryptor = _encryptor;
        }

        public void Add(Bike bike)
        {
            base.InsertOne(bike);
        }

        public List<BikeDto> Get(BikeFilterModel filter)
        {
            var bikes = base.Find(filter.GetFilterDefinition()).ToList();

            List<BikeDto> bikesResp = new List<BikeDto>();
            bikes.ForEach(bike => bikesResp.Add(BikeDto.Of(bike, encryptor)));

            return bikesResp;
        }

        public bool Delete(ObjectId id)
        {
            return base.DeleteOne(Builders<Bike>.Filter.Eq(bike => bike.Id, id)).IsAcknowledged;
        }

        public void Update(Bike bike)
        {
            var filter = Builders<Bike>.Filter.Eq(x => x.Id, bike.Id);

            var update = Builders<Bike>.Update
                .Set(x => x.Identifier, bike.Identifier)
                .Set(x => x.BikeModel, bike.BikeModel)
                .Set(x => x.LicensePlate, bike.LicensePlate);

            base.UpdateOne(filter, update);
        }

        public bool Edit(ObjectId id, string licensePlate)
        {
            var filter = Builders<Bike>.Filter
                .Eq(bike => bike.Id, id);

            var update = Builders<Bike>.Update.Set(bike => bike.LicensePlate, licensePlate);

            return base.UpdateOne(filter, update).IsAcknowledged;
        }
    }
}
