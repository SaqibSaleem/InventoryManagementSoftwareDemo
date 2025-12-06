using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSoftwareDemo.Models
{
	public class Products
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public int?	Quantity { get; set; }
		public float? Price { get; set; }
		public string? ItemPicture { get; set; }
		public string? AddedBy { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		[NotMapped]
		public IFormFile ItemPictureFile { get; set; }
		
	}
	
	public class ProductList
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int? Quantity { get; set; }
		public float? Price { get; set; }
		public string? ItemPicture { get; set; }
		public string? AddedBy { get; set; }
	}
}
