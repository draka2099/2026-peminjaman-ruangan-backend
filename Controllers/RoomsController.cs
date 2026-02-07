using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Models;

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

    // POST: api/rooms
    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    // PUT: api/rooms/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, Room room)
    {
        if (id != room.Id)
        {
            return BadRequest();
        }

        _context.Entry(room).State = EntityState.Modified;

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
