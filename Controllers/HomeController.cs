using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public IActionResult DeleteRestoreUser(int id)
        {
            var user = _dbContext.UserDetails.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
		}

        // Get User By Id
        [HttpGet]
        public object GetUserById(int id)
        {
            try
            {
                var userDetails = _dbContext.UserDetails.Where(x => x.Id == id).FirstOrDefault();
                if (userDetails != null)
                {
                    return userDetails;
                }
                else
                {
                    return ("User Not found");
                }
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        // Update user Details
        [HttpPost]
        public IActionResult UpdateUserDetails(UserDetails userDetails)
        {
            try
            {
                var userData = _dbContext.UserDetails.Where(x => x.Id == userDetails.Id).FirstOrDefault();
                if (userData != null)
                {
                    userData.PhoneNumber = userDetails.PhoneNumber;
                    userData.UpdatedDate = System.DateTime.Now;
                    _dbContext.SaveChanges();
					return RedirectToAction("Index", "Home");
				}
                else
                {
                    return View("User Not Updated");
                }
            }
            catch (Exception ex)
            {

                return View(ex);
            }
           
		

		}
	}
}
