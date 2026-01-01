namespace InventoryManagementSoftwareDemo.Models.StripeHelper
{
	public interface IPaymentService
	{
		Task<StripePaymentResult> CreateStripePaymentAsync(
		string productName,
		int quantity,
		decimal amount,
		string userEmail);
	}
}
