using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Models;
using _2026_peminjaman_ruangan_backend.DTOs.Room;

namespace _2026_peminjaman_ruangan_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        return await _context.Rooms.ToListAsync();
    }

    // GET: api/rooms/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound();
        }

        return room;
    }

    // POST: api/rooms (Create)
    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(CreateRoomDto dto)
    {
        // Manual mapping dari DTO ke Model
        var room = new Room
        {
            NamaRuangan = dto.NamaRuangan,
            Lokasi = dto.Lokasi,
            Kapasitas = dto.Kapasitas,
            Deskripsi = dto.Deskripsi,
            IsAvailable = dto.IsAvailable, // Default true biasanya
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    // PUT: api/rooms/5 (Update)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, UpdateRoomDto dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        // Manual mapping - update hanya field yang diberikan
        if (!string.IsNullOrWhiteSpace(dto.NamaRuangan))
            room.NamaRuangan = dto.NamaRuangan;

        if (!string.IsNullOrWhiteSpace(dto.Lokasi))
            room.Lokasi = dto.Lokasi;

        if (dto.Kapasitas.HasValue)
            room.Kapasitas = dto.Kapasitas.Value;

        if (!string.IsNullOrWhiteSpace(dto.Deskripsi))
            room.Deskripsi = dto.Deskripsi;

        if (dto.IsAvailable.HasValue)
            room.IsAvailable = dto.IsAvailable.Value;

        room.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoomExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/rooms/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RoomExists(int id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}