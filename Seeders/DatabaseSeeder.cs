using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Models;
using _2026_peminjaman_ruangan_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace _2026_peminjaman_ruangan_backend.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Users
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User
                {
                    NamaLengkap = "Administrator",
                    Email = "admin@kampus.ac.id",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    NimNip = "198501012010011001",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    NamaLengkap = "Dr. Budi Santoso",
                    Email = "budi.santoso@kampus.ac.id",
                    Password = BCrypt.Net.BCrypt.HashPassword("dosen123"),
                    Role = UserRole.Dosen,
                    NimNip = "197801012005011001",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    NamaLengkap = "Ahmad Rizki",
                    Email = "ahmad.rizki@student.kampus.ac.id",
                    Password = BCrypt.Net.BCrypt.HashPassword("mahasiswa123"),
                    Role = UserRole.Mahasiswa,
                    NimNip = "2023001001",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    NamaLengkap = "Siti Nurhaliza",
                    Email = "siti.nurhaliza@student.kampus.ac.id",
                    Password = BCrypt.Net.BCrypt.HashPassword("mahasiswa123"),
                    Role = UserRole.Mahasiswa,
                    NimNip = "2023001002",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // Seed Rooms
        if (!await context.Rooms.AnyAsync())
        {
            var rooms = new List<Room>
            {
                new Room
                {
                    NamaRuangan = "Ruang Rapat A",
                    Lokasi = "Gedung A Lantai 2",
                    Kapasitas = 20,
                    Deskripsi = "Ruang rapat dengan fasilitas proyektor dan AC",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Room
                {
                    NamaRuangan = "Ruang Rapat B",
                    Lokasi = "Gedung A Lantai 3",
                    Kapasitas = 15,
                    Deskripsi = "Ruang rapat kecil dengan fasilitas whiteboard",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Room
                {
                    NamaRuangan = "Aula Utama",
                    Lokasi = "Gedung B Lantai 1",
                    Kapasitas = 200,
                    Deskripsi = "Aula besar untuk acara seminar dan wisuda",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Room
                {
                    NamaRuangan = "Lab Komputer 1",
                    Lokasi = "Gedung C Lantai 1",
                    Kapasitas = 40,
                    Deskripsi = "Lab komputer dengan 40 unit PC",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Room
                {
                    NamaRuangan = "Ruang Seminar",
                    Lokasi = "Gedung A Lantai 4",
                    Kapasitas = 50,
                    Deskripsi = "Ruang seminar dengan fasilitas lengkap",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await context.Rooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();
        }

        // Seed sample Bookings
        if (!await context.Bookings.AnyAsync())
        {
            var mahasiswa = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Mahasiswa);
            var dosen = await context.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Dosen);
            var ruangRapatA = await context.Rooms.FirstOrDefaultAsync(r => r.NamaRuangan == "Ruang Rapat A");
            var aula = await context.Rooms.FirstOrDefaultAsync(r => r.NamaRuangan == "Aula Utama");

            if (mahasiswa != null && dosen != null && ruangRapatA != null && aula != null)
            {
                var bookings = new List<Booking>
                {
                    new Booking
                    {
                        UserId = mahasiswa.Id,
                        RoomId = ruangRapatA.Id,
                        TanggalPeminjaman = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                        WaktuMulai = new TimeOnly(9, 0),
                        WaktuSelesai = new TimeOnly(11, 0),
                        Keperluan = "Rapat organisasi mahasiswa",
                        Status = BookingStatus.Pending,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Booking
                    {
                        UserId = dosen.Id,
                        RoomId = aula.Id,
                        TanggalPeminjaman = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                        WaktuMulai = new TimeOnly(13, 0),
                        WaktuSelesai = new TimeOnly(16, 0),
                        Keperluan = "Seminar nasional teknologi informasi",
                        Status = BookingStatus.Approved,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                await context.Bookings.AddRangeAsync(bookings);
                await context.SaveChangesAsync();
            }
        }

        Console.WriteLine("Database seeding completed successfully.");
    }
}
