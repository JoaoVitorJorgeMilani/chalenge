using Main.Settings.Database;

namespace Main.App.Domain.Bike
{
    public class Bike : BaseEntity
    {
        public string? Identifier { get; set; }
        public string? ManufacturingYear { get; set; }
        public string? BikeModel { get; set; }
        public string? LicensePlate { get; set; }

    }
}



