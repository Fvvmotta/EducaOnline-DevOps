
using EducaOnline.Conteudo.API.Configuration;
using EducaOnline.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();


builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddDependencyConfig();
//builder.Services.AddAutoMapper(typeof(ConteudoMapperConfig));

var app = builder.Build();

DbMigrationHelpers.EnsureSeedData(app).Wait();
app.UseApiConfig(app.Environment);

app.Run();
