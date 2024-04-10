using FluentValidation;
using Main.App.Domain.Bike;
using Main.App.Domain.Order;
using Main.App.Domain.User;
using Main.App.Redis;
using Main.App.Settings.Messaging;
using Main.App.Settings.Messaging.RabbitMQ;
using Main.App.SignalR;
using Main.Settings.Database;
using Main.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Main.Settings
{
    public static class ExtendedSettings
    {
        public static void BuildExtendedSettings(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var redisSettings = serviceProvider.GetRequiredService<IOptions<RedisSettings>>().Value;
                if (redisSettings == null || string.IsNullOrEmpty(redisSettings.ConnectionString))
                {
                    throw new InvalidOperationException("RedisSettings not found, review appsettings.json");
                }
                return ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
            });

            services.AddSingleton(serviceProvider =>
            {
                var databaseSettings = configuration.GetSection("DatabaseSettings").Get<MongoSettings>();

                if (databaseSettings == null)
                {

                    throw new InvalidOperationException("DatabaseSettings not found, review appsettings.json");
                }

                return new MongoClient(databaseSettings.GetConnectionStringWithDatabaseName()).GetDatabase(databaseSettings.DatabaseName);
            });




            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                    );
            });

            services.AddSignalR();

            // Utils
            services.AddSingleton<IEncryptor, Encryptor>();

            // Bike
            services.AddSingleton<IBikeRepository, BikeRepository>();
            services.AddSingleton<IBikeService, BikeService>();
            services.AddSingleton<IValidator<Bike>, BikeValidator>();

            // Order
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IOrderService, OrderService>();

            // User
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepository, UserRepository>();

            // Messaging
            services.AddSingleton<IMessagingService, RabbitMQService>();

            // SignalR
            services.AddSignalR();
            services.AddSingleton<HubService>();
            
            // Redis
            services.AddSingleton<RedisService>();



        }


        public static void CustomMongoSerializer()
        {
            var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };

            ConventionRegistry.Register("EnumStringConvention", pack, _ => true);
        }

    }
}

