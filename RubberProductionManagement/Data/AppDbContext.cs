using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RubberProductionManagement.API.Models;

namespace RubberProductionManagement.API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RubberLot> RubberLots { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<WorkAssignment> WorkAssignments { get; set; }
        public DbSet<DailyProduction> DailyProductions { get; set; }
        public DbSet<PriceTable> PriceTables { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
} 