using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Layer
{
    public class MyOptions
    {
        public MyOptions()
        {

        }
        public string FromAdress { get; set; }
        public string FromPassword { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string Subject { get; set; }
        public int LifeTime { get; set; } = 1;
        public string Body { get; set; }
        public string FromAddressDisplayName { get; set; }
        public string ToAddressDisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public string StripeApiKey { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
