using CarRentalAdministration.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAdministration.DAL
{
    public class CarRentalDBContext : DbContext
    {
        public CarRentalDBContext() : this(new DbContextOptions<CarRentalDBContext>())
        {
        }

        public CarRentalDBContext(DbContextOptions<CarRentalDBContext> options)
            : base(options)
        {
        }
        
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Booking>()
                        .HasOne(c => c.Car)
                        .WithMany(b => b.Bookings)
                        .HasForeignKey(c => c.CarId)
                        .IsRequired();

            modelbuilder.Entity<Car>()
                        .Property(e => e.Category)
                        .HasConversion<string>();

            modelbuilder.Entity<Booking>()
                        .Property(t => t.TotalCostOfRent)
                        .HasColumnType("decimal(10,5)")
                        .HasPrecision(10,5);
        }
    }
}
