using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyProductionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DailyProductionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productions = await _context.DailyProductions.Include(dp => dp.WorkAssignment).ToListAsync();
            return Ok(productions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var production = await _context.DailyProductions.Include(dp => dp.WorkAssignment).FirstOrDefaultAsync(dp => dp.Id == id);
            if (production == null) return NotFound();
            return Ok(production);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DailyProduction production)
        {
            _context.DailyProductions.Add(production);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = production.Id }, production);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DailyProduction production)
        {
            if (id != production.Id) return BadRequest();
            _context.Entry(production).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var production = await _context.DailyProductions.FindAsync(id);
            if (production == null) return NotFound();
            _context.DailyProductions.Remove(production);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 