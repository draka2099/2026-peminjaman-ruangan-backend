using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.Models;

[Table("bookings")]
public class Booking
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("room_id")]
    public int RoomId { get; set; }

    [Required]
    [Column("tanggal_peminjaman")]
    public DateOnly TanggalPeminjaman { get; set; }

    [Required]
    [Column("waktu_mulai")]
    public TimeOnly WaktuMulai { get; set; }

    [Required]
    [Column("waktu_selesai")]
    public TimeOnly WaktuSelesai { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("keperluan")]
    public string Keperluan { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("RoomId")]
    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<BookingStatusHistory> StatusHistories { get; set; } = new List<BookingStatusHistory>();

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
