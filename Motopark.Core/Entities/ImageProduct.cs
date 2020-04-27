using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class ImageProduct
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public string ImagePath { get; set; }
        public bool IsFirst { get; set; }
    }
}
