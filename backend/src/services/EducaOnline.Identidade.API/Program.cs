using EducaOnline.Identidade.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services
    .AddIdentityConfiguration(builder.Configuration)
    .AddApiConfiguration()
    .AddSwaggerConfiguration()
    .AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();
DbMigrationHelpers.EnsureSeedData(app).Wait();

app
    .UseSwaggerConfiguration()
    .UseApiConfiguration();

app.Run();
