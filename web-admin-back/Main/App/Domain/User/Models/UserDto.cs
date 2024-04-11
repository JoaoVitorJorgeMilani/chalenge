using Main.Utils;

namespace Main.App.Domain.User
{
    public class UserDto
    {
        public string? encryptedId { get; set; }

        public string? name { get; set; }

        public string? cnpj { get; set; }

        public string? cnh { get; set; }

        public string? birthday { get; set; }

        public bool? hasRentedBike { get; set; }

        public string? status { get; set; }

        public string? deliveringOrder { get; set; }

        public static UserDto Of(UserEntity user, IEncryptor encryptor)
        {
            return new UserDto()
            {
                encryptedId = encryptor.Encrypt(user.Id),
                name = user.Name,
                cnpj = user.Cnpj,
                cnh = user.Cnh,
                birthday = user.Birthday,
                hasRentedBike = user.HasRentedBike,
                status = user.Status.ToString(),
                deliveringOrder = encryptor.Encrypt(user.DeliveringOrder)
            };
        }
    }
}