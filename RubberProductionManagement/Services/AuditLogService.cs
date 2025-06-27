using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;

namespace RubberProductionManagement.Services
{
    public class AuditLogService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string action, string tableName, string recordId, string? changes, string? description = null)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                Changes = changes,
                ChangedAt = DateTime.UtcNow,
                Description = description
            };
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
} 