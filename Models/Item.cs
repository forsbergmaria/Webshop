using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string? ArticleNr { get; set; }
        public bool HasSize { get; set; }
        public string? Color { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public bool IsPublished { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(SubcategoryId))]
        public virtual Subcategory? Subcategory { get; set; }
        public ICollection<Image>? ProductImages { get; set; }
    }
}
