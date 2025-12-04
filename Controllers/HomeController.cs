using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace InventoryManagementSoftwareDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


		public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
			ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_userManager = userManager;
			_dbContext = dbContext;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
        {
            try
            {
				var UserDetails = _dbContext.UserDetails.ToList();
                if (UserDetails == null || UserDetails.Count == 0)
                {
                    return View("No User Found");
				}
                
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
					string wwwRootPath = _webHostEnvironment.WebRootPath;
					string FileName = userDetails.ProfileImage.FileName;
					//if (userData.ProfilePic!= null && userData.ProfilePic.Length > 0)
     //               {
     //                   var oldImg = Path.Combine("wwwroot/uploads", userDetails.ProfilePic);
					//	if (System.IO.File.Exists(oldImg))
					//		System.IO.File.Delete(oldImg);
					//}
                    FileName = Guid.NewGuid()+ Path.GetExtension(userDetails.ProfileImage.FileName);
                    var newPath = Path.Combine(wwwRootPath+ "/ProfilePicFolder", FileName);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        userDetails.ProfileImage.CopyTo(fileStream);
					}
					userData.PhoneNumber = userDetails.PhoneNumber;
                    userData.UpdatedDate = System.DateTime.Now;
                    userData.ProfilePic = FileName;
					//userData.ProfilePic = img.Name;
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
