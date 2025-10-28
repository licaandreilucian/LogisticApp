using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;


namespace LogisticApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Driver> Drivers => Set<Driver>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // Driver configuration
            // -------------------------
            modelBuilder.Entity<Driver>(builder =>
            {
                builder.ToTable("Drivers");

                builder.HasKey(d => d.Id);

                builder.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(d => d.MaxDeliveriesPerDay)
                    .HasColumnType("int")
                    .IsRequired()
                    .HasDefaultValue(1);

                builder.HasMany(d => d.Orders)
                    .WithOne(o => o.Driver)
                    .HasForeignKey(o => o.DriverId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // -------------------------
            // Order configuration
            // -------------------------
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders");

                builder.HasKey(o => o.Id);

                builder.Property(o => o.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(o => o.DestinationCity)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(o => o.Weight)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
