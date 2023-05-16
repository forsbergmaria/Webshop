using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemHasSize
    {
        public int ItemId { get; set; }
        public int SizeId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }
        [ForeignKey(nameof(SizeId))]
        public virtual Size Size { get; set; }
    }
}
