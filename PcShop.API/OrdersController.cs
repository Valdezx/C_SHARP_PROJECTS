using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcShop.Api.Data;
using PcShop.Api.DTOs;
using PcShop.Api.Models;

namespace PcShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound($"Замовлення #{id} не знайдено.");

        var response = new OrderResponseDto(
            order.Id,
            order.CustomerName,
            order.CustomerEmail,
            order.CustomerPhone,
            order.OrderDate,
            order.TotalAmount,
            order.Items.Select(i => new OrderItemResponseDto(
                i.ProductId,
                i.Product?.Name ?? "Невідомий товар",
                i.Quantity,
                i.UnitPrice,
                i.UnitPrice * i.Quantity
            )).ToList()
        );

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto dto)
    {
        // Відкриваємо транзакцію для гарантії атомарності операції
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();

            // Дістаємо всі потрібні товари з бази одним запитом
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var order = new Order
            {
                CustomerName = dto.CustomerName.Trim(),
                CustomerEmail = dto.CustomerEmail.Trim(),
                CustomerPhone = dto.CustomerPhone.Trim(),
                OrderDate = DateTime.UtcNow
            };

            decimal calculatedTotal = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == itemDto.ProductId);

                if (product == null)
                    return BadRequest($"Товар з ID {itemDto.ProductId} не знайдено.");

                // Перевірка наявності на складі
                if (product.StockQuantity < itemDto.Quantity)
                {
                    return BadRequest($"Недостатньо товару '{product.Name}' на складі. Доступно: {product.StockQuantity}, запитано: {itemDto.Quantity}.");
                }

                // Списання кількості зі складу
                product.StockQuantity -= itemDto.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price // Фіксуємо реальну ціну з бази даних
                };

                calculatedTotal += orderItem.UnitPrice * orderItem.Quantity;
                order.Items.Add(orderItem);
            }

            order.TotalAmount = calculatedTotal;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Підтверджуємо транзакцію (фіксуємо зміни в базі)
            await transaction.CommitAsync();

            var response = new OrderResponseDto(
                order.Id,
                order.CustomerName,
                order.CustomerEmail,
                order.CustomerPhone,
                order.OrderDate,
                order.TotalAmount,
                order.Items.Select(i => new OrderItemResponseDto(
                    i.ProductId,
                    products.First(p => p.Id == i.ProductId).Name,
                    i.Quantity,
                    i.UnitPrice,
                    i.UnitPrice * i.Quantity
                )).ToList()
            );

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
        }
        catch (Exception)
        {
            // У разі будь-якого збою скасовуємо всі списання та зміни
            await transaction.RollbackAsync();
            return StatusCode(500, "Під час оформлення замовлення сталася внутрішня помилка.");
        }
    }
}