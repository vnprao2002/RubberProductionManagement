namespace RubberProductionManagement.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int WorkSessionId { get; set; }
        public WorkSession? WorkSession { get; set; }
        public double TotalProduction { get; set; }
        public double TotalArea { get; set; }
        public double Yield { get; set; }
        public double Price { get; set; }
        public double TotalSalary { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 