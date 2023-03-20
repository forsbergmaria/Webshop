using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Models
{
	public class Admin : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public bool HasFullAccess { get; set; }
	}
}
