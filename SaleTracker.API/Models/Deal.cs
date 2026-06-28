namespace SaleTracker.API.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
        public DateTime FoundAt { get; set; } = DateTime.UtcNow;
    }
}
