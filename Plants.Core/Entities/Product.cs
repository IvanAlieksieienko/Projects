using System;
using System.Collections.Generic;
using System.Text;

namespace Plants.Core.Entities
{
    public class Product
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public bool IsAvailable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; } 
    }
}
