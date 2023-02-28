using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(op => {
    op.HttpStatusCode = 420;
    op.EnableEndpointRateLimiting = true;
    op.GeneralRules = new List<RateLimitRule>()
    {
        new RateLimitRule()
        {
            Endpoint = "*",
            Period = "20s",
            Limit = 3
        }
    };
});
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
var app = builder.Build();
app.UseIpRateLimiting();
app.MapGet("/", () => "Hello World!");
app.Run();