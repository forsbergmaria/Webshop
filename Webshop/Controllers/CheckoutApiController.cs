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

        public CheckoutApiController(ShoppingCartManager cm)
        {
            _cm = cm;
        }

        //[HttpPost]
        public ActionResult Create()
        {
            var cart = _cm.GetCartItems();

            StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

            var lineItem = cart.Items.DistinctBy(i => i.ItemId).Select(item => new SessionLineItemOptions
            {
                Price = item.StripePriceId,
                Quantity = cart.ItemQuantity?.GetValueOrDefault(item.ItemId) ?? 0
            }).ToList();

            // Skapa en ny checkout session med Stripe API
            var options = new SessionCreateOptions
            {
                Locale = "sv",
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "SE" },
                },
                ShippingOptions = new List<SessionShippingOptionOptions>
                {
                    new SessionShippingOptionOptions { ShippingRate = "shr_1MnW4gJ9NmDaISNLsDI6gLUz"},
                    new SessionShippingOptionOptions { ShippingRate = "shr_1N3cELJ9NmDaISNLaYLSbzBy"},
                },
                PaymentMethodTypes = new List<string>
                {
                    "card",
                    "klarna",
                },

                LineItems = lineItem,

                Mode = "payment",
                SuccessUrl = Url.Action("Success", "CheckoutApi", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "CheckoutApi", null, Request.Scheme),
            };


            var service = new SessionService();
            var session = service.Create(options);

            // Dirigera användaren till checkout sidan
            return Redirect(session.Url);
        }

        public IActionResult Success()
        {
            return RedirectToAction("successView");
        }

        public IActionResult SuccessView()
        {
            return View("success");
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("CancelView");
        }

        public IActionResult CancelView()
        {
            return View("cancel");
        }

    }
}
