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
        public int? ItemId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string? ArticleNr { get; set; }
        public bool HasSize { get; set; }
        public string? Color { get; set; }
        [Precision(18, 2)]
        public decimal PriceWithoutVAT { get; set; }
        [Precision(18, 2)]
        public decimal VAT { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ICollection<Image>? ProductImages { get; set; }
    }
}
