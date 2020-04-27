using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Feature
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public string FeatureName { get; set; }
        public string FeatureValue { get; set; }
        public int Position { get; set; }
    }
}
