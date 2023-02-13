using Microsoft.EntityFrameworkCore;

namespace BirdsApi.Data;

public class BirdsDbContext : DbContext
{
    public DbSet<Bird> Birds => Set<Bird>();
    public DbSet<Observation> Observations => Set<Observation>();

    public BirdsDbContext(DbContextOptions<BirdsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Observation>().Navigation(o => o.Bird).AutoInclude();

        modelBuilder.Entity<Observation>().OwnsOne(o=> o.Photo);

        // Seed the most common feeder birds in Europe
        modelBuilder.Entity<Bird>()
            .HasData(
                new { Id = 1, Name = "Common Chaffinch" },       // Vink
                new { Id = 2, Name = "Common Wood Pigeon" },     // Houtduif
                new { Id = 3, Name = "Dunnock" },                // Heggenmus
                new { Id = 4, Name = "Common Blackbird" },       // Merel
                new { Id = 5, Name = "Bullfinch" },              // Goudvink
                new { Id = 6, Name = "Eurasian Collared Dove" }, // Turkse Tortel
                new { Id = 7, Name = "Jackdaw" },                // Kauw
                new { Id = 8, Name = "Eurasian Jay" },           // Gaai
                new { Id = 9, Name = "Magpie" }                  // Ekster
            );

        base.OnModelCreating(modelBuilder);
    }
}