using System.ComponentModel.DataAnnotations;

namespace _2026_peminjaman_ruangan_backend.DTOs.Room;

public class CreateRoomDto
{
    [Required(ErrorMessage = "Nama ruangan wajib diisi")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nama ruangan harus antara 3-100 karakter")]
    public string NamaRuangan { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Lokasi maksimal 200 karakter")]
    public string? Lokasi { get; set; }

    [Required(ErrorMessage = "Kapasitas wajib diisi")]
    [Range(1, 1000, ErrorMessage = "Kapasitas harus antara 1-1000")]
    public int Kapasitas { get; set; }

    [StringLength(500, ErrorMessage = "Deskripsi maksimal 500 karakter")]
    public string? Deskripsi { get; set; }

    public bool IsAvailable { get; set; } = true;
}
