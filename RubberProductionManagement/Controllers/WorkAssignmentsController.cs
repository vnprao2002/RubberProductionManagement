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
    public class WorkAssignmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public WorkAssignmentsController(AppDbContext context, AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _context.WorkAssignments
                .Include(wa => wa.Employee)
                .Include(wa => wa.WorkSession)
                .Include(wa => wa.RubberLot)
                .ToListAsync();
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var assignment = await _context.WorkAssignments
                .Include(wa => wa.Employee)
                .Include(wa => wa.WorkSession)
                .Include(wa => wa.RubberLot)
                .FirstOrDefaultAsync(wa => wa.Id == id);
            if (assignment == null) return NotFound();
            return Ok(assignment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkAssignment assignment)
        {
            _context.WorkAssignments.Add(assignment);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Create",
                tableName: "WorkAssignment",
                recordId: assignment.Id.ToString(),
                changes: JsonConvert.SerializeObject(assignment),
                description: "Thêm mới phân công công việc"
            );
            return CreatedAtAction(nameof(Get), new { id = assignment.Id }, assignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WorkAssignment assignment)
        {
            if (id != assignment.Id) return BadRequest();
            _context.Entry(assignment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Update",
                tableName: "WorkAssignment",
                recordId: assignment.Id.ToString(),
                changes: JsonConvert.SerializeObject(assignment),
                description: "Cập nhật phân công công việc"
            );
            return NoContent();
        }
            
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var assignment = await _context.WorkAssignments.FindAsync(id);
            if (assignment == null) return NotFound();
            _context.WorkAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(
                action: "Delete",
                tableName: "WorkAssignment",
                recordId: id.ToString(),
                changes: JsonConvert.SerializeObject(assignment),
                description: "Xóa phân công công việc"
            );
            return NoContent();
        }

        // API tổng hợp diện tích, sản lượng kế hoạch, sản lượng thực tế của nhân viên trong một phiên
        [HttpGet("summary")]
        public async Task<IActionResult> GetEmployeeSessionSummary(int employeeId, int workSessionId)
        {
            // Lấy tất cả WorkAssignment của nhân viên trong phiên
            var assignments = await _context.WorkAssignments
                .Include(wa => wa.RubberLot)
                .Where(wa => wa.EmployeeId == employeeId && wa.WorkSessionId == workSessionId)
                .ToListAsync();

            // Tổng diện tích các lô
            double totalArea = assignments.Sum(wa => wa.RubberLot != null ? wa.RubberLot.Area : 0);
            // Tổng sản lượng kế hoạch
            double totalPlanned = assignments.Sum(wa => wa.PlannedProduction);

            // Lấy tất cả DailyProduction của các WorkAssignment này
            var assignmentIds = assignments.Select(wa => wa.Id).ToList();
            double totalActual = await _context.DailyProductions
                .Where(dp => assignmentIds.Contains(dp.WorkAssignmentId))
                .SumAsync(dp => dp.Production);

            return Ok(new
            {
                employeeId,
                workSessionId,
                totalArea,
                totalPlanned,
                totalActual
            });
        }
    }
} 