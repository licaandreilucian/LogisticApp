namespace LogisticApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string DestinationCity { get; set; } = string.Empty;
        public decimal Weight { get; set; }

        // Foreign key
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
     
    }
}
