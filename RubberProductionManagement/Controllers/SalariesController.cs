using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.Data;
using RubberProductionManagement.Models;

namespace RubberProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalariesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SalariesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var salaries = await _context.Salaries.Include(s => s.Employee).Include(s => s.WorkSession).ToListAsync();
            return Ok(salaries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var salary = await _context.Salaries.Include(s => s.Employee).Include(s => s.WorkSession).FirstOrDefaultAsync(s => s.Id == id);
            if (salary == null) return NotFound();
            return Ok(salary);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Salary salary)
        {
            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = salary.Id }, salary);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Salary salary)
        {
            if (id != salary.Id) return BadRequest();
            _context.Entry(salary).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null) return NotFound();
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 