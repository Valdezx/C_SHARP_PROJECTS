using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcShop.Api.Data;
using PcShop.Api.DTOs;
using PcShop.Api.Models;

namespace PcShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryResponseDto(c.Id, c.Name, c.Description))
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/categories/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
    {
        var category = await _context.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponseDto(c.Id, c.Name, c.Description))
            .FirstOrDefaultAsync();

        if (category == null)
            return NotFound($"Категорію з ID {id} не знайдено");

        return Ok(category);
    }

    // POST: api/categories
    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(CreateCategoryDto dto)
    {
        var trimmedName = dto.Name.Trim();

        // Перевірка на унікальність назви категорії
        var nameExists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == trimmedName.ToLower());

        if (nameExists)
            return Conflict($"Категорія '{trimmedName}' уже існує.");

        var category = new Category
        {
            Name = trimmedName,
            Description = dto.Description.Trim()
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var response = new CategoryResponseDto(category.Id, category.Name, category.Description);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, response);
    }

    // PUT: api/categories/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound($"Категорію з ID {id} не знайдено.");

        var trimmedName = dto.Name.Trim();

        // Перевірка, чи не зайнята нова назва іншою категорією
        var duplicateExists = await _context.Categories
            .AnyAsync(c => c.Id != id && c.Name.ToLower() == trimmedName.ToLower());

        if (duplicateExists)
            return Conflict($"Інша категорія з назвою '{trimmedName}' уже існує.");

        category.Name = trimmedName;
        category.Description = dto.Description.Trim();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/categories/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound($"Категорію з ID {id} не знайдено.");

        // Блокуємо видалення, якщо в категорії є товари
        var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
        {
            return BadRequest($"Неможливо видалити категорію '{category.Name}', оскільки до неї прив'язані товари.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}