using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Areas.Admin.Controllers
{
		[Area("Admin")]
	public class ServiceController : Controller
	{

		private readonly AppDbContext _context;

		public ServiceController(AppDbContext context)
		{
			_context = context;
		}




		public IActionResult Index()
		{
			List<Service> services = _context.Services.ToList();
			ViewBag.Count = services.Count;
			return View(services);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Service service)

		{
			if (!ModelState.IsValid)
				return View();



		
			_context.Services.Add(service);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
		public IActionResult Detail(int id)
		{
			if (_context.Services.Count() == 1)
				return BadRequest();

			Service? service = _context.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			return View(service);
		}
		public IActionResult Delete(int id)
		{
			Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			return View(service);

		}
		[HttpPost]
		[ActionName("Delete")]
		public IActionResult DeleteService(int id)
		{
			Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			_context.Services.Remove(service);
			_context.SaveChanges();



			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			Service? service = _context.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();


			return View(service);
		}
		[HttpPost]

		public IActionResult Update(Service service, int id)
		{

			Service? dbService = _context.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			dbService.Name = service.Name;
			dbService.Description = service.Description;
			dbService.Image = service.Image;


			_context.SaveChanges();

			return RedirectToAction(nameof(Index));

		}
	}
}
