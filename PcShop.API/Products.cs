using System.ComponentModel.DataAnnotations;
using System;
namespace PcShop.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // Зовнішній ключ до категорії
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
