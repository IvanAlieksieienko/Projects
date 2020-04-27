using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace BookStore.APILayer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class PayController : Controller
    {
        private readonly MyOptions _options;
        public PayController(IOptionsMonitor<MyOptions> optionsAccessor)
        {
            _options = optionsAccessor.CurrentValue;
        }

        [HttpPost("Payment")]
        public string Pay([FromBody]PayFields fields)
        {
            StripeConfiguration.ApiKey = _options.StripeApiKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions> {
                    new SessionLineItemOptions {
                        Name = fields.Email,
                        Description = fields.Title,
                        Amount = fields.Price * 10,
                        Currency = "usd",
                        Quantity = 1,
                    },
                },
                SuccessUrl = _options.SuccessUrl + fields.BookId.ToString(),
                CancelUrl = _options.CancelUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session.Id;
        }

        public struct PayFields
        {
            public string Email;
            public string Title;
            public int Price;
            public int BookId;
        }
    }
}