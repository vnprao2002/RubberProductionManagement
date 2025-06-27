using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;
using RubberProductionManagement.Services;
using Newtonsoft.Json;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkSessionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public WorkSessionsController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _context.WorkSessions.Include(ws => ws.WorkAssignments).ToListAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var session = await _context.WorkSessions.Include(ws => ws.WorkAssignments).FirstOrDefaultAsync(ws => ws.Id == id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkSession session)
        {
            _context.WorkSessions.Add(session);
            await _context.SaveChangesAsync();
            
            // Reload the session with related data
            var createdSession = await _context.WorkSessions
                .Include(ws => ws.WorkAssignments)
                    .ThenInclude(wa => wa.RubberLot)
                .FirstOrDefaultAsync(ws => ws.Id == session.Id);
            
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "WorkSession",
                recordId: session.Id.ToString(),
                changes: JsonConvert.SerializeObject(session),
                description: "Thêm mới phiên làm việc"
            );
            
            return CreatedAtAction(nameof(Get), new { id = session.Id }, createdSession);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WorkSession session)
        {
            if (id != session.Id) return BadRequest();
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "WorkSession",
                recordId: session.Id.ToString(),
                changes: JsonConvert.SerializeObject(session),
                description: "Cập nhật phiên làm việc"
            );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _context.WorkSessions.FindAsync(id);
            if (session == null) return NotFound();
            _context.WorkSessions.Remove(session);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "WorkSession",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(session),
                description: "Xóa phiên làm việc"
            );
            return NoContent();
        }
    }
} 