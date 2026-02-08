using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.Models;

[Table("booking_status_histories")]
public class BookingStatusHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Required]
    [Column("status_sebelumnya")]
    public BookingStatus StatusSebelumnya { get; set; }

    [Required]
    [Column("status_baru")]
    public BookingStatus StatusBaru { get; set; }

    [Required]
    [Column("changed_by_user_id")]
    public int ChangedByUserId { get; set; }

    [Column("changed_at")]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    [Column("catatan")]
    public string? Catatan { get; set; }

    // Navigation properties
    [ForeignKey("BookingId")]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey("ChangedByUserId")]
    public virtual User ChangedByUser { get; set; } = null!;
}
