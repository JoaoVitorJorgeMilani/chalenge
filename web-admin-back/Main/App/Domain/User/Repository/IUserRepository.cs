using MongoDB.Bson;

namespace Main.App.Domain.User
{
    public interface IUserRepository
    {
        public bool Add(UserEntity user);
        public List<UserDto> Get(UserFilterModel filter);
        
        public UserDto GetUserByName(string userName);
        public UserDto Update(UserDto userDto);

        public UserDto GetUserById(string encryptedId);

        public List<UserEntity> GetUsersById(List<ObjectId> userIds);
    }
}