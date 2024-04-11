using FluentValidation;
using Main.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptor _encryptor;
        private readonly IValidator<UserEntity> _validatorEntity;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IEncryptor encryptor, IValidator<UserEntity> validatorEntity)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _encryptor = encryptor;
            _validatorEntity = validatorEntity;
        }

        public bool Add(UserEntity user)
        {
            if (!_validatorEntity.Validate(user).IsValid)
            {
                _logger.LogError("UserService - Add() | Invalid User");
            }

            if (UserAlreadyExist(user))
            {
                _logger.LogError("UserService - Add() | User already exists");
                throw new ValidationException("User already exists");
            }

            // TODO Adiciona bike alugada apenas para testes
            // depois implementar o aluguel na tela.
            user.HasRentedBike = true;

            return _userRepository.Add(user);
        }

        public List<UserDto> Get(UserFilterModel filter)
        {
            return _userRepository.Get(filter).Select(user => UserDto.Of(user, _encryptor)).ToList();
        }

        public UserDto Login(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogError("UserService - Login() | User name cannot be null or empty");
                throw new ArgumentException("User name cannot be null or empty", nameof(userName));
            }

            var user = _userRepository.GetUserByName(userName);

            if (user == null)
            {
                _logger.LogError("UserService - Login() | User not found");
                throw new InvalidOperationException("User not found");
            }

            if (user.Status == UserStatus.Offline)
                user.Status = UserStatus.Available;

            return UserDto.Of(_userRepository.Update(user), _encryptor);
        }

        public List<UserEntity> GetUsersById(List<ObjectId> userIds)
        {
            return _userRepository.GetUsersById(userIds);
        }

        public async Task<UserEntity> UpdateUserOnPickUp(ObjectId userId, ObjectId orderId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                _logger.LogError("UserService - UpdateUserOnPickUp() | User not found");
                throw new InvalidOperationException("User not found");
            }

            ValidateUserToDelivery(user, orderId);

            user.Status = UserStatus.Delivering;
            user.DeliveringOrder = orderId;
            user.Deliveries!.Add(Delivery.Of(orderId, DeliveryStatus.OnDelivery));


            var updatedUser = await _userRepository.UpdateUserOnPickUp(user);

            if (updatedUser == null)
            {
                _logger.LogError("UserService - UpdateUserOnPickUp() | User cannot be updated");
                throw new InvalidOperationException("User cannot be updated");
            }

            return updatedUser;
        }

        public async Task UpdateUserOnDecline(ObjectId userId, ObjectId orderId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                _logger.LogError("UserService - UpdateUserOnDecline() | User not found");
                throw new InvalidOperationException("User not found");
            }

            if (user.Status != UserStatus.Delivering)
            {
                _logger.LogError("UserService - UpdateUserOnDecline() | User is not delivering");
                throw new InvalidOperationException("User is not delivering");
            }

            user.Status = UserStatus.Available;
            user.DeliveringOrder = null;
            user.Deliveries!.ForEach(delivery =>
            {
                if (delivery.OrderId == orderId)
                {
                    delivery.UpdateDate = DateTime.Now;
                    delivery.Status = DeliveryStatus.DeliveryDeclined;
                }
            });

            var updateResult = await _userRepository.UpdateUserOnDecline(user);

            if (!(updateResult.IsAcknowledged && updateResult.MatchedCount > 0 && updateResult.ModifiedCount > 0))
            {
                _logger.LogError("UserService - UpdateUserOnDecline() | User cannot be updated");
                throw new InvalidOperationException("User cannot be updated");
            }

        }

        private void ValidateUserToDelivery(UserEntity user, ObjectId orderId)
        {
            _logger.LogInformation("UserService - CanUserDeliveryOrder() | OrderId: {OrderId}", orderId);

            if (!user.HasRentedBike)
            {
                _logger.LogError("UserService - CanUserDeliveryOrder() | User has no bike rented");
                throw new InvalidOperationException("User has no bike rented");
            }

            if (user.Status != UserStatus.Available)
            {
                _logger.LogError("UserService - CanUserDeliveryOrder() | User isnt available");
                throw new InvalidOperationException("User isnt available");
            }

            if (user.Deliveries!.Find(delivery => delivery.OrderId == orderId) != null)
            {
                _logger.LogError("UserService - CanUserDeliveryOrder() | User already try to delivery this order");
                throw new InvalidOperationException("User already try to delivery this order");
            }
        }

        private bool UserAlreadyExist(UserEntity user)
        {
            return _userRepository.GetUserByCnhOrCnpj(user.Cnpj!, user.Cnh!) != null;
        }


    }
}