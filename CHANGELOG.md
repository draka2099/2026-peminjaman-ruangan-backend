# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **DTOs (Data Transfer Objects)** untuk validasi input dan output yang lebih aman
  - `CreateBookingDto` - DTO untuk membuat booking baru dengan validasi lengkap
  - `UpdateBookingDto` - DTO untuk update booking dengan partial update
  - `CreateUserDto` - DTO untuk registrasi user dengan validasi email dan password
  - `UpdateUserDto` - DTO untuk update profil user
  - `CreateRoomDto` - DTO untuk membuat ruangan baru dengan validasi kapasitas
  - `UpdateRoomDto` - DTO untuk update data ruangan
- **Data Annotations** untuk validasi input otomatis:
  - `[Required]` - Field wajib diisi
  - `[EmailAddress]` - Validasi format email
  - `[StringLength]` - Validasi panjang string (min dan max)
  - `[Range]` - Validasi range angka

### Changed
- **BookingsController**:
  - Refactored `PostBooking` method to use `CreateBookingDto` instead of raw `Booking` entity
  - Refactored `PutBooking` method to use `UpdateBookingDto` with partial update support
  - Added manual DTO-to-Model mapping for better control
  - Set default `Status = BookingStatus.Pending` pada pembuatan booking baru
  - Set `CreatedAt` and `UpdatedAt` timestamps automatically
- **UsersController**:
  - Refactored `PostUser` method to use `CreateUserDto` for input validation
  - Refactored `PutUser` method to use `UpdateUserDto` with partial update
  - Added manual mapping from DTO to User entity
  - Improved null-safety with conditional field updates
- **RoomsController**:
  - Refactored `PostRoom` method to use `CreateRoomDto`
  - Refactored `PutRoom` method to use `UpdateRoomDto`
  - Added validation for required fields (nama ruangan, kapasitas)

### Technical Details

#### Input Validation
Semua endpoint POST dan PUT sekarang memiliki validasi input otomatis melalui Data Annotations:

**Booking Validation:**
- `UserId` dan `RoomId` harus > 0
- `Keperluan` harus antara 10-500 karakter
- Semua field tanggal dan waktu wajib diisi

**User Validation:**
- `NamaLengkap` harus antara 3-100 karakter
- `Email` harus format email yang valid (max 100 karakter)
- `Password` harus antara 6-255 karakter
- `Role` wajib diisi (Admin, Mahasiswa, atau Dosen)
- `NimNip` opsional, maksimal 20 karakter

**Room Validation:**
- `NamaRuangan` harus antara 3-100 karakter
- `Kapasitas` harus antara 1-1000
- `Lokasi` dan `Deskripsi` opsional

#### Manual DTO Mapping
Semua controllers menggunakan manual mapping dari DTO ke Model entities:

```csharp
// Example: CreateBookingDto -> Booking
var booking = new Booking
{
    UserId = dto.UserId,
    RoomId = dto.RoomId,
    TanggalPeminjaman = dto.TanggalPeminjaman,
    WaktuMulai = dto.WaktuMulai,
    WaktuSelesai = dto.WaktuSelesai,
    Keperluan = dto.Keperluan,
    Status = BookingStatus.Pending,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow
};
```

#### Partial Updates
Update endpoints (PUT) sekarang support partial updates - hanya field yang dikirim yang akan diupdate:

```csharp
// Hanya update field yang tidak null/empty
if (!string.IsNullOrWhiteSpace(dto.Keperluan))
    booking.Keperluan = dto.Keperluan;

if (dto.RoomId.HasValue)
    booking.RoomId = dto.RoomId.Value;
```

### Security Improvements
- Input validation mencegah SQL injection dan XSS attacks
- Email validation mencegah format email yang salah
- Range validation mencegah overflow dan nilai yang tidak masuk akal
- StringLength validation mencegah buffer overflow attacks

### Notes
- Password masih disimpan dalam **plain text** (belum ada hashing - akan diimplementasikan di Task 9)
- Belum ada business logic validation (double booking check, availability check, dll)
- Belum ada uniqueness validation untuk email dan NimNip
- Error messages sudah dalam Bahasa Indonesia untuk user-friendly

---

## [0.1.0] - 2026-02-07

### Added
- Initial project setup dengan ASP.NET Core 10.0
- Entity Framework Core dengan PostgreSQL
- Models: User, Room, Booking, BookingStatusHistory
- Enums: UserRole, BookingStatus
- Database migrations (InitialCreate)
- ApplicationDbContext dengan DbSet configurations
- Database seeder dengan sample data
- Basic CRUD controllers untuk Bookings, Rooms, dan Users
- Environment configuration dengan .env support

### Features
- RESTful API endpoints untuk manajemen peminjaman ruangan
- Database schema dengan foreign key relationships
- Automatic timestamps (CreatedAt, UpdatedAt)
- Development environment dengan auto-migration dan seeding