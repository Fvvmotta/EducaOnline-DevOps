using EducaOnline.Pedidos.API.Configurations;
using EducaOnline.WebAPI.Core.Identidade;
using EducaOnLine.Pedidos.API.Data;

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
    .AddApiConfiguration(builder.Configuration)
    .AddJwtConfiguration(builder.Configuration)
    .AddDependenceInjectionConfiguration()
    .AddSwaggerConfiguration()
    .AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly))
    .AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var env = services.GetRequiredService<IWebHostEnvironment>();

    try
    {
        var context = services.GetRequiredService<PedidosContext>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
        {
            logger.LogInformation("Verificando banco de dados...");

            await context.Database.EnsureCreatedAsync();

            logger.LogInformation("Banco de dados verificado/criado com sucesso!");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao verificar/criar banco de dados");
        throw;
    }
}

app
    .UseApiConfiguration()
    .UseSwaggerConfiguration();

app.Run();