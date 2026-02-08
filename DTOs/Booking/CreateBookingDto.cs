using System.ComponentModel.DataAnnotations;

namespace _2026_peminjaman_ruangan_backend.DTOs.Booking;

public class CreateBookingDto
{
    [Required(ErrorMessage = "User ID wajib diisi")]
    [Range(1, int.MaxValue, ErrorMessage = "User ID harus lebih besar dari 0")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Room ID wajib diisi")]
    [Range(1, int.MaxValue, ErrorMessage = "Room ID harus lebih besar dari 0")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Tanggal peminjaman wajib diisi")]
    public DateOnly TanggalPeminjaman { get; set; }

    [Required(ErrorMessage = "Waktu mulai wajib diisi")]
    public TimeOnly WaktuMulai { get; set; }

    [Required(ErrorMessage = "Waktu selesai wajib diisi")]
    public TimeOnly WaktuSelesai { get; set; }

    [Required(ErrorMessage = "Keperluan wajib diisi")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Keperluan harus antara 10-500 karakter")]
    public string Keperluan { get; set; } = string.Empty;
}
