using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace InventoryManagementSoftwareDemo.Controllers
{
	[Authorize]
	public class LocationsController : Controller
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		public LocationsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) 
		{
			_dbContext = dbContext;
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			var resultLocations = new List<Locations>();
			try
			{
				resultLocations = _dbContext.Locations.OrderByDescending(x => x.Id).ToList();
				if (resultLocations!= null && resultLocations.Count>0)
				{
					return View(resultLocations);
				}
				else
				{
					ViewData["ErrorMessage"] = "No Record Found";
					return View(resultLocations);
				}
				
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.ToString();
				return View(resultLocations);
			}
		}
		// Add Locations
		[HttpPost]
		public object AddLocation(Locations locations)
		{
			try
			{
				var locationExists = _dbContext.Locations.FirstOrDefault(l => l.LocationName == locations.LocationName);
				if(locationExists != null)
				{
					return ("Index");
				}
				else
				{
					Locations objLocation = new Locations();
					objLocation.LocationName = locations.LocationName;
					objLocation.CreatedDate = DateTime.Now;
					_dbContext.Locations.Add(objLocation);
					_dbContext.SaveChanges();
					return ("Index");
				}
				
			}
			catch (Exception ex)
			{
				return ("Index");
			}
		}
		// Delete Location
		[HttpGet]
		public IActionResult DeleteLocation(int id)
		{
			try
			{
				var locationToDelete = _dbContext.Locations.FirstOrDefault(l => l.Id == id);
				if(locationToDelete != null)
				{
					_dbContext.Locations.Remove(locationToDelete);
					_dbContext.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index");
			}
		}
		// Get Location By Id
		[HttpGet]
		public object GetLocationById(int id)
		{
			try
			{
				var getById = _dbContext.Locations.Where(x => x.Id == id).FirstOrDefault();
				if (getById != null)
				{
					return (new { success = true, message = "ok", data = getById });
				}
				else
				{
					return (new { success = false, message = "No item found!" });
				}
			}
			catch (Exception ex)
			{
				return (new { successerror = false, message = ex.ToString() });
				
			}
		}
		[HttpPost]
		public object updateLocation(Locations locations)
		{
			try
			{
				var getLocation = _dbContext.Locations.Where(x => x.Id == locations.Id).FirstOrDefault();
				if (getLocation!=null)
				{
					getLocation.LocationName = locations.LocationName;
					getLocation.UpdatedDate = System.DateTime.Now;
					_dbContext.SaveChanges();
					return (new { success = true, message = "Data updated successfully!" });
				}
				else
				{
					return (new { success = false, message = "Location not found!" });
				}
			}
			catch (Exception ex)
			{
				return (new { success = false, message = ex.ToString() });
			}
		}

	}
}
