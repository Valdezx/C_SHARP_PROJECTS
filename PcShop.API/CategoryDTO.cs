using System.ComponentModel.DataAnnotations;

namespace PcShop.Api.DTOs;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Назва категорії є обов'язковою.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Назва категорії повинна мати від 2 до 50 символів.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "Опис не повинен перевищувати 250 символів.")]
    public string Description { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Назва категорії є обов'язковою.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Назва категорії повинна мати від 2 до 50 символів.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250, ErrorMessage = "Опис не повинен перевищувати 250 символів.")]
    public string Description { get; set; } = string.Empty;
}

public record CategoryResponseDto(
    int Id,
    string Name,
    string Description
);