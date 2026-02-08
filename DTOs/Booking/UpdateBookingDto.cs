using System.ComponentModel.DataAnnotations;

namespace _2026_peminjaman_ruangan_backend.DTOs.Booking;

public class UpdateBookingDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Room ID harus lebih besar dari 0")]
    public int? RoomId { get; set; }

    public DateOnly? TanggalPeminjaman { get; set; }

    public TimeOnly? WaktuMulai { get; set; }

    public TimeOnly? WaktuSelesai { get; set; }

    [StringLength(500, MinimumLength = 10, ErrorMessage = "Keperluan harus antara 10-500 karakter")]
    public string? Keperluan { get; set; }
}
