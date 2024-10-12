using Microsoft.EntityFrameworkCore;
using Inventory1.Models;

namespace Inventory1.Data;

public class AppDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Location> Locations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Area)
            .WithMany(a => a.Items)
            .HasForeignKey(i => i.AreaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Area>() 
            .HasOne(a => a.Section)
            .WithMany(s => s.Areas)
            .HasForeignKey(a => a.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Section>()
            .HasOne(s => s.Location)
            .WithMany(l => l.Sections)
            .HasForeignKey(s => s.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}