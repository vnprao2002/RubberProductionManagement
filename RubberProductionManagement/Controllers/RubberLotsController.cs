using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RubberLotsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RubberLotsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lots = await _context.RubberLots.ToListAsync();
            return Ok(lots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lot = await _context.RubberLots.FindAsync(id);
            if (lot == null) return NotFound();
            return Ok(lot);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RubberLot lot)
        {
            _context.RubberLots.Add(lot);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = lot.Id }, lot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RubberLot lot)
        {
            if (id != lot.Id) return BadRequest();
            _context.Entry(lot).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lot = await _context.RubberLots.FindAsync(id);
            if (lot == null) return NotFound();
            _context.RubberLots.Remove(lot);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 