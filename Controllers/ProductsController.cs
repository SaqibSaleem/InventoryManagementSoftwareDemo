using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Migrations;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;


namespace InventoryManagementSoftwareDemo.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		// Injecting the required services
		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<ApplicationUser> _userManager;

		public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment,
			UserManager<ApplicationUser> userManager)
		{
			_dbContext = dbContext;
			_webHostEnvironment = webHostEnvironment;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			try
			{
				List<ProductList> products = new List<ProductList>();
				products = _dbContext.ProductList.FromSqlRaw("SP_GET_Products").ToList();
				if (products != null || products.Count() > 0)
				{
					return View(products);
				}
				else
				{
					ViewData["ErrorMessage"] = "No Product Found";
					return View(new List<Products>());
				}
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View(new List<Products>());
			}
			
		}
		// Add Product
		[HttpPost]
		public IActionResult AddProduct(Products products)
		{
			try
			{
				if (products.ItemPictureFile!= null && products.ItemPictureFile.Length>0)
				{
					string wwwRootPath = _webHostEnvironment.WebRootPath;
					string FileName = products.ItemPictureFile.FileName;
					FileName = Guid.NewGuid() + Path.GetExtension(products.ItemPictureFile.FileName);
					var newPath = Path.Combine(wwwRootPath + "/ProductsImages", FileName);
					using (var fileStream = new FileStream(newPath, FileMode.Create))
					{
						products.ItemPictureFile.CopyTo(fileStream);
					}
					products.ItemPicture = FileName;
				}
				else
				{
					products.ItemPicture = "";
				}
				products.CreatedDate = DateTime.Now;
				products.AddedBy = _userManager.GetUserName(User);
				var Result = _dbContext.Products.FromSqlRaw("SP_Add_Products @p0,@p1,@p2,@p3,@p4,@p5", parameters: new object[] { products.Name,products.Quantity,products.Price,products.ItemPicture, products.AddedBy, products.CreatedDate }).ToList();
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return View(ex.ToString());
			}
		}

		// Get Product By Id
		[HttpGet]
		public object GetProductById(int id)
		{
			try
			{
				var product = _dbContext.Products.Where(x => x.Id == id).FirstOrDefault();
				if (product!=null)
				{
					return product;
				}
				else
				{
					return NotFound(new { error = $"Product with ID {id} not found" });
				}
					
			}
			catch (Exception ex)
			{

				return StatusCode(500, ex.ToString());
			}
		}

		public IActionResult DeleteProduct(int id)
		{
			try
			{
				var Result = _dbContext.Products.Where(x => x.Id == id).FirstOrDefault();
				if (Result!=null)
				{
					_dbContext.Products.Remove(Result);
					_dbContext.SaveChanges();
				}
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{

				return View(ex.ToString());
			}
		}


	}
}
