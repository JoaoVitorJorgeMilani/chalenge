using FluentValidation;
using Main.App.Consumer;
using Main.App.Domain.Bike;
using Main.App.Domain.Order;
using Main.App.Domain.User;
using Main.App.Messaging;
using Main.App.Redis;
using Main.App.SignalR;
using Main.Settings.Database;
using Main.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NLog.Extensions.Logging;
using Polly;
using StackExchange.Redis;

namespace Main.Settings
{
    public static class ExtendedSettings
    {
        public static void BuildExtendedSettings(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddNLog(configuration);
            });

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            services.AddSingleton(serviceProvider =>
            {
                return new RedisConnectionService(loggerFactory.CreateLogger<RedisConnectionService>(), configuration.GetSection("RedisSettings")!.Get<RedisSettings>()!);
            });

            services.AddSingleton(serviceProvider =>
            {
                return new RabbitMQConnectionService(configuration, loggerFactory.CreateLogger<RabbitMQConnectionService>());
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


            BuildCorsPolicies(services, configuration);

            // Utils
            services.AddSingleton<IEncryptor, Encryptor>();
            services.AddSingleton<ILoggerFactory>(loggerFactory);

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
            services.AddSingleton<IValidator<UserEntity>, UserValidator>();
            services.AddSingleton<IValidator<UserDto>, UserDtoValidator>();


            // Messaging
            services.AddSingleton<IMessagingService, RabbitMQService>();

            // SignalR
            services.AddSignalR();
            services.AddSingleton<HubService>();

            // Redis
            // services.AddSingleton<RedisConnectionService>();
            services.AddSingleton<RedisService>();

            // Consumer
            services.AddHostedService<HostedService>();
            services.AddSingleton<OrderConsumer>();
        }

        private static void BuildCorsPolicies(IServiceCollection services, IConfiguration configuration)
        {
            List<CorsSettings> corsSettings = configuration.GetSection("CorsSettings").Get<List<CorsSettings>>()
                ?? throw new InvalidOperationException("Error while loading CORS settings");

            if (corsSettings == null || corsSettings.Count == 0 || corsSettings.Any(policie => !policie.Validate()))
            {
                throw new InvalidOperationException("Verify the Cors settings on appsettings.json.");
            }

            corsSettings.ForEach(policie =>
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(policie.PolicyName!, policie.GetPolicy());
                });
            });
        }

        public static void CustomMongoSerializer()
        {
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("EnumStringConvention", pack, _ => true);
        }

        public static void RegisterShutdownPolicies(IServiceProvider provider)
        {
            var applicationLifetime = provider.GetRequiredService<IHostApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(async () =>
            {
                Console.WriteLine("Shutting down all services");
                provider.GetRequiredService<RabbitMQConnectionService>().Dispose();
                provider.GetRequiredService<RedisConnectionService>().Dispose();
                provider.GetRequiredService<IMessagingService>().Dispose();
                await provider.GetRequiredService<HubService>().Dispose();

                throw new Exception("Something went wrong");
            });
        }

    }
}

