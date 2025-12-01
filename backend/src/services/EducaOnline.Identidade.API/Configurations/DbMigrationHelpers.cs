using EducaOnline.Core.Enums;
using EducaOnline.Identidade.API.Data;
using EducaOnline.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EducaOnline.Identidade.API.Configurations
{
    public class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            var services = app.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var passwordHash = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var ssoContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await DbHealthChecker.TestConnection(ssoContext);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await ssoContext.Database.EnsureCreatedAsync();

                var usuarioDb = userManager.FindByEmailAsync("admin@educaonline.com.br").Result;
                if (usuarioDb == null)
                {
                    var identityUser = new IdentityUser("admin@educaonline.com.br");
                    identityUser.Email = "admin@educaonline.com.br";

                    var result = userManager.CreateAsync(identityUser).Result;

                    if (result.Succeeded)
                    {

                        CreateRoles(roleManager).Wait();

                        await userManager.AddToRoleAsync(identityUser, PerfilUsuarioEnum.ADM.ToString());


                        var hash = passwordHash.HashPassword(identityUser, "Teste@123");
                        identityUser.SecurityStamp = Guid.NewGuid().ToString();
                        identityUser.PasswordHash = hash;
                        userManager.UpdateAsync(identityUser).Wait();
                    }
                }

                var usuarioDbAluno = userManager.FindByEmailAsync("aluno@educaonline.com.br").Result;
                if (usuarioDbAluno == null)
                {
                    var identityUser = new IdentityUser("aluno@educaonline.com.br");
                    identityUser.Email = "aluno@educaonline.com.br";
                    identityUser.Id = "40640fec-5daf-4956-b1c0-2fde87717b66";

                    var result = userManager.CreateAsync(identityUser).Result;

                    if (result.Succeeded)
                    {
                        CreateRoles(roleManager).Wait();

                        await userManager.AddToRoleAsync(identityUser, PerfilUsuarioEnum.ALUNO.ToString());

                        var hash = passwordHash.HashPassword(identityUser, "Teste@123");
                        identityUser.SecurityStamp = Guid.NewGuid().ToString();
                        identityUser.PasswordHash = hash;
                        userManager.UpdateAsync(identityUser).Wait();
                    }
                }
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] rolesNames = {
                PerfilUsuarioEnum.ADM.ToString(),
                PerfilUsuarioEnum.ALUNO.ToString(),
            };

            foreach (var namesRole in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
        }
    }
}
