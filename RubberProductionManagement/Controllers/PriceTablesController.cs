using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceTablesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PriceTablesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prices = await _context.PriceTables.ToListAsync();
            return Ok(prices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var price = await _context.PriceTables.FindAsync(id);
            if (price == null) return NotFound();
            return Ok(price);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PriceTable price)
        {
            _context.PriceTables.Add(price);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = price.Id }, price);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PriceTable price)
        {
            if (id != price.Id) return BadRequest();
            _context.Entry(price).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var price = await _context.PriceTables.FindAsync(id);
            if (price == null) return NotFound();
            _context.PriceTables.Remove(price);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 