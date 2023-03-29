using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateSubcategory2ViewModel
    {
        [Required(ErrorMessage = "Vänligen fyll i ett namn för underkategorin")]
        [Display(Name = "Underkategorinamn*")]
        public string Name { get; set; }
    }
}
