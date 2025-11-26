using System;

namespace InventoryManagementSoftwareDemo.Models
{
	public class UserDetails
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public string ProfilePic { get; set; } = null;
		public DateTime CreatedDate { get; set; } = System.DateTime.Now;
		public DateTime UpdatedDate { get; set; }
	}
}
