using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Controller]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration, 
                SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var response = await userManager.CreateAsync(user, userCredentials.Password);

            if(response.Succeeded)
            {
                return await BuildToken(userCredentials);
            } 
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password,
                                isPersistent: false, lockoutOnFailure: false);
            if(result.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Error al logearse");
            }
        }

        [HttpGet("renewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponse>> Renew()
        {
            var emailClaims = HttpContext.User.Claims.Where(claim => claim.Type == "Email").FirstOrDefault();//Permite obtener valores del JWT
            var email = emailClaims.Value;

            var userCredentials = new UserCredentials { Email = email };
            return await BuildToken(userCredentials);
        }

        [HttpPost("create-admin")]
        public async Task<ActionResult> CreateAdmin(EditUserDTO userDTO)
        {
            var newAdmin = await userManager.FindByEmailAsync(userDTO.Email);
            await userManager.AddClaimAsync(newAdmin, new Claim("Admin", "Admin"));

            return NoContent();
        }

        [HttpPost("remove-admin")]
        public async Task<ActionResult> RemoveAdmin(EditUserDTO userDTO)
        {
            var newAdmin = await userManager.FindByEmailAsync(userDTO.Email);
            await userManager.RemoveClaimAsync(newAdmin, new Claim("Admin", "true"));

            return NoContent();
        }

        private async Task<AuthenticationResponse> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("Email", userCredentials.Email) 
            };

            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDB); //

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwtkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, 
                                    claims: claims, expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration,
            };
        }
    }
}
