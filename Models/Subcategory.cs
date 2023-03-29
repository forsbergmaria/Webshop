using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Subcategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubcategoryId { get; set; }
        [Required(ErrorMessage = "Vänligen fyll i ett namn på underkategorin")]
        [Display(Name = "Underkategorinamn*")]
        public string Name { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Categories { get; set; }
    }
}
