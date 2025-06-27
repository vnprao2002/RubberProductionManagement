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
    public class PriceTablesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public PriceTablesController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "PriceTable",
                recordId: price.Id.ToString(),
                changes: JsonConvert.SerializeObject(price),
                description: "Thêm mới bảng giá"
            );
            return CreatedAtAction(nameof(Get), new { id = price.Id }, price);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PriceTable price)
        {
            if (id != price.Id) return BadRequest();
            _context.Entry(price).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "PriceTable",
                recordId: price.Id.ToString(),
                changes: JsonConvert.SerializeObject(price),
                description: "Cập nhật bảng giá"
            );
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var price = await _context.PriceTables.FindAsync(id);
            if (price == null) return NotFound();
            _context.PriceTables.Remove(price);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "PriceTable",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(price),
                description: "Xóa bảng giá"
            );
            return NoContent();
        }
    }
} 