


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

        return View(homeViewModel);
    }
}
