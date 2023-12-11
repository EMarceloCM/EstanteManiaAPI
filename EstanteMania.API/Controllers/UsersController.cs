using AutoMapper;
using EstanteMania.API.Context;
using EstanteMania.API.Identity_Entities;
using EstanteMania.API.Identity_Entities.DTO_s;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EstanteMania.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] User model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                return Ok(model);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos.");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email!, userInfo.Password!, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido.");
                return BadRequest(ModelState);
            }
        }

        private UserToken BuildToken(UserDTO userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, userInfo.Email!),
                //new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName!),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]!),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
