using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSoftwareDemo.Models
{
	public class Locations
	{
		[Key]
		public int Id { get; set; }
		public string LocationName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
