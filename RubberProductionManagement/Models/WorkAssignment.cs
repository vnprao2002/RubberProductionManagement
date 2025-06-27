    namespace RubberProductionManagement.Models
{
    public class WorkAssignment
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int WorkSessionId { get; set; }
        public WorkSession? WorkSession { get; set; }
        public int RubberLotId { get; set; }
        public RubberLot? RubberLot { get; set; }
        public double AssignedArea { get; set; }
        public double PlannedProduction { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int TappingTimes { get; set; }
        public string? Note { get; set; }
        public string? Description { get; set; }
        public ICollection<DailyProduction>? DailyProductions { get; set; }
    }
} 