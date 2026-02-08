using System.ComponentModel.DataAnnotations;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.DTOs.User;

public class UpdateUserDto
{
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nama lengkap harus antara 3-100 karakter")]
    public string? NamaLengkap { get; set; }

    [EmailAddress(ErrorMessage = "Format email tidak valid")]
    [StringLength(100, ErrorMessage = "Email maksimal 100 karakter")]
    public string? Email { get; set; }

    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password harus antara 6-255 karakter")]
    public string? Password { get; set; }

    public UserRole? Role { get; set; }

    [StringLength(20, ErrorMessage = "NIM/NIP maksimal 20 karakter")]
    public string? NimNip { get; set; }
}
