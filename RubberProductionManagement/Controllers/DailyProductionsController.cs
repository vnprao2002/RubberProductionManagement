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
    public class DailyProductionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public DailyProductionsController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "DailyProduction",
                recordId: production.Id.ToString(),
                changes: JsonConvert.SerializeObject(production),
                description: "Thêm mới sản lượng hàng ngày"
            );
            return CreatedAtAction(nameof(Get), new { id = production.Id }, production);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DailyProduction production)
        {
            if (id != production.Id) return BadRequest();
            _context.Entry(production).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "DailyProduction",
                recordId: production.Id.ToString(),
                changes: JsonConvert.SerializeObject(production),
                description: "Cập nhật sản lượng hàng ngày"
            );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var production = await _context.DailyProductions.FindAsync(id);
            if (production == null) return NotFound();
            _context.DailyProductions.Remove(production);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "DailyProduction",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(production),
                description: "Xóa sản lượng hàng ngày"
            );
            return NoContent();
        }
    }
} 