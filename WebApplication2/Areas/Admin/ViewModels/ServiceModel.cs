using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.Admin.ViewModels;


public class ServiceModel
{

	public int Id { get; set; }
	[Required]
	[MaxLength(100)]
	public string Name { get; set; } 

	[MaxLength(200), Required]
	public string Description { get; set; }

	public IFormFile? Image { get; set; } 

}
