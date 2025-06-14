using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;
using RubberProductionManagement.Models.DTOs;

namespace RubberProductionManagement.Services
{
    public interface IAuthService
    {
        Task<UserResponseDTO> Register(UserRegistrationDTO model);
        Task<UserResponseDTO> Login(UserLoginDTO model);
        Task<bool> ChangePassword(int userId, ChangePasswordDTO model);
        Task<User> GetUserById(int id);
        Task<IEnumerable<UserResponseDTO>> GetAllUsers();
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDTO> Register(UserRegistrationDTO model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                throw new Exception("Username already exists");
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                FullName = model.FullName,
                EmployeeCode = model.EmployeeCode,
                Role = model.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                EmployeeCode = user.EmployeeCode,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }

        public async Task<UserResponseDTO> Login(UserLoginDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                EmployeeCode = user.EmployeeCode,
                Role = user.Role,
                Token = GenerateJwtToken(user)
            };
        }

        public async Task<bool> ChangePassword(int userId, ChangePasswordDTO model)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Current password is incorrect");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.LastModifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    EmployeeCode = u.EmployeeCode,
                    Role = u.Role,
                    Token = null
                })
                .ToListAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 