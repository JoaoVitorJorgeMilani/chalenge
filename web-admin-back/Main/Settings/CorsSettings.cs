using Microsoft.AspNetCore.Cors.Infrastructure;
using ZstdSharp.Unsafe;

class CorsSettings
{
    public string? PolicyName { get; set; }
    public string[]? AllowedOrigins { get; set; }
    public string[]? AllowedHeaders { get; set; }
    public string[]? AllowedMethods { get; set; }
    public bool AllowCredentials { get; set; }

    public CorsPolicy GetPolicy()
    {
        var builder = new CorsPolicyBuilder();
        builder = (AllowedOrigins![0] == "*") ? builder.AllowAnyOrigin() : builder.WithOrigins(AllowedOrigins!);
        builder = (AllowedHeaders![0] == "*") ? builder.AllowAnyHeader() : builder.WithHeaders(AllowedHeaders!);
        builder = (AllowedMethods![0] == "*") ? builder.AllowAnyMethod() : builder.WithMethods(AllowedMethods!);
        builder = AllowCredentials ? builder.AllowCredentials() : builder.DisallowCredentials();
        return builder.Build();
    }

    public bool Validate()
    {
        return !string.IsNullOrEmpty(PolicyName)
        && AllowedOrigins != null && AllowedOrigins!.Length > 0
        && AllowedHeaders != null && AllowedOrigins!.Length > 0
        && AllowedMethods != null && AllowedOrigins!.Length > 0;
    }
}