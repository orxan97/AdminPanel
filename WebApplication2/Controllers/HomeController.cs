
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
       
        List<Service> services = _context.Services.ToList();
        List<Slider> sliders = _context.Sliders.ToList();

        HomeViewModel homeViewModel = new()
        {
            Sliders = sliders,
            Services = services
        };
        HttpContext.Session.SetString("name", "Orxan");
        Response.Cookies.Append("surname", "Aslanov", new CookieOptions
        {
            MaxAge =TimeSpan.FromSeconds(30)
        });

        return View(homeViewModel);
    }
    public IActionResult Test()
    {
        var name = HttpContext.Session.GetString("name");
        var surname = Request.Cookies["surname"];
        return Content(name+ " " + surname);
    }
}
