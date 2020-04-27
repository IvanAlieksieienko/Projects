    using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Category
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
