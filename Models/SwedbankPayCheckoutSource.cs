using SwedbankPay.Sdk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SwedbankPayCheckoutSource
    {
        public Uri AbortOperationLink { get; set; }
        public CultureInfo Culture { get; set; }
        public Uri JavascriptSource { get; set; }
        public HttpOperation UpdateOperation { get; set; }
        public bool UseAnonymousCheckout { get; set; }
        public Uri ConsumerUiScriptSource { get; set; }
    }
}
