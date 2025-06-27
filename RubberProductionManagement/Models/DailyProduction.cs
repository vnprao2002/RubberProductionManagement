namespace RubberProductionManagement.Models
{
    public class DailyProduction
    {
        public int Id { get; set; }
        public int WorkAssignmentId { get; set; }
        public WorkAssignment? WorkAssignment { get; set; }
        public DateTime Date { get; set; }
        public int TreesTapped { get; set; }
        public double AreaWorked { get; set; }
        public int TappingTimes { get; set; }
        public double Production { get; set; }
        public string? Note { get; set; }
        public string? Weather { get; set; }
        public string? Quality { get; set; }
        public string? Description { get; set; }
    }
} 