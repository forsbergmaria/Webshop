using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "Vänligen ange ett produktnamn")]
        [Display(Name = "Produktnamn*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vänligen ange ett varumärke")]
        [Display(Name = "Varumärke*")]
        public string Brand { get; set; }
        [Display(Name = "Artikelnummer")]
        public string? ArticleNr { get; set; }
        [Display(Name = "Denna produkt har storlekar")]
        public bool HasSize { get; set; }
        [Display(Name = "Färg")]
        public string? Color { get; set; }
        [Precision(18, 2)]
        [Required(ErrorMessage = "Vänligen ange ett pris")]
        [Display(Name = "Pris utan moms*")]
        [RegularExpression(@"^(?!0\d)(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$",
            ErrorMessage = "Ogiltigt format. Vänligen ange ett heltal eller decimaltal")]
        public decimal PriceWithoutVAT { get; set; }
        [Precision(18, 2)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Endast siffror tillåtna. Ange ett procenttal")]
        [Required(ErrorMessage = "Vänligen ange en momssats i procent")]
        [Display(Name = "Momssats (%)*")]
        public string VAT { get; set; }
        [Display(Name = "Produktbeskrivning")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vänligen välj en kategori")]
        [Display(Name = "Produktkategori")]
        public string Category { get; set; }
        [Display(Name = "Produktbilder")]
        public ICollection<Image>? ProductImages { get; set; }
    }
}
