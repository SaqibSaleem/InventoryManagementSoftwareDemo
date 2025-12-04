using InventoryManagementSoftwareDemo.Areas.Identity.Data;
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
	}
}
