using Microsoft.EntityFrameworkCore;
using _2026_peminjaman_ruangan_backend.Data;
using _2026_peminjaman_ruangan_backend.Seeders; // Tambahkan namespace ini
using DotNetEnv; // Pastikan DotNetEnv terinstall (jika pakai .env)
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// 1. Load Environment Variables (jika pakai file .env)
Env.Load();

// 2. Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ini kuncinya: Abaikan siklus berulang (Circular Reference)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 3. Konfigurasi Database (Ambil dari Environment Variable atau appsettings)
// Prioritas: Environment Variable > appsettings.json
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 4. Konfigurasi Swagger (SWASHBUCKLE - INI KUNCINYA)
// Menggantikan AddOpenApi() yang tidak punya UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Konfigurasi CORS untuk Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173", "http://127.0.0.1:5173",
                "http://localhost:5174", "http://127.0.0.1:5174",
                "http://localhost:5175", "http://127.0.0.1:5175",
                "http://localhost:5176", "http://127.0.0.1:5176"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// 6. Jalankan Seeder Otomatis
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Jalankan migrasi database otomatis (opsional, biar aman)
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // Isi data awal (Seeder)
        await DatabaseSeeder.SeedAsync(context); // Pastikan method SeedAsync dipanggil dengan benar
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration or seeding.");
    }
}

// 7. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // AKTIFKAN SWAGGER UI DI SINI
    app.UseSwagger();
    app.UseSwaggerUI(); // Ini yang memunculkan halaman HTML
}

// Aktifkan CORS - HARUS sebelum UseAuthorization
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();