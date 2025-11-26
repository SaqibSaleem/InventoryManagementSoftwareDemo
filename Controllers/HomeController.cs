using System.Diagnostics;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using InventoryManagementSoftwareDemo.Areas.Identity.Data;

namespace InventoryManagementSoftwareDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;


		public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
			ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
		}
        public IActionResult Index()
        {
            try
            {
				var UserDetails = _dbContext.UserDetails.ToList();
				return View(UserDetails);
			}
            catch (Exception ex)
            {

                return View(ex);
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult MainPage()
        {
            return View();
        }
    }
}
