using System;
using System.Collections.Generic;
using System.Text;

namespace Plants.Core.Entities
{
    public class Admin
    {
        public Guid ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
