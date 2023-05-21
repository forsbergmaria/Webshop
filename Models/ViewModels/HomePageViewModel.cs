using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class HomePageViewModel
    {
        public Item Item { get; set; }
        [AllowNull]
        public Image Image { get; set; }
    }
}
