using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Förnamn*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Efternamn*")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i en e-postadress")]
        [Display(Name = "E-postadress*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ett lösenord")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        [Display(Name = "Lösenord*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vänligen bekräfta lösenordet")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenordet*")]
        public string ConfirmPassword { get; set;}

        [Required(ErrorMessage = "Vänligen välj en administratörsroll")]
        [Display(Name = "Välj administratörsroll*")]
        public string Role { get; set; }

    }
}
