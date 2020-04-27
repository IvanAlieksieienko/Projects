using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Basket
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public int Count { get; set; }
    }
}
