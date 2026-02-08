using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Models;
using _2026_peminjaman_ruangan_backend.DTOs.Booking;
using _2026_peminjaman_ruangan_backend.Models.Enums;

namespace _2026_peminjaman_ruangan_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        // Query Filter di DbContext otomatis menyembunyikan yang IsDeleted = true
        return await _context.Bookings
            .Include(b => b.Room) // Tampilkan detail Ruangan
            .Include(b => b.User) // Tampilkan detail Peminjam
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    // GET: api/bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        return booking;
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(CreateBookingDto dto)
    {
        // 1. Validasi Waktu
        if (dto.WaktuMulai >= dto.WaktuSelesai)
        {
            return BadRequest(new { message = "Waktu mulai harus lebih awal dari waktu selesai." });
        }

        // 2. Cek Ketersediaan Ruangan (Availability Check)
        if (!await IsRoomAvailable(dto.RoomId, dto.TanggalPeminjaman, dto.WaktuMulai, dto.WaktuSelesai))
        {
            return BadRequest(new { message = "Ruangan tidak tersedia pada tanggal dan jam tersebut." });
        }

        // 3. Mapping DTO ke Model
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

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    // PUT: api/bookings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(int id, UpdateBookingDto dto)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        // Siapkan data untuk pengecekan
        var checkRoomId = dto.RoomId ?? booking.RoomId;
        var checkDate = dto.TanggalPeminjaman ?? booking.TanggalPeminjaman;
        var checkStart = dto.WaktuMulai ?? booking.WaktuMulai;
        var checkEnd = dto.WaktuSelesai ?? booking.WaktuSelesai;

        // Validasi Waktu
        if (checkStart >= checkEnd)
        {
            return BadRequest(new { message = "Waktu mulai harus lebih awal dari waktu selesai." });
        }

        // Cek Bentrok Jadwal (Kecuali booking ini sendiri)
        if (!await IsRoomAvailable(checkRoomId, checkDate, checkStart, checkEnd, excludeBookingId: id))
        {
            return BadRequest(new { message = "Jadwal bentrok dengan peminjaman lain." });
        }

        // Update Data
        if (dto.RoomId.HasValue) booking.RoomId = dto.RoomId.Value;
        if (dto.TanggalPeminjaman.HasValue) booking.TanggalPeminjaman = dto.TanggalPeminjaman.Value;
        if (dto.WaktuMulai.HasValue) booking.WaktuMulai = dto.WaktuMulai.Value;
        if (dto.WaktuSelesai.HasValue) booking.WaktuSelesai = dto.WaktuSelesai.Value;
        if (!string.IsNullOrWhiteSpace(dto.Keperluan)) booking.Keperluan = dto.Keperluan;

        booking.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    // DELETE: api/bookings/5 (SOFT DELETE IMPLEMENTATION)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        // --- LOGIKA SOFT DELETE ---
        // Alih-alih menghapus permanen, kita tandai sebagai terhapus
        booking.IsDeleted = true;
        booking.DeletedAt = DateTime.UtcNow;
        
        // Opsional: Batalkan statusnya juga
        booking.Status = BookingStatus.Cancelled;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper Method: Cek apakah ruangan kosong
    private async Task<bool> IsRoomAvailable(int roomId, DateOnly date, TimeOnly start, TimeOnly end, int? excludeBookingId = null)
    {
        var query = _context.Bookings.AsQueryable();

        // Ambil booking di ruangan & tanggal yang sama
        // Note: Booking yang IsDeleted=true otomatis terfilter oleh Global Query Filter di DbContext
        query = query.Where(b => 
            b.RoomId == roomId && 
            b.TanggalPeminjaman == date &&
            b.Status != BookingStatus.Rejected && 
            b.Status != BookingStatus.Cancelled);

        // Jika update, jangan cek diri sendiri
        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        // Logika Tabrakan Waktu (Overlap)
        return !await query.AnyAsync(b => start < b.WaktuSelesai && b.WaktuMulai < end);
    }

    private bool BookingExists(int id)
    {
        return _context.Bookings.Any(e => e.Id == id);
    }
}