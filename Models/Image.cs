using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { set; get; }
        public int ItemId { set; get; }
        public string Path { set; get; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Items { get; set; }

    }
}
