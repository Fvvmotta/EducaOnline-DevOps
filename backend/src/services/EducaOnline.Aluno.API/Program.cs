
using EducaOnline.Aluno.API.Configuration;
using EducaOnline.Catalogo.API.Configurations;
using EducaOnline.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();


builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddDependencyConfig();
builder.Services.AddMessageBusConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);

var app = builder.Build();
await DbMigrationHelpers.EnsureSeedData(app);

app.UseApiConfig(app.Environment);

app.Run();
