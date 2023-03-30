using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AdminViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Förnamn")]
        public string? FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string? LastName { get; set; }
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
        [Display(Name = "Administratörsroll")]
        public string? Role { get; set; }
    }
}
