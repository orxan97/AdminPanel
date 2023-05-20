using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers;

    public class ShopController : Controller
    {
    private readonly AppDbContext _context;

	public ShopController(AppDbContext context)
	{
		_context = context;
	}

    public async Task<IActionResult> Index(int? categoryId)
    {
        IQueryable<Product> products = _context.Products.IgnoreQueryFilters().AsQueryable();

        ViewBag.ProductsCount = await products.CountAsync();

        ShopViewModel shopViewModel = new()
        {
            Products =categoryId !=null 
            ? await products.Where(p => p.CategoryId == categoryId).ToListAsync() 
            :await products.ToListAsync(),

			Categories = await _context.Categories.Include(c => c.Products).
            Where(p => !p.IsDeleted).ToListAsync()
        };
        return View(shopViewModel);
    }


        public IActionResult Detail()
    {
        return View();
    }
    }

