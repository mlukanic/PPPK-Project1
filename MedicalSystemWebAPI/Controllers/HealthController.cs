using MedicalSystemClassLibrary;
using MedicalSystemClassLibrary.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public HealthController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CheckDatabaseHealth()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                return Ok("Database connection is healthy.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database connection is unhealthy: {ex.Message}");
            }
        }
    }
}
