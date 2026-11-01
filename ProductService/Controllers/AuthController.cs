
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Authentication;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Models;


namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AuthController(ProductDbContext context,
    IPasswordHasher<User> passwordHasher,JwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        // LOCAL LEARNING ONLY: This does not authenticate a user.
   
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] LoginRequestDto request)
        {
            var username = request.Username.Trim();

            var existingUser = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == username);

            if(existingUser!=null)
            {
                return Conflict(new { message = "Username already exists." });

            }

            var user = new User
            {
                Username = username,
                Role = "User"
            };
            user.PasswordHash = _passwordHasher.HashPassword(
       user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var username = request.Username.Trim();
            var user = await _context.Users.FirstOrDefaultAsync
                (u => u.Username == username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var token = _jwtTokenService.GenerateToken(user.Id.ToString(),user.Role);

            return Ok(new
            {
                accessToken = token,
                tokenType = "Bearer",
                expiresInMinutes = 30
            });

        }
    }
}
