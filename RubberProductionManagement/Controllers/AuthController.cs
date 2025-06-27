using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RubberProductionManagement.Models.DTOs;
using RubberProductionManagement.Services;
using Newtonsoft.Json;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AuditLogService _auditLogService;

        public AuthController(IAuthService authService, AuditLogService auditLogService)
        {
            _authService = authService;
            _auditLogService = auditLogService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        

        public async Task<ActionResult<UserResponseDTO>> Register(UserRegistrationDTO model)
        {
            try
            {
                var result = await _authService.Register(model);
                await _auditLogService.LogAsync(
                    action: "Create",
                    tableName: "User",
                    recordId: result.Id.ToString(),
                    changes: JsonConvert.SerializeObject(result),
                    description: "Tạo tài khoản mới"
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseDTO>> Login(UserLoginDTO model)
        {
            try
            {
                var result = await _authService.Login(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                await _authService.ChangePassword(userId, model);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();
            return Ok(users);
        }
    }
} 