using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SmartChargingContext : DbContext
    {
        public SmartChargingContext(DbContextOptions<SmartChargingContext> options) : base(options)
        {
        }
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<ChargeStation> ChargeStations => Set<ChargeStation>();
        public DbSet<Connector> Connectors => Set<Connector>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<ChargeStation>().ToTable("ChargeStation");
            modelBuilder.Entity<Connector>().ToTable("Connector");

            modelBuilder.Entity<ChargeStation>().HasOne(s=>s.Group).WithMany(g=>g.ChargeStations)
    .HasForeignKey(s => s.GroupId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Connector>().HasOne(c=>c.ChargeStation).WithMany(s=>s.Connectors)
     .HasForeignKey(c => c.ChargeStationId)
     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Connector>()
                .HasKey(c => new
                {
                    c.Id,
                    c.ChargeStationId
                });

            modelBuilder.Entity<ChargeStation>()
              .HasKey(s => s.Id);

            modelBuilder.Entity<Group>()
              .HasKey(g => g.Id);
        }
    }
}