using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.Count = sliders.Count;
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Create(Slider slider)
            
        {
            if (!ModelState.IsValid)
                return View();
                    


            if (slider.Offer > 100)
            {
                ModelState.AddModelError("Offer", "offer 100-den cox ola bilmez");

                return View();
            }
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detail(int id)
        {
            if (_context.Sliders.Count() == 1)
                return BadRequest();

          Slider? slider =  _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (slider is null)
                return NotFound();

            return View(slider);
        }
        public IActionResult Delete(int id)
        {
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
			if (slider is null)
				return NotFound();

			return View(slider);

		}
        [HttpPost]
		[ActionName("Delete")]
		public IActionResult DeleteSlider(int id)
        {
			Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
			if (slider is null)
				return NotFound();

            _context.Sliders.Remove(slider);
            _context.SaveChanges();



			return RedirectToAction(nameof(Index));
        }

        public IActionResult Update( int id)
        {
			Slider? slider = _context.Sliders.AsNoTracking().FirstOrDefault(s => s.Id == id);
			if (slider is null)
				return NotFound();


			return View(slider);
        }
        [HttpPost]

        public IActionResult Update(Slider slider, int id)
        {

			Slider? dbSlider = _context.Sliders.FirstOrDefault(s => s.Id == id);
			if (slider is null)
				return NotFound();

            dbSlider.Title = slider.Title;
            dbSlider.Description = slider.Description;
            dbSlider.Image = slider.Image;
            dbSlider.Offer = slider.Offer;


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

		}
	}
}

