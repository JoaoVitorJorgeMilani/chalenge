
using Main.App.Messaging;
using Main.App.Redis;
using Main.App.SignalR;
using Main.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

ExtendedSettings.BuildExtendedSettings(builder.Services);
ExtendedSettings.CustomMongoSerializer();

var app = builder.Build();

ExtendedSettings.RegisterShutdownPolicies(app.Services);

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowLocal");
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapDefaultControllerRoute();
    _ = endpoints.MapHub<MainHub>("api/mainhub");
});

app.Run();

