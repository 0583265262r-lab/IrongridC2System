using IronGridC2Api.Models;
using Microsoft.EntityFrameworkCore;

namespace IronGridC2Api.Data
{

    public class IronGridDbContext : DbContext
    {
        public IronGridDbContext(DbContextOptions<IronGridDbContext> options)
            : base(options)
        {
        }
        public DbSet<AssetLiveStatus> AssetLiveStatus => Set<AssetLiveStatus>();
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Unit> Units => Set<Unit>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>()
                       .HasKey(u => u.Id);
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Unit)
                    .WithMany(x => x.Assets)
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<AssetLiveStatus>(entity =>
            {
                entity.ToTable("AssetLiveStatus");
                entity.HasKey(x => x.AssetId);
                entity.HasOne(x => x.Asset)
                    .WithOne(x => x.LiveStatus)
                    .HasForeignKey<AssetLiveStatus>(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }
    }
}
