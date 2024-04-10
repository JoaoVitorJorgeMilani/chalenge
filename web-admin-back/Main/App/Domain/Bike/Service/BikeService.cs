using FluentValidation;
using Main.Utils;
using MongoDB.Bson;

namespace Main.App.Domain.Bike 
{
    public class BikeService : IBikeService
    {
        private readonly IBikeRepository repository;
        private readonly IValidator<Bike> validator;
        private readonly IEncryptor encryptor;

        public BikeService(IBikeRepository _repository, IValidator<Bike> _validator, IEncryptor _encryptor)
        {
            repository = _repository;
            validator = _validator;
            encryptor = _encryptor;
        }

        public void Add(Bike bike)
        {
            var validationResult = validator.Validate(bike);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => $"{e.ErrorCode}: {e.ErrorMessage}");
                throw new ValidationException(string.Join("; ", errors));
            }

            //Verify unique plate
            if (repository.Get(new BikeFilterModel { LicensePlate = bike.LicensePlate}).Count > 0)
            {
                throw new ValidationException("License plate already exists");
            }
            
            repository.Add(bike);         
        }

        public List<BikeDto> Get(BikeFilterModel filter)
        {
            return repository.Get(filter);

        }

        public bool Delete(string encryptedId)
        {
            
            return repository.Delete(this.encryptor.DecryptObjectId(encryptedId));
        }

        public bool Edit(string encryptedId, string licensePlate) 
        {
            if(string.IsNullOrEmpty(licensePlate) || licensePlate.Length != 7){
                throw new ValidationException("Invalid LicensePlate");
            }

            ObjectId id = encryptor.DecryptObjectId(encryptedId);
            
            return repository.Edit(id, licensePlate);
        }

    }
}
