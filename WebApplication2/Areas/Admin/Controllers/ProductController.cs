﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebApplication2.Areas.Admin.ViewModels;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;
		public ProductController(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var product = await _context.Products.Include(p => p.Category).OrderByDescending(p => p.ModifiedAt).ToListAsync();
			return View(product);
		}
		public IActionResult Create()
		{
			ViewBag.Categories = _context.Categories.AsEnumerable();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductViewModel productViewModel)
		{

			ViewBag.Categories = _context.Categories.AsEnumerable();

			if (!ModelState.IsValid)
				return View();

			if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
				return BadRequest();


			Product product = new()
			{
				Image = productViewModel.Image,
				Name = productViewModel.Name,
				Price = productViewModel.Price,
				Rating = productViewModel.Rating,
				CategoryId = productViewModel.CategoryId,
				IsDeleted = false
			};
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();




			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Update(int id)
		{
			Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id );
			if (product is null)
				return NotFound();

			ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);


			ProductViewModel productViewModel = new()
			{
				Id = product.Id,
				Name = product.Name,
				Image = product.Image,
				Price = product.Price,
				Rating = product.Rating,
				CategoryId = product.CategoryId
			};
			return View(productViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, ProductViewModel productViewModel)
		{
			ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted);
			if (!ModelState.IsValid)
				return View();
			if (!_context.Categories.Any(c => c.Id == productViewModel.CategoryId))
				return BadRequest();

			Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product is null)
				return NotFound();

			product.Image = productViewModel.Image;
			product.Name = productViewModel.Name;
			product.Price = productViewModel.Price;
			product.Rating = productViewModel.Rating;
			product.CategoryId = productViewModel.CategoryId;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete (int id)
		{
			var foundProduct = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id ==  id );
			if (foundProduct == null) 
				return NotFound();

			return View(foundProduct);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id );
			if (product == null) return NotFound();
			product.IsDeleted = true;

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		   
		}
	}
}
