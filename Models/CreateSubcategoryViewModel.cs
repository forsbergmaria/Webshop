using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateSubcategoryViewModel
    {
        [Required(ErrorMessage = "Vänligen välj en huvudkategori")]
        [Display(Name = "Huvudkategori*")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Vänligen fyll i ett namn för underkategorin")]
        [Display(Name = "Underkategorinamn*")]
        public string SubcategoryName { get; set; }
    }
}
