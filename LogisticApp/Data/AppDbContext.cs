using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;


namespace LogisticApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<AssignmentResult> AssignmentResults => Set<AssignmentResult>();

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

            modelBuilder.Entity<AssignmentResult>(entity =>
            {
                entity.ToTable("AssignmentResults");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.IsAssigned)
                      .IsRequired();

                entity.Property(e => e.Notes)
                      .HasMaxLength(500);

                entity.Property(e => e.AssignedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // === Relationships ===
                // One AssignmentResult has one main Order
                entity.HasOne(e => e.Order)
                      .WithOne(o => o.AssignmentResult)
                      .HasForeignKey<AssignmentResult>(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Optional Driver (may be unassigned)
                entity.HasOne<DriverDto>(e => e.Driver)
                      .WithMany()
                      .HasForeignKey(e => e.DriverId)
                      .OnDelete(DeleteBehavior.SetNull);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
