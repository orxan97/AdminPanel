
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels;

public class LoginViewModel
{
    [Required, MaxLength(30)]
    public string? UserName { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool RememberMe { get; set; }


}
