using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using SocialMedia.Core.Interfaces;
using System.Threading.Tasks;

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
        // (18) Inject the security service so token validation and login validation can be made
        private readonly ISecurityService _securityService;
        // (19.2) Inject the password service
        private readonly IPasswordService _passwordService;

        public TokenController(
            IConfiguration configuration,
            // (18) Inject the security service so token validation and login validation can be made
            ISecurityService securityService,
            // (19.2) Inject the password service
            IPasswordService passwordService
        )
        {
            _configuration = configuration;
            // (18) Inject the security service so token validation and login validation can be made
            _securityService = securityService;
            // (19.2) Inject the password service
            _passwordService = passwordService;
        }

        /// <summary>
        /// Login route
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Authentication(UserCredentials login) 
        {
            //if (IsValidUser(login))
            //{
            //    var token = GenerateToken();
            //    return Ok(new { token });
            //}
            //return NotFound();

            // (18) Now we are actually validating the user, then do the following:
            var validation = await IsValidUser(login);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }
            return NotFound();
        }

        //private async Task<Tuple<bool, Security>> IsValidUser(UserLogin login)
        // Better return the tuple with thw following syntax
        private async Task<(bool, Security)> IsValidUser(UserCredentials credentials)
        {
            //return true;
            // (18) Modify the method to actually validate a user
            var user = await _securityService.GetSecurityByUsername(credentials.Username);
            var isValid = _passwordService.Check(user.Password, credentials.Password);
            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            // Header
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            // Payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, security.UserName),
                new Claim("User", security.User),
                new Claim(ClaimTypes.Role, security.Role.ToString())
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
