namespace LogisticApp.Models
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxDeliveriesPerDay { get; set; }
        public int OrdersCount { get; set; }    // computed field
    }
}
