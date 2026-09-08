using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcShop.Api.Data;
using PcShop.Api.DTOs;
using PcShop.Api.Models;

namespace PcShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    // GET: api/products?search=rtx&categoryId=1&minPrice=1000&maxPrice=50000&sortBy=price_desc
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll(
    [FromQuery] string? search,
    [FromQuery] int? categoryId,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] string? sortBy)
    {
        // 1. Починаємо формувати запит як IQueryable (SQL ще не виконується в базі)
        var query = _context.Products.AsNoTracking().AsQueryable();

        // 2. Фільтр за назвою або описом (регістронезалежний пошук для PostgreSQL)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search.Trim()}%";
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, searchPattern) ||
                EF.Functions.ILike(p.Description, searchPattern));
        }

        // 3. Фільтр за категорією
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // 4. Фільтр за діапазоном цін
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // 5. Сортування
        query = sortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_desc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name) // Сортування за назвою за замовчуванням
        };

        // 6. Проєкція у DTO та відправка фінального SQL до PostgreSQL
        var products = await query
            .Select(p => new ProductResponseDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.StockQuantity,
                p.CategoryId,
                p.Category != null ? p.Category.Name : null
            ))
            .ToListAsync();

        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.StockQuantity,
                p.CategoryId,
                p.Category != null ? p.Category.Name : null
            ))
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound($"Товар з ID {id} не знайдено");

        return Ok(product);
    }

    // POST: api/products
    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
    {
        var trimmedName = dto.Name.Trim();

        // 1. Бізнес-перевірка: дублікат назви товару в межах каталогу
        var nameExists = await _context.Products
            .AnyAsync(p => p.Name.ToLower() == trimmedName.ToLower());

        if (nameExists)
            return Conflict($"Товар із назвою '{trimmedName}' уже існує.");

        // 2. Бізнес-перевірка: існування категорії
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
            return BadRequest($"Категорії з ID {dto.CategoryId} не існує.");

        var product = new Product
        {
            Name = trimmedName,
            Description = dto.Description.Trim(),
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var response = new ProductResponseDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.CategoryId,
            category.Name
        );

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound($"Товар з ID {id} не знайдено.");

        var trimmedName = dto.Name.Trim();

        // 1. Бізнес-перевірка: чи не зайнята нова назва ІНШИМ товаром
        var duplicateExists = await _context.Products
            .AnyAsync(p => p.Id != id && p.Name.ToLower() == trimmedName.ToLower());

        if (duplicateExists)
            return Conflict($"Інший товар із назвою '{trimmedName}' уже існує.");

        // 2. Бізнес-перевірка: існування категорії
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            return BadRequest($"Категорії з ID {dto.CategoryId} не існує.");

        product.Name = trimmedName;
        product.Description = dto.Description.Trim();
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound($"Товар з ID {id} не знайдено");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}