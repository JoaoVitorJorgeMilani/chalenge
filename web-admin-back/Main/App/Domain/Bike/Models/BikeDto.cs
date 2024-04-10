using Main.Utils;

namespace Main.App.Domain.Bike
{
    public class BikeDto
    {
        public string? encryptedId { get; set; }
        public string? identifier { get; set; }
        public string? manufacturingYear { get; set; }
        public string? bikeModel { get; set; }
        public string? licensePlate { get; set; }

        public static BikeDto Of(Bike bike, IEncryptor encryptor)
        {
            return new BikeDto(
                encryptor.Encrypt(bike.Id),
                bike.Identifier,
                bike.ManufacturingYear,
                bike.BikeModel,
                bike.LicensePlate
            );
        }

        private BikeDto(
            string? encryptedId,
            string? identifier,
            string? manufacturingYear,
            string? bikeModel,
            string? licensePlate
        )
        {
            this.encryptedId = encryptedId;
            this.identifier = identifier;
            this.manufacturingYear = manufacturingYear;
            this.bikeModel = bikeModel;
            this.licensePlate = licensePlate;

        }

        public BikeDto() { }
    }
}



