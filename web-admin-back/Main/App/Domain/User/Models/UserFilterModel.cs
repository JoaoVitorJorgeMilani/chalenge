using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.User
{
    public class UserFilterModel
    {
        public string? Name { get; set; }
        public string? Cnpj { get; set; }
        public string? Cnh { get; set; }
        public string? Birthday { get; set; }
        public bool? HasRentedBike { get; set; }
        public UserStatus? Status { get; set; }

        public FilterDefinition<UserEntity> GetFilterDefinition()
        {
            var filters = new List<FilterDefinition<UserEntity>>();

            if (!string.IsNullOrEmpty(Name))
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.Name, Name));
            }

            if (!string.IsNullOrEmpty(Cnpj))
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.Cnpj, Cnpj));
            }

            if (!string.IsNullOrEmpty(Cnh))
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.Cnh, Cnh));
            }

            if (!string.IsNullOrEmpty(Birthday))
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.Birthday, Birthday));
            }

            if (HasRentedBike.HasValue)
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.HasRentedBike, HasRentedBike));
            }

            if (Status.HasValue && Enum.IsDefined(typeof(UserStatus), Status))
            {
                filters.Add(Builders<UserEntity>.Filter.Eq(user => user.Status, Status));
            }

            if (filters.Count <= 0)
            {
                return Builders<UserEntity>.Filter.Empty;
            }
            return Builders<UserEntity>.Filter.And(filters);
        }
    }
}