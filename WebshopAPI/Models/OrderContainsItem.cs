using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderContainsItem
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Orders { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual Item Items { get; set; }
    }
}
