﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SwedbankPay.Sdk;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.ViewModels;

namespace Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerZipCode { get; set; }
        //public string CustomerEmail { get; set; }
        public string CustomerCity { get; set; }
        public int? ShippingStatusId { get; set; }
        [ForeignKey(nameof(ShippingStatusId))]
        public virtual ShippingStatus? ShippingStatus { get; set; }
    }
}
