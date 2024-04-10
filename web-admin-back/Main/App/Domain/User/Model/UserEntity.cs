
using Main.Settings.Database;

namespace Main.App.Domain.User
{
    public class UserEntity : BaseEntity
    {
        public string? Name { get; set; }
        
        public string? Cnpj { get; set; }

        public string? Cnh { get; set; }
        
        public string? Birthday { get; set; }

        public bool? HasRentedBike { get; set; }

        public UserStatus? Status  { get; set; }

        public UserEntity(): base() {
            Status = UserStatus.Offline;
        }


    }
}
