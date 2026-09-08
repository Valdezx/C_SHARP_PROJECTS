namespace PcShop.Api.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    // Фіксуємо ціну на момент замовлення, щоб зміна ціни товару в майбутньому не змінювала історію чека
    public decimal UnitPrice { get; set; }
}