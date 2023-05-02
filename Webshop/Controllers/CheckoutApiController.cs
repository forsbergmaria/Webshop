using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Newtonsoft.Json;
using NuGet.Protocol;
using Stripe;
using Stripe.Checkout;

namespace Webshop.Controllers
{
	//[Route("create-checkout-session")]
	//[ApiController]
	public class CheckoutApiController : Controller
	{
		private readonly ShoppingCartManager _cm;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CheckoutApiController(ShoppingCartManager cm, IHttpContextAccessor httpContextAccessor)
		{
			_cm = cm;
			_httpContextAccessor = httpContextAccessor;
		}

		//[HttpPost]
		public ActionResult Create()
		{
			var cart = _cm.GetCartItems();

			StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

            var lineItem = cart.Items.Select(item => new SessionLineItemOptions
            {
                Price = item.StripePriceId,
                Quantity = cart.ItemQuantity?.GetValueOrDefault(item.ItemId) ?? 0
            }).ToList();

            // Skapa en ny checkout session med Stripe API
            var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
					"card",
					"klarna",
				},

				LineItems = lineItem,

				Mode = "payment",
				SuccessUrl = "https://www.example.com/success",
				CancelUrl = "https://www.example.com/cancel",
			};


			var service = new SessionService();
			var session = service.Create(options);

			// Dirigera användaren till checkout sidan
			return Redirect(session.Url);
		}
	}
}
