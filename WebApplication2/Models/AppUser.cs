using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models;

public class AppUser :IdentityUser
{
	public string Fullname { get; set; }
	public bool IsActive { get; set; }
}
