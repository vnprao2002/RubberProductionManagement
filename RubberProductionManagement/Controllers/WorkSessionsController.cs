using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.API.Data;
using RubberProductionManagement.API.Models;

namespace RubberProductionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkSessionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WorkSessionsController(AppDbContext context)
        {
            _context = context;
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
            
            return CreatedAtAction(nameof(Get), new { id = session.Id }, createdSession);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WorkSession session)
        {
            if (id != session.Id) return BadRequest();
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _context.WorkSessions.FindAsync(id);
            if (session == null) return NotFound();
            _context.WorkSessions.Remove(session);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 