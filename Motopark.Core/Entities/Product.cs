using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Product
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
