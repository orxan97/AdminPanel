using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication2.Areas.Admin.ViewModels;
using WebApplication2.Utils;
using WebApplication2.Utils.Enum;
using WebApplication2.Utilsı;

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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ServiceModel serviceModel)

		{
			//return Content(serviceModel.Image.ContentType);

			if (_count == 3)
				return BadRequest();

			if (!ModelState.IsValid) return View();
			


			if (serviceModel.Image == null)
			{
				ModelState.AddModelError("Image", "image bosh ola  bilmez");
				return View();
			}
			if (!serviceModel.Image.CheckFileSize(100))
			{
				ModelState.AddModelError("Image","fayylin hecmi 100kb dan kicik olmalidir");
				return View();
			}
			if (!serviceModel.Image.CheckFileType(ContentType.image.ToString()))
			{
				ModelState.AddModelError("Image","faylin tipi image olmalidir");
				return View();
			}

		    bool isExists = await _appDbContext.Services.AnyAsync(s => s.Name == serviceModel.Name);

			if (isExists)
			{
				ModelState.AddModelError("Name", "Hal hazirda bu adda bir servis movcuddur");
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



		
		public async Task<IActionResult> Delete(int id)
		{
			Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
			if (service is null)
				return NotFound();


			return View(service);
		}
		[HttpPost]
		[ActionName(nameof(Delete))]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteService(int id)
		{
			Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
			if (service is null)
				return NotFound();

			//string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",
			//	service.Image);
			//if (System.IO.File.Exists(path))
			//{
			//	System.IO.File.Delete(path);
			//}

			FileService.DeleteFiled(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",
			service.Image);


			_appDbContext.Services.Remove(service);
			await _appDbContext.SaveChangesAsync();		

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			Service? service = _appDbContext.Services.FirstOrDefault(s => s.Id == id);
			if (service is null)
				return NotFound();
			ServiceModel serviceModel = new()
			{
				Id = service.Id,
				Name = service.Name,
				Description = service.Description
			};


			return View(serviceModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, ServiceModel serviceModel)
		{

			if (!ModelState.IsValid)
				return View();


			Service? service = await _appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
			if (service is null)
				return NotFound();

			if (serviceModel.Image !=null)
			{

				if (!serviceModel.Image.CheckFileSize(100))
				{
					ModelState.AddModelError("Image", "fayylin hecmi 100kb dan kicik olmalidir");
					return View();
				}
				if (!serviceModel.Image.CheckFileType(ContentType.image.ToString()))
				{
					ModelState.AddModelError("Image", "faylin tipi image olmalidir");
					return View();
				}
				var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",service.Image);
				FileService.DeleteFiled(path);
				string fileName = $"{Guid.NewGuid()}-{serviceModel.Image.FileName}";
				var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);
				using (FileStream stream = new FileStream(newPath, FileMode.Create))
				{
					await serviceModel.Image.CopyToAsync(stream);
				}
				service.Image = fileName;
			}

			service.Name = serviceModel.Name;
			service.Description = serviceModel.Description;
		
			//Service newservice = new()
			//{
			//	Id = serviceModel.Id,
			//	Name = serviceModel.Name,
			//	Description = serviceModel.Description
			//};
			////Service.Name = service.Name;
			////Service.Description = service.Description;
			////Service.Image = service.Image;




			//_appDbContext.Services.Update(newservice);
			await _appDbContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}

		

	}
}
