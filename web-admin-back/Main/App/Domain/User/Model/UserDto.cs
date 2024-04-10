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
        
        public UserStatus? status  { get; set; }

        public static UserDto Of(UserEntity user, IEncryptor encryptor)
        {            
            return new UserDto(
                encryptor.Encrypt(user.Id),
                user.Name,
                user.Cnpj,
                user.Cnh,
                user.Birthday,
                user.HasRentedBike,
                user.Status
            );
        }

        public UserDto(
            string? encryptedId,
            string? nome,
            string? cnpj,
            string? cnh,
            string? birthday,
            bool? hasRentedBike,
            UserStatus? status
        )
        {
            this.encryptedId = encryptedId;
            this.name = nome;
            this.cnpj = cnpj;
            this.cnh = cnh;
            this.birthday = birthday;
            this.hasRentedBike = hasRentedBike;
            this.status = status;
        }

        public UserDto() { }

        public bool Validate(){
            return !(string.IsNullOrEmpty(encryptedId) 
                && string.IsNullOrEmpty(name) 
                && string.IsNullOrEmpty(cnpj) 
                && string.IsNullOrEmpty(cnh)
                && string.IsNullOrEmpty(birthday)
            );
        }

    }
}