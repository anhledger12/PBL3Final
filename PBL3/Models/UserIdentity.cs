using Microsoft.AspNetCore.Identity;
using PBL3.Models.Entities;

namespace PBL3.Models
{
	public class UserIdentity:IdentityUser
	{
		public Account? account { get; set; } 
	}
}
