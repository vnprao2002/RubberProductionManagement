namespace RubberProductionManagement.Models
{
    public class RubberLot
    {
        public int Id { get; set; }
        public string LotCode { get; set; } = string.Empty;
        public string LotName { get; set; } = string.Empty;
        public double Area { get; set; }
        public string? Description { get; set; }
        public ICollection<WorkAssignment>? WorkAssignments { get; set; }
    }
} 