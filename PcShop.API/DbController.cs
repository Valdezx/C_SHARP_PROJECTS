using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcShop.Api.Data;

namespace PcShop.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class DbController : ControllerBase
{
    private readonly AppDbContext _context;

    public DbController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("reset-database")]
    public async Task<IActionResult> ResetDatabase()
    {
        await _context.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE \"Products\", \"Categories\" RESTART IDENTITY CASCADE;");

        return Ok("Базу даних очищено, лічильники ID скинуто до 1.");
    }
}