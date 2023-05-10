using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication2.Areas.Admin.ViewModels;

namespace WebApplication2.Areas.Admin.Controllers
{
		[Area("Admin")]
	public class ServiceController : Controller
	{

		private readonly AppDbContext _appDbContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private int _count = 0;
		public ServiceController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
		{
			_appDbContext = appDbContext;
			_webHostEnvironment = webHostEnvironment;
			IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();
			_count = services.Count();
		}




		public IActionResult Index()
		{
			IEnumerable<Service> services = _appDbContext.Services.AsEnumerable();

			ViewBag.Count = _count;
			return View(services);
		}

		public IActionResult Create()
		{
	
			if (_count== 3)
				return BadRequest();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ServiceModel serviceModel)

		{

			if (_count == 3)
				return BadRequest();

			if (!ModelState.IsValid) return View();
			


			if (serviceModel.Image == null)
			{
				ModelState.AddModelError("Image", "image bosh ola  bilmez");
				return View();
			}
			if (serviceModel.Image.Length / 1024 > 100)
			{
				ModelState.AddModelError("Image","fayylin hecmi 100kb dan kicik olmalidir");
				return View();
			}
			if (!serviceModel.Image.ContentType.Contains("image/"))
			{
				ModelState.AddModelError("Image","faylin tipi image olmalidir");
				return View();
			}

			//string path = _webHostEnvironment.WebRootPath + @"\Assets\images\website-images" + serviceModel.Image.FileName;
			//return Content(path);
			string fileName = $"{Guid.NewGuid()} - {serviceModel.Image.FileName}"; 
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "Assets", "images", "website-images",
			fileName);
			

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
			 await serviceModel.Image.CopyToAsync(stream);
			}
			Service service = new ()
			{
				Name = serviceModel.Name,
				Description = serviceModel.Description,
				Image =fileName
			};

		     await	_appDbContext.Services.AddAsync(service);
			await _appDbContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

				//return Content(serviceModel.Image.FileName);
			//return Content(serviceModel.Image.Length.ToString());
			//return Content(serviceModel.Image.ContentType);
			



		}
		public IActionResult Detail(int id)
		{
			if (_appDbContext.Services.Count() == 1)
				return BadRequest();

			Service? service = _appDbContext.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			return View(service);
		}
		public IActionResult Delete(int id)
		{
			Service? service = _appDbContext.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			return View(service);

		}
		[HttpPost]
		[ActionName("Delete")]
		public IActionResult DeleteService(int id)
		{
			Service? service = _appDbContext.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			_appDbContext.Services.Remove(service);
			_appDbContext.SaveChanges();



			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			Service? service = _appDbContext.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();


			return View(service);
		}
		[HttpPost]

		public IActionResult Update(Service service, int id)
		{

			Service? dbService = _appDbContext.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();

			dbService.Name = service.Name;
			dbService.Description = service.Description;
			dbService.Image = service.Image;


			_appDbContext.SaveChanges();

			return RedirectToAction(nameof(Index));

		}
	}
}
