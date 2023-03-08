using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class ItemHasSize
    {
        [Key]
        public int ItemId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Items { get; set; }
        [ForeignKey(nameof(SizeId))]
        public virtual Size Sizes { get; set; }
    }
}
