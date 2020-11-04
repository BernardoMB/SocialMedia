using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Api.Controllers
{
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IMapper _mappper;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;

        public SecurityController(
            IMapper mapper,
            ISecurityService securityService,
            IPasswordService passwordService
        ) {
            _mappper = mapper;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mappper.Map<Security>(securityDto);
            security.Password = _passwordService.Hash(securityDto.Password);
            await _securityService.RegisterUser(security);
            securityDto = _mappper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }
    }
}
