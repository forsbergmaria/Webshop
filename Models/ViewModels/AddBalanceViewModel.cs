using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.ViewModels
{
    public class AddBalanceViewModel
    {
        public Item item { get; set; }
        public int? CurrentBalance { get; set; }
        [Display(Name = "Öka lagersaldo med:*")]
        [Required(ErrorMessage = "Vänligen ange en siffra")]
        public int AddToBalance { get; set; }
        //public int RemoveFromBalanceSizes { get; set; }
        public string TransactionType { get; set; }
    }
}
