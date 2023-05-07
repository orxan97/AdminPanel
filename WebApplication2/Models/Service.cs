using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Service
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(200), Required]
    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;
}
