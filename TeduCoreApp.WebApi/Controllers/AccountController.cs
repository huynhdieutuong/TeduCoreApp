using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Models.AccountViewModels;

namespace TeduCoreApp.WebApi.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new BadRequestObjectResult("User doesn't exists");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("fullName", user.FullName),
                new Claim("avatar", user.Avatar ?? string.Empty),
                new Claim("roles", string.Join(";", roles)),
                new Claim("permissions", ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            _logger.LogError(_config["Tokens"]);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
            _logger.LogInformation(1, "User logged in.");

            return new OkObjectResult(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
