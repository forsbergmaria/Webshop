using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Newtonsoft.Json;
using NuGet.Protocol;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

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
            var cartItems = cart.Items.ToList();

            StripeConfiguration.ApiKey = "sk_test_51MnVSuJ9NmDaISNLt2DpWzyfEpec4JZF1Zf9gwPkecoDj2OYmXX9ThWfvXB2nEbadLp51BI6AuooidYslZ6yykDg00pjXolXbJ";

            var lineItems = new List<SessionLineItemOptions>();
            foreach (var cartItem in cartItems)
            {
                var priceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "sek",
                    UnitAmount = (long)cartItem.PriceWithoutVAT * 100 * (long)cartItem.VAT,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cartItem.Name,
                        Description = cartItem.Description,
                    },
                };

                //var price = new PriceCreateOptions
                //{
                //    UnitAmountDecimal = cartItem.PriceWithoutVAT * 100 * cartItem.VAT,
                //    Currency = "sek",
                //    ProductData = new PriceProductDataOptions
                //    {
                //        Name = cartItem.Name,
                //        Id = cartItem.StripeItemId,
                //    },
                //};

                foreach (var item in cart.Items.DistinctBy(i => i.ItemId))
                {
                    var lineItem = new SessionLineItemOptions
                    {
                        PriceData = priceData,
                        //Price = item.StripePriceId,
                        Quantity = cart.ItemQuantity?.GetValueOrDefault(item.ItemId) ?? 0
                    };

                    lineItems.Add(lineItem);
                }

                //var lineItem = cart.Items.DistinctBy(i => i.ItemId).Select(item => new SessionLineItemOptions
                //{
                //    PriceData = priceData,
                //    Price = item.StripePriceId,
                //    Quantity = cart.ItemQuantity?.GetValueOrDefault(item.ItemId) ?? 0
                //}).ToList();
                //lineItems.Add(lineItem);



                //var lineItem = new SessionLineItemOptions
                //{
                //    PriceData = priceData,
                //    Price = cartItem.StripePriceId,
                //    Quantity = cart.ItemQuantity,
                //};
                //lineItems.Add(lineItem);
            }

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

                LineItems = lineItems,

                //LineItems = lineItem,

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
