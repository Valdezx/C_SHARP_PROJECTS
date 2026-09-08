using PcShop.WEB.Models;

namespace PcShop.WEB.Services;

public class CartService
{
    public List<CartItem> Items { get; private set; } = new();

    public event Action? OnChange;

    public void AddToCart(ProductDto product)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem != null)
        {
            if (existingItem.Quantity < product.StockQuantity)
            {
                existingItem.Quantity++;
                NotifyStateChanged();
            }
        }
        else
        {
            Items.Add(new CartItem
            {
                ProductId = product.Id,
                Title = product.Name,
                Price = product.Price,
                Quantity = 1,
                MaxStock = product.StockQuantity
            });
            NotifyStateChanged();
        }
    }

    public void RemoveFromCart(int productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                Items.Remove(item);
            }
            else if (quantity <= item.MaxStock)
            {
                item.Quantity = quantity;
            }
            NotifyStateChanged();
        }
    }

    public void ClearCart()
    {
        Items.Clear();
        NotifyStateChanged();
    }

    public int GetTotalCount() => Items.Sum(i => i.Quantity);

    public decimal GetTotalPrice() => Items.Sum(i => i.TotalPrice);

    private void NotifyStateChanged() => OnChange?.Invoke();
}