namespace LogisticApp.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxDeliveriesPerDay { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
