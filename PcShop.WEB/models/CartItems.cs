namespace PcShop.WEB.Models;

public class CartItem
{
    public int ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
    public int MaxStock { get; set; }

    public decimal TotalPrice => Price * Quantity;
}