using MongoDB.Bson;

namespace Main.App.Domain.Bike
{
    public interface IBikeRepository
    {
        public void Add(Bike bike);
        public List<BikeDto> Get(BikeFilterModel filter);
        public bool Delete(ObjectId id);
        public void Update(Bike bike);
        public bool Edit(ObjectId id, string licensePlate);

    }
}

