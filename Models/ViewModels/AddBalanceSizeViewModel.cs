using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AddBalanceSizeViewModel
    {
        public Item Item { get; set; }

        public Dictionary<Size, int>? CurrentBalance { get; set; }
        [Display(Name = "Öka lagersaldo med:*")]
        [Required(ErrorMessage = "Vänligen ange en siffra")]
        public Dictionary<Size, int> AddToBalance { get; set; }
        //public Dictionary<Size, int>? RemoveFromBalanceSizes { get; set; }
        public string TransactionType { get; set; }
    }
}
