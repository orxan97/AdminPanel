using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace WebApplication2.Areas.Admin.ViewModels;

public class ProductViewModel
{

	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public double Price { get; set; }
	[Required]
	public int Rating { get; set; }

	public string Image { get; set; }

	public int CategoryId { get; set; }
}
