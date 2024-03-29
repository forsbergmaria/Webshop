﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        [ForeignKey(nameof(ItemId))]
        public virtual Item Items { get; set; }
    }
}
