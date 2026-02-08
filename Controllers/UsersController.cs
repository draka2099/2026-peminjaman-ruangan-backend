using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Models;
using _2026_peminjaman_ruangan_backend.DTOs.User;
using _2026_peminjaman_ruangan_backend.Models.Enums; // Pastikan Enum UserRole ada di sini

namespace _2026_peminjaman_ruangan_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null)
        {
            return Unauthorized(new { message = "Email tidak ditemukan" });
        }

        // Verifikasi password dengan BCrypt
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return Unauthorized(new { message = "Password salah" });
        }

        // Return user tanpa password
        return Ok(new {
            id = user.Id,
            namaLengkap = user.NamaLengkap,
            email = user.Email,
            role = user.Role,
            nimNip = user.NimNip
        });
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/users (Create)
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(CreateUserDto dto)
    {
        // Cek apakah email sudah ada
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new { message = "Email sudah terdaftar" });
        }

        // Manual mapping dari DTO ke Model
        var user = new User
        {
            NamaLengkap = dto.NamaLengkap,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Hash password
            Role = dto.Role,
            NimNip = dto.NimNip,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/users/5 (Update)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Manual mapping - update hanya field yang diberikan
        if (!string.IsNullOrWhiteSpace(dto.NamaLengkap))
            user.NamaLengkap = dto.NamaLengkap;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.Password = dto.Password; 

        if (dto.Role.HasValue)
            user.Role = dto.Role.Value;

        if (dto.NimNip != null) 
            user.NimNip = dto.NimNip;

        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}