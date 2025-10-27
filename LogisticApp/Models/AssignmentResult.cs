namespace LogisticApp.Models
{
    public class AssignmentResult
    {

        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int? DriverId { get; set; }
        public DriverDto? Driver { get; set; }
        public bool IsAssigned { get; set; }

        public string? Notes { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    }
}

