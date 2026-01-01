using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using InventoryManagementSoftwareDemo.Models;
using InventoryManagementSoftwareDemo.Models.StripeHelper;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace InventoryManagementSoftwareDemo.Controllers
{
	[Route("payment")]
	public class PaymentController : Controller
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly IPaymentService _paymentService;
		public PaymentController(IConfiguration configuration, ApplicationDbContext dbContext, IPaymentService paymentService)
		{
			_configuration = configuration;
			_dbContext = dbContext;
			_paymentService = paymentService;
		}
		public IActionResult Index()
		{
			try
			{
				var productsResults = _dbContext.Products.ToList();
				if (productsResults!= null && productsResults.Count>0)
				{
					return View(productsResults);
				}
				else
				{
					ViewData["ErrorMessage"] = "No products found!";
					return View(new List<Products>());
				}
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View(new List<Products>());
			}
		}
		

		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
		{
			try
			{
				int id = request.Id;
				var requestResult = await _dbContext.Products.FindAsync(id);
				if (requestResult != null)
				{
					var totalAmount = requestResult.Price * requestResult.Quantity;

					var result = await _paymentService.CreateStripePaymentAsync(
						requestResult.Name,
						(int)requestResult.Quantity,
						(decimal)totalAmount,
						"TestEmail@gmail.com"
					);
					return Ok(result);
				}
				else
				{
					return NotFound("Product not found");
				}
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

	}
}
