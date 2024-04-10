using MongoDB.Driver;

namespace Main.App.Domain.Bike
{
    public class BikeFilterModel
    {
        public string? Identifier { get; set; }
        public string? ManufacturingYear { get; set; }
        public string? BikeModel { get; set; }
        public string? LicensePlate { get; set; }

        public FilterDefinition<Bike> GetFilterDefinition()
        {
            var filters = new List<FilterDefinition<Bike>>();

            if (!string.IsNullOrEmpty(Identifier))
            {
                filters.Add(Builders<Bike>.Filter.Eq(x => x.Identifier, Identifier));
            }

            if (!string.IsNullOrEmpty(ManufacturingYear))
            {
                filters.Add(Builders<Bike>.Filter.Eq(x => x.ManufacturingYear, ManufacturingYear));
            }

            if (!string.IsNullOrEmpty(BikeModel))
            {
                filters.Add(Builders<Bike>.Filter.Eq(x => x.BikeModel, BikeModel));
            }

            if (!string.IsNullOrEmpty(LicensePlate))
            {
                filters.Add(Builders<Bike>.Filter.Eq(x => x.LicensePlate, LicensePlate));
            }

            if(filters.Count <= 0){
                return Builders<Bike>.Filter.Empty;
            }
            return Builders<Bike>.Filter.And(filters);
        }
    }
}

