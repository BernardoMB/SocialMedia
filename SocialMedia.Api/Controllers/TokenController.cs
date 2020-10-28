using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

namespace SocialMedia.Api.Controllers
{
    /**
    * (16) Controller for requesting tokens for authentication.
    * TODO: Inject the user service and check for user valid password before issuing a token.
    */
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin login) 
        {
            if (IsValidUser(login))
            {
                var token = GenerateToken();
                return Ok(new { token });
            }
            return NotFound();
        }

        private bool IsValidUser(UserLogin login)
        {
            return true;
        }

        private string GenerateToken()
        {
            // Header
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Jonathan Estrada"),
                new Claim(ClaimTypes.Email, "jestrada@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var lifeTime = int.Parse(_configuration["Authentication:Lifetime"]);
            var payload = new JwtPayload(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(0)
            );

            // Token
            var token = new JwtSecurityToken(header, payload);

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return serializedToken;
        }
    }
}
