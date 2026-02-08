using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2026_peminjaman_ruangan_backend.Models;

[Table("rooms")]
public class Room
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nama_ruangan")]
    public string NamaRuangan { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("lokasi")]
    public string? Lokasi { get; set; }

    [Column("kapasitas")]
    public int Kapasitas { get; set; }

    [MaxLength(500)]
    [Column("deskripsi")]
    public string? Deskripsi { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
