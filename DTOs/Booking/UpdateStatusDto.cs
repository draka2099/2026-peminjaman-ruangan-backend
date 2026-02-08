using System.ComponentModel.DataAnnotations;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.DTOs.Booking;

public class UpdateStatusDto
{
    [Required(ErrorMessage = "Status wajib diisi")]
    public BookingStatus Status { get; set; }
}
