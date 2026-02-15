var builder = WebApplication.CreateBuilder(args);

// YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Map reverse proxy
app.MapReverseProxy();

app.Run();
