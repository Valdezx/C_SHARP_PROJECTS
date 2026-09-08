namespace PcShop.Api.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    int CategoryId
);

public record UpdateProductDto(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    int CategoryId
);

public record ProductResponseDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    int CategoryId,
    string? CategoryName
);
