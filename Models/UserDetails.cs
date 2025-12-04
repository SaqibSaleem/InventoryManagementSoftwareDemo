using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InventoryManagementSoftwareDemo.Models
{
	public class UserDetails
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		[AllowNull]
		public string PhoneNumber { get; set; }
		[AllowNull]
		public string ProfilePic { get; set; } = null;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedDate { get; set; } = System.DateTime.Now;
		public DateTime UpdatedDate { get; set; }
		[NotMapped]
		public IFormFile ProfileImage { get; set; }
	}
}
