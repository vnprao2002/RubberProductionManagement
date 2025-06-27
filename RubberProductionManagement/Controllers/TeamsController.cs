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
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public TeamsController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _context.Teams.Include(t => t.Employees).ToListAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var team = await _context.Teams.Include(t => t.Employees).FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "Team",
                recordId: team.Id.ToString(),
                changes: JsonConvert.SerializeObject(team),
                description: "Thêm mới đội"
            );
            return CreatedAtAction(nameof(Get), new { id = team.Id }, team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Team team)
        {
            if (id != team.Id) return BadRequest();
            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "Team",
                recordId: team.Id.ToString(),
                changes: JsonConvert.SerializeObject(team),
                description: "Cập nhật đội"
            );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "Team",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(team),
                description: "Xóa đội"
            );
            return NoContent();
        }
    }
} 