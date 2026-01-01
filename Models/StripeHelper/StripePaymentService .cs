using Stripe;
namespace InventoryManagementSoftwareDemo.Models.StripeHelper
{
	public class StripePaymentService : IPaymentService
	{
		public async Task<StripePaymentResult> CreateStripePaymentAsync(
		string productName,
		int quantity,
		decimal amount,
		string userEmail)
		{
			var options = new PaymentIntentCreateOptions
			{
				Amount = (long)(amount * 100),
				Currency = "usd",
				ReceiptEmail = userEmail,
				Description = $"{productName} x {quantity}"
			};

			var service = new PaymentIntentService();
			var intent = await service.CreateAsync(options);

			return new StripePaymentResult
			{
				PaymentIntentId = intent.Id,
				ClientSecret = intent.ClientSecret,
				Status = intent.Status
			};
		}
	}
}
