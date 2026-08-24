using Consumer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Consumer.Data
{

    public class IronGridDbContext : DbContext
    {
        public IronGridDbContext(DbContextOptions<IronGridDbContext> options)
            : base(options)
        {
        }
        public DbSet<AssetLiveStatus> AssetLiveStatus => Set<AssetLiveStatus>();
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Units> Units => Set<Units>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Units>()
                       .HasKey(u => u.Id);
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Units)
                    .WithMany(x => x.Assets)
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<AssetLiveStatus>(entity =>
            {
                entity.HasKey(x => x.AssetId);
                entity.HasOne(x => x.Asset)
                    .WithOne(x => x.LiveStatus)
                    .HasForeignKey<AssetLiveStatus>(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
