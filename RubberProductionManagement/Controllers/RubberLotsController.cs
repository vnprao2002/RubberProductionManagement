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
    public class RubberLotsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public RubberLotsController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "RubberLot",
                recordId: lot.Id.ToString(),
                changes: JsonConvert.SerializeObject(lot),
                description: "Thêm mới lô cạo"
            );
            return CreatedAtAction(nameof(Get), new { id = lot.Id }, lot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RubberLot lot)
        {
            if (id != lot.Id) return BadRequest();
            _context.Entry(lot).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "RubberLot",
                recordId: lot.Id.ToString(),
                changes: JsonConvert.SerializeObject(lot),
                description: "Cập nhật lô cạo"
            );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lot = await _context.RubberLots.FindAsync(id);
            if (lot == null) return NotFound();
            _context.RubberLots.Remove(lot);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "RubberLot",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(lot),
                description: "Xóa lô cạo"
            );
            return NoContent();
        }
    }
} 