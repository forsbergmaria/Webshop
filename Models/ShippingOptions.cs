using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class ShippingOptions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StripeShippingId { get; set; }
        public string ShippingName { get; set;}
        [Precision(18, 2)]
        public decimal ShippingPrice { get; set;}
    }
}
