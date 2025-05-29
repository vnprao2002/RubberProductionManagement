namespace RubberProductionManagement.API.Models
{
    public class PriceTable
    {
        public int Id { get; set; }
        public double MinYield { get; set; }
        public double MaxYield { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 