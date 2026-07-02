using Microsoft.EntityFrameworkCore;
using staj_mvc.Models;

namespace staj_mvc.Data;

public class StajDbContext : DbContext
{
    public StajDbContext(DbContextOptions<StajDbContext> options) : base(options)
    {
    }

    public DbSet<Kayit> Kayitlar => Set<Kayit>();
    public DbSet<SensorKayit> SensorKayitlar => Set<SensorKayit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SensorKayit>()
            .HasKey(s => new { s.CihazId, s.PaketSayisi });
    }
}
