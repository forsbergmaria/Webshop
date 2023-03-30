using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Vänligen fyll i ett kategorinamn")]
        [Display(Name = "Kategorinamn*")]
        public string Name { get; set; }

        public bool IsPublished { get; set; }
        public ICollection<Subcategory>? Subcategories { get; set;}
    }
}
