using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			return View();
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
	}
}
