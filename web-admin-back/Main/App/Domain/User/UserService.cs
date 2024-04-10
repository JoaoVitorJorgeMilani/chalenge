using MongoDB.Bson;

namespace Main.App.Domain.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public bool Add(UserEntity user)
        {
            return _userRepository.Add(user);
        }

        public List<UserDto> Get(UserFilterModel filter)
        {
            return _userRepository.Get(filter);
        }

        public UserDto Login(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("User name cannot be null or empty", nameof(userName));
            }

            var userDto = _userRepository.GetUserByName(userName);

            if (userDto == null)
            {
                throw new InvalidOperationException("User not found");
            }

            userDto.status = UserStatus.Available; // Logged User
            return _userRepository.Update(userDto);
        }

        public List<UserEntity> GetUsersById(List<ObjectId> userIds)
        {
            return _userRepository.GetUsersById(userIds);
        }

        private bool IsUserLoged(string encryptedId)
        {
            var userDto = _userRepository.GetUserById(encryptedId);
            return userDto.status == UserStatus.Available;
        }
    }
}