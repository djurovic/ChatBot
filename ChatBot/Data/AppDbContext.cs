using ChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MicroserviceCatalog> MicroserviceCatalogs { get; set; }
        public DbSet<MicroserviceMethod> MicroserviceMethods { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MicroserviceCatalog>()
                .HasMany(c => c.Methods)
                .WithOne(m => m.MicroserviceCatalog)
                .HasForeignKey(m => m.MicroserviceCatalogId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MicroserviceMethod>()
                .HasOne(m => m.MicroserviceCatalog)
                .WithMany(c => c.Methods)
                .HasForeignKey(m => m.MicroserviceCatalogId)
                .IsRequired();

            modelBuilder.Entity<ChatMessage>()
                .HasKey(cm => cm.Id);

            modelBuilder.Entity<ChatMessage>()
                .Property(cm => cm.Timestamp)
                .HasDefaultValueSql("GETDATE()");
        }

        
    }
}
