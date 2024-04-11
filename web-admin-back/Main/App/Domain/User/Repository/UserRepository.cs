
using FluentValidation;
using Main.Settings.Database;
using Main.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.User
{
    public class UserRepository : BaseMongoRepository<UserEntity>, IUserRepository
    {
        private readonly IEncryptor _encryptor;
        private readonly ILogger<UserRepository> _logger;


        public UserRepository(IMongoDatabase mongoDatabase, IEncryptor encryptor, IValidator<UserEntity> validatorEntity,
            ILogger<UserRepository> logger) : base(mongoDatabase, "User")
        {
            _encryptor = encryptor;
            _logger = logger;
        }

        public bool Add(UserEntity user)
        {
            base.InsertOne(user);
            return true;
        }

        public List<UserEntity> Get(UserFilterModel filter)
        {
            var users = base.Find(filter.GetFilterDefinition()).ToList();
            return users;
        }

        public UserEntity GetUserByName(string userName)
        {
            var user = base.FindFirstOrDefault(Builders<UserEntity>.Filter.Eq(user => user.Name, userName));

            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            return user;
        }

        public UserEntity GetUserById(string encryptedId)
        {

            ObjectId userId = _encryptor.DecryptObjectId(encryptedId!);

            var user = base.FindFirstOrDefault(Builders<UserEntity>.Filter.Eq(user => user.Id, userId));

            if (user == null)
            {
                throw new ValidationException("User not found");
            }

            return user;
        }

        public UserEntity Update(UserEntity user)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, user.Id);

            var update = Builders<UserEntity>.Update.Set(user => user.Name, user.Name)
                .Set(user => user.Cnpj, user.Cnpj)
                .Set(user => user.Cnh, user.Cnh)
                .Set(user => user.Birthday, user.Birthday)
                .Set(user => user.HasRentedBike, user.HasRentedBike)
                .Set(user => user.Status, user.Status);

            UserEntity userUpdated = base.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<UserEntity> { ReturnDocument = ReturnDocument.After });

            if (userUpdated == null)
            {
                throw new ValidationException("User not found");
            }

            return userUpdated;

        }

        public List<UserEntity> GetUsersById(List<ObjectId> userIds)
        {
            var filter = Builders<UserEntity>.Filter.In(user => user.Id, userIds);
            return base.Find(filter).ToList();
        }

        public UserEntity GetUserByCnhOrCnpj(string cnh, string cnpj)
        {
            var filter = Builders<UserEntity>.Filter.Or(
                Builders<UserEntity>.Filter.Eq(user => user.Cnh, cnh),
                Builders<UserEntity>.Filter.Eq(user => user.Cnpj, cnpj)
            );
            return base.FindFirstOrDefault(filter);
        }

        public Task<UserEntity> GetUserById(ObjectId userId)
        {
            var filter = Builders<UserEntity>.Filter
                .Eq(user => user.Id, userId);

            return base.FindFirstAsync(filter);
        }

        public Task<UserEntity> UpdateUserOnPickUp(UserEntity user)
        {
            var filter = Builders<UserEntity>.Filter.Eq(_user => _user.Id, user.Id);

            var update = Builders<UserEntity>.Update
                .Set(_user => _user.Status, user.Status)
                .Set(_user => _user.Deliveries, user.Deliveries)
                .Set(_user => _user.DeliveringOrder, user.DeliveringOrder);

            return base.UpdateOneAndGetAsync(filter, update);
        }

        public Task<UpdateResult> UpdateUserOnDecline(UserEntity user)
        {
            var filter = Builders<UserEntity>.Filter.Eq(_user => _user.Id, user.Id);

            var update = Builders<UserEntity>.Update
                .Set(_user => _user.Status, user.Status)
                .Set(_user => _user.DeliveringOrder, user.DeliveringOrder)
                .Set(_user => _user.Deliveries, user.Deliveries);

            return base.UpdateOneAsync(filter, update);
        }
    }
}