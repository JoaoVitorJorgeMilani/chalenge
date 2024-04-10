
using System.ComponentModel.DataAnnotations;
using Main.Settings.Database;
using Main.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.User
{
    public class UserRepository : BaseMongoRepository<UserEntity>, IUserRepository
    {
        private readonly IEncryptor encryptor;

        public UserRepository(IMongoDatabase mongoDatabase, IEncryptor encryptor) : base(mongoDatabase, "User")
        {
            this.encryptor = encryptor;
        }
        
        public bool Add(UserEntity user)
        {
             try
            {
                base.InsertOne(user);
                return true;
            }
            catch (MongoWriteException)
            {
                return false;
            }
        }

        public List<UserDto> Get(UserFilterModel filter)
        {   
            var users = base.Find(filter.GetFilterDefinition()).ToList();        
            List<UserDto> userResp = new List<UserDto>();
            users.ForEach(user => userResp.Add(UserDto.Of(user, encryptor)));

            return userResp;
        }

        public UserDto GetUserByName(string userName)
        {            
            var user = base.FindFirstOrDefault(Builders<UserEntity>.Filter.Eq(user => user.Name, userName));

            if(user == null)
            {
                throw new ValidationException("User not found");
            }

            return UserDto.Of(user, encryptor);
        }

        public UserDto GetUserById(string encryptedId)
        {            
            
            ObjectId userId = encryptor.DecryptObjectId(encryptedId!);

            var user = base.FindFirstOrDefault(Builders<UserEntity>.Filter.Eq(user => user.Id, userId));

            if(user == null)
            {
                throw new ValidationException("User not found");
            }

            return UserDto.Of(user, encryptor);
        }

        public UserDto Update(UserDto user)
        {
            if(!user.Validate())
            {
                throw new ValidationException("Invalid User");
            }

            ObjectId userId = encryptor.DecryptObjectId(user.encryptedId!);
            
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Id, userId);

            var update = Builders<UserEntity>.Update.Set(user => user.Name, user.name)
                .Set(user => user.Cnpj, user.cnpj)
                .Set(user => user.Cnh, user.cnh)
                .Set(user => user.Birthday, user.birthday)
                .Set(user => user.HasRentedBike, user.hasRentedBike)
                .Set(user => user.Status, user.status);


            UserEntity userUpdated = base.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<UserEntity> { ReturnDocument = ReturnDocument.After });

            if(userUpdated == null)
            {
                throw new ValidationException("User not found");
            }
            return UserDto.Of(userUpdated, encryptor);

        }

        public List<UserEntity> GetUsersById(List<ObjectId> userIds)
        {
            var filter = Builders<UserEntity>.Filter.In(user => user.Id, userIds);
            return base.Find(filter).ToList();
        }
    }
}