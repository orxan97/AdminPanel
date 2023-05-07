using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Areas.Admin.Controllers
{

    [Area("Admin")]

      
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }
       
        

        
        public IActionResult Index()
        {

            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.Offer > 100)
                return Content("100den boyuk ola bilmez");
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}

