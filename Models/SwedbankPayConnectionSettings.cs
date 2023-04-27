using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SwedbankPayConnectionSettings
    {
        public Uri ApiBaseUrl { get; set; }
        public string Token { get; set; }
        public string PayeeId { get; set; }
    }
}
