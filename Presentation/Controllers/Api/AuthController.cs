using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using Presentation.Interfaces;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEfRepository<User> _userRepository;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AuthController(
            IEfRepository<User> userRepository
            , ITokenBuilder tokenBuilder
            , IPasswordHasher passwordHasher
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenBuilder = tokenBuilder;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginBindingModel model)
        {
            var user = await _userRepository.FindAsync(x => x.Email == model.Email);

            if (user == null) return NotFound("User not found.");

            var (Verified, NeedsUpgrade) = _passwordHasher.Check(user.Password, model.Password);
            
            if (!Verified)
                return BadRequest("Could not authenticate user.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

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
                return Unauthorized();

            var user = await _userRepository.ExistsAsync(u => u.Username == username.Value);

            if (!user) return Unauthorized();

            return NoContent();
        }

        [HttpPost("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            // Check if user already exists
            var userExists = await _userRepository.ExistsAsync(x => x.Email == model.Email.Trim());
            if (userExists) return BadRequest("User already exists");

            var user = _mapper.Map<User>(model);

            user.Password = _passwordHasher.Hash(model.Password);
            user.Username = model.Email;

            // Later this fields will be enabled disabled by apis as required.
            user.Verified = true;
            user.Active = true;
            user.Role = UserRoles.User;

            await _userRepository.AddAsync(user);

            return Ok();
        }
    }
}