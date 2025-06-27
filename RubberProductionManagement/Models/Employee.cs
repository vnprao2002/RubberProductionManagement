namespace RubberProductionManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; } // STT
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public double TotalArea { get; set; } // Diện tích được giao
        public int TotalTrees { get; set; } // Số cây cạo
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; } // Đang làm việc, nghỉ việc, nghỉ phép
        public string? Description { get; set; }
        public ICollection<WorkAssignment>? WorkAssignments { get; set; }
    }
} 