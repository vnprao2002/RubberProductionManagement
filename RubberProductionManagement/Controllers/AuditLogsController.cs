using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.API.Data;
using RubberProductionManagement.API.Models;

namespace RubberProductionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuditLogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _context.AuditLogs.ToListAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var log = await _context.AuditLogs.FindAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = log.Id }, log);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AuditLog log)
        {
            if (id != log.Id) return BadRequest();
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _context.AuditLogs.FindAsync(id);
            if (log == null) return NotFound();
            _context.AuditLogs.Remove(log);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 