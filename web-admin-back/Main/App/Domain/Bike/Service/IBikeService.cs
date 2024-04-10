namespace Main.App.Domain.Bike 
{
    public interface IBikeService 
    {
        public void Add(Bike bike);
        public List<BikeDto> Get(BikeFilterModel filter);
        public bool Delete(string encryptedId);
        public bool Edit(string encryptedId, string licensePlate);

    }
}