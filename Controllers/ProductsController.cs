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
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


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
				if (products.ItemPictureFile != null && products.ItemPictureFile.Length > 0)
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
				var Result = _dbContext.Products.FromSqlRaw("SP_Add_Products @p0,@p1,@p2,@p3,@p4,@p5", parameters: new object[] { products.Name, products.Quantity, products.Price, products.ItemPicture, products.AddedBy, products.CreatedDate }).ToList();
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
				if (product != null)
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
				if (Result != null)
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
		[HttpPost]
		public IActionResult UpdateProduct(Products products)
		{
			try
			{
				var productData = _dbContext.Products.Where(x => x.Id == products.Id).FirstOrDefault();
				if (productData != null)
				{
					if (products.ItemPictureFile != null && products.ItemPictureFile.Length > 0)
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
						products.ItemPicture = productData.ItemPicture;
					}
					productData.UpdatedBy = _userManager.GetUserName(User);
					var Result = _dbContext.Products.FromSqlRaw("SP_Update_Products @p0,@p1,@p2,@p3,@p4,@p5", parameters: new object[] { products.Id, products.Name, products.Quantity, products.Price, products.ItemPicture, productData.UpdatedBy }).ToList();

					return RedirectToAction("Index");
				}
				else
				{
					return View("Product Not Updated");
				}
			}
			catch (Exception ex)
			{
				return View(ex.ToString());
			}
		}

		// Export Products to Excel
		[HttpPost]
		public async Task<IActionResult> ExportProductsToExcel()
		{
			try
			{
				// Get Products List from database
				var productsList = _dbContext.ProductList.FromSqlRaw("SP_GET_Products").ToList();
				if (productsList != null && productsList.Count > 0)
				{
					using (var workbook = new XLWorkbook())
					{
						// Create a new Excel workbook
						var workSheet = workbook.Worksheets.Add("Products");

						// Create HeaderRow
						var headerRow = workSheet.Row(1);
						headerRow.Style.Font.Bold = true;
						headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

						// Add Header Columns
						workSheet.Cell(1, 1).Value = "Sr No.";
						//workSheet.Cell(1, 2).Value = "Picture";
						workSheet.Cell(1, 2).Value = "Name";
						workSheet.Cell(1, 3).Value = "Quantity";
						workSheet.Cell(1, 4).Value = "Price";
						workSheet.Cell(1, 5).Value = "Added By";

						// Fill Rows with Data
						int rowNumber = 2;
						int serialNumber = 1;
						foreach (var product in productsList)
						{
							workSheet.Cell(rowNumber, 1).Value = serialNumber;
							//workSheet.Cell(rowNumber, 2).Value = product.ItemPicture;
							workSheet.Cell(rowNumber, 2).Value = product.Name;
							workSheet.Cell(rowNumber, 3).Value = product.Quantity;
							workSheet.Cell(rowNumber, 4).Value = product.Price;
							workSheet.Cell(rowNumber, 5).Value = product.AddedBy;
							rowNumber++;
							serialNumber++;
						}

						// Sizing the Columns
						workSheet.Columns().AdjustToContents();
						// Add border to cells
						var range = workSheet.Range(1, 1, rowNumber - 1, 5);
						range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
						range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

						// Set FileName
						var fileName = $"Products_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
						var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

						// Save the workbook to a MemoryStream
						using (var steam = new MemoryStream())
						{
							workbook.SaveAs(steam);
							var content = steam.ToArray();
							return File(content, contentType, fileName);
						}
					}

				}
				else
				{
					TempData["ErrorMessage"] = "Products not found.";
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Error exporting products. Please try again.";
				return RedirectToAction("Index");
			}

		}
	}
}
