using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Models;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.NimNip).IsUnique();
            
            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        // Configure Room entity
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.NamaRuangan);
        });

        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.RoomId, e.TanggalPeminjaman, e.WaktuMulai, e.WaktuSelesai });
        });

        // Configure BookingStatusHistory entity
        modelBuilder.Entity<BookingStatusHistory>(entity =>
        {
            entity.Property(e => e.StatusSebelumnya)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.StatusBaru)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasOne(e => e.Booking)
                .WithMany(b => b.StatusHistories)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChangedByUser)
                .WithMany(u => u.StatusChanges)
                .HasForeignKey(e => e.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
