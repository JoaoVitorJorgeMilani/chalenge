using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.User
{
    public interface IUserRepository
    {
        public bool Add(UserEntity user);
        public List<UserEntity> Get(UserFilterModel filter);

        public UserEntity GetUserByName(string userName);
        public UserEntity Update(UserEntity userDto);

        public UserEntity GetUserById(string encryptedId);
        public Task<UserEntity> GetUserById(ObjectId userId);
        public List<UserEntity> GetUsersById(List<ObjectId> userIds);
        public UserEntity GetUserByCnhOrCnpj(string cnh, string cnpj);
        public Task<UserEntity> UpdateUserOnPickUp(UserEntity user);
        public Task<UpdateResult> UpdateUserOnDecline(UserEntity user);
    }
}