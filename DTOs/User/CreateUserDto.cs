using System.ComponentModel.DataAnnotations;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.DTOs.User;

public class CreateUserDto
{
    [Required(ErrorMessage = "Nama lengkap wajib diisi")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nama lengkap harus antara 3-100 karakter")]
    public string NamaLengkap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email wajib diisi")]
    [EmailAddress(ErrorMessage = "Format email tidak valid")]
    [StringLength(100, ErrorMessage = "Email maksimal 100 karakter")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password wajib diisi")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password harus antara 6-255 karakter")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role wajib diisi")]
    public UserRole Role { get; set; }

    [StringLength(20, ErrorMessage = "NIM/NIP maksimal 20 karakter")]
    public string? NimNip { get; set; }
}
