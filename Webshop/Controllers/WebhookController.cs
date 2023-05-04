using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Webshop.Controllers
{
    [Route("webhook")]
    [ApiController]
    public class WebhookController : Controller
    {

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "we_1N3edAJ9NmDaISNL9Trmgsx1";

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == Events.CheckoutSessionAsyncPaymentFailed)
                {
                    return RedirectToAction("success");
                }
                else if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
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
    }
}
