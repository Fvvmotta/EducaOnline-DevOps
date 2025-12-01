using EducaOnline.Core.DomainObjects;
using EducaOnline.Core.Enums;
using EducaOnline.Core.Messages.Integration;
using EducaOnline.Identidade.API.Extensions;
using EducaOnline.Identidade.API.Models;
using EducaOnline.MessageBus;
using EducaOnline.WebAPI.Core.Controllers;
using EducaOnline.WebAPI.Core.Identidade;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducaOnline.Identidade.API.Controllers
{    
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMessageBus _messageBus;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings, IMessageBus messageBus, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _messageBus = messageBus;
            _roleManager = roleManager;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser()
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha!);
            if (result.Succeeded)
            {
                var roleExist = await _roleManager.RoleExistsAsync(nameof(PerfilUsuarioEnum.ALUNO));
                if (!roleExist)
                    await _roleManager.CreateAsync(new IdentityRole(nameof(PerfilUsuarioEnum.ALUNO)));

                await _userManager.AddToRoleAsync(user, nameof(PerfilUsuarioEnum.ALUNO));

                var clienteResult = await RegistrarAluno(usuarioRegistro);
                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }
                return CustomResponse(await GetJwt(usuarioRegistro.Email!));
            }

            foreach (var error in result.Errors)
                AdicionarErro(error.Description);

            return CustomResponse();
        }


        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {            
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email!, usuarioLogin.Senha!, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
                return CustomResponse(await GetJwt(usuarioLogin.Email!));

            if (result.IsLockedOut)
                AdicionarErro("Usuário bloqueado");
            else
                AdicionarErro("Usuário ou senha inválidos");

            return CustomResponse();

        }

        protected async Task<UsuarioRespostaLogin> GetJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user!);

            var identityClaims = await ObterClaimsDoUsuario(claims, user!);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user!, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsDoUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user!);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user!.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user!.Email!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpocheDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpocheDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim("nome", user.UserName!));
            claims.Add(new Claim("id", user.Id!));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return new ClaimsIdentity(claims);
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo!);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Subject = identityClaims,
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.HorasParaExpirar),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UsuarioRespostaLogin()
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_jwtSettings.HorasParaExpirar).TotalSeconds,
                UserToken = new UsuarioToken()
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim() { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpocheDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        
        private async Task<ResponseMessage> RegistrarAluno(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email!);
            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario!.Id), usuarioRegistro.Nome!, usuarioRegistro.Email!);
            try
            {
                return await _messageBus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }            
        }
    }
}
