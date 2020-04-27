using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Filters;
using Presentation.Interfaces;
using Presentation.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IEfRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AuthController(
            AppDbContext dbContext
            , ITokenBuilder tokenBuilder
            , IEfRepository<User> userRepository
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _tokenBuilder = tokenBuilder;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]User user)
        {
            var dbUser = await _dbContext
                .UserSet
                .SingleOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null)
            {
                return NotFound("User not found.");
            }

            // This is just an example, made for simplicity; do not store plain passwords in the database
            // Always hash and salt your passwords
            var isValid = dbUser.Password == user.Password;

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _tokenBuilder.BuildToken(user.Username);

            return Ok(token);
        }

        [HttpGet("verify")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyToken()
        {
            var username = User
                .Claims
                .SingleOrDefault();

            if (username == null)
            {
                return Unauthorized();
            }

            var userExists = await _dbContext
                .UserSet
                .AnyAsync(u => u.Username == username.Value);

            if (!userExists)
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpPost("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            var user = _mapper.Map<User>(model);

            // Later this fields will be enabled disabled by apis as required.
            user.Verified = true;
            user.Active = true;
            user.Role = UserRoles.GENERAL;

            await _userRepository.AddAsync(user);

            return Ok();
        }
    }
}