using System.ComponentModel.DataAnnotations;

namespace PcShop.Api.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Вкажіть повне ім'я.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть email."), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть пароль."), MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required(ErrorMessage = "Вкажіть email."), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть пароль.")]
    public string Password { get; set; } = string.Empty;
}

public record AuthResponseDto(
    string Token,
    string Email,
    string FullName,
    List<string> Roles
);