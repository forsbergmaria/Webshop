using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public bool HasSize { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
