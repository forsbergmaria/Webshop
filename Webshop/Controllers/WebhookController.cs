using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using Stripe.Checkout;
using Data;
using Models;

namespace Webshop.Controllers
{
    
    [ApiController]
    public class WebhookController : Controller
    {
        OrderRepository _orderRepository { get { return new OrderRepository(); } }
        const string endpointSecret = "whsec_f3e2fe06ee6092c7f6e31baead862e98d2179531ac6c446b2cf4b3aec9dfabbe";

        [Route("webhook")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var session = stripeEvent.Data.Object as Session;
                var options = new SessionListLineItemsOptions
                {
                    Limit = 100,
                };
                var service = new SessionService();
                StripeList<LineItem> lineItems = service.ListLineItems(session.Id, options);


                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentFailed)
                {
                }
                else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {

                    AddOrderToDatabase(session);
                    return Ok();
                }
                else if (stripeEvent.Type == Events.CheckoutSessionExpired)
                {
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        [Route("AddOrder")]
        public void AddOrderToDatabase(Session session)
        {
            var order = new Order();
            order.OrderDate = DateTime.Now;
            order.CustomerName = session.ShippingDetails.Name;
            order.CustomerAddress = session.ShippingDetails.Address.Line1;
            order.CustomerAddress2 = session.ShippingDetails.Address.Line2;
            order.CustomerPhone = session.ShippingDetails.Phone;
            order.CustomerCity = session.ShippingDetails.Address.City;
            order.CustomerZipCode = session.ShippingDetails.Address.PostalCode;
            _orderRepository.CreateOrder(order);
        }
    }
}
