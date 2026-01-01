using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSoftwareDemo.Models.StripeHelper
{
	public class StripePayment
	{
		[Key]
		public int Id { get; set; }
		public string StripePaymentId { get; set; }
		public string CustomerEmail { get; set; }
		public decimal Amount { get; set; }
		public string Currency { get; set; } = "Pound Sterling";
		public string Status { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime? PaymentDate { get; set; }
		public int? ProductId { get; set; }
		public int? ProductQuantity { get; set; }
	}
	[NotMapped]
	public class StripePaymentResult
	{
		public string PaymentIntentId { get; set; }
		public string ClientSecret { get; set; }
		public string Status { get; set; }
	}
	public class CreatePaymentRequest
	{
		public int Id { get; set; }
	}

}
