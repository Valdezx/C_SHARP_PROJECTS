using Microsoft.AspNetCore.Identity;

namespace PcShop.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}