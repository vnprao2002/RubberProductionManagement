namespace RubberProductionManagement.Models
{
    public class WorkSession
    {
        public int Id { get; set; }
        public string SessionCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty; // Chuẩn bị/Đang thực hiện/Hoàn thành
        public string? Description { get; set; }
        public ICollection<WorkAssignment>? WorkAssignments { get; set; }
    }
} 