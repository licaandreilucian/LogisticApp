namespace LogisticApp.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public decimal Weight { get; set; }

        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
    }
}
