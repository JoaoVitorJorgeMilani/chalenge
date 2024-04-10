
using Main.App.SignalR;
using Main.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAngularApp",
            builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    }
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

ExtendedSettings.BuildExtendedSettings(builder.Services);
ExtendedSettings.CustomMongoSerializer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAngularApp");
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapDefaultControllerRoute();
    _ = endpoints.MapHub<MainHub>("api/mainhub");
});

app.Run();

