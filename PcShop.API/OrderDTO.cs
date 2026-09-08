using System.ComponentModel.DataAnnotations;

namespace PcShop.Api.DTOs;

public class CreateOrderItemDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Вкажіть коректний ID товару.")]
    public int ProductId { get; set; }

    [Range(1, 100, ErrorMessage = "Кількість товару повинна бути від 1 до 100.")]
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    [Required(ErrorMessage = "Вкажіть ім'я клієнта.")]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Некоректний формат email.")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть контактний телефон.")]
    [Phone(ErrorMessage = "Некоректний номер телефону.")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "Замовлення має містити хоча б один товар.")]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public record OrderItemResponseDto(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record OrderResponseDto(
    int Id,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    DateTime OrderDate,
    decimal TotalAmount,
    List<OrderItemResponseDto> Items
);