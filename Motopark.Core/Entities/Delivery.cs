using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Delivery
    {
        private PaymentType payType;
        private PostType postName;
        private DeliveryType deliveryName;
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public int PayType 
        {
            get
            {
                return (int)payType;
            }
            set
            {
                payType = value == 0 ? PaymentType.Cash : PaymentType.Cashless;
            }
        }
        public int PostName
        {
            get
            {
                return (int)postName;
            }
            set
            {
                postName = value == 0 ? PostType.Self : value == 1 ? PostType.NovaPoshta : value == 2 ? PostType.MeestExpress : PostType.UkrPoshta;
            }
        }
        public int DeliveryName
        {
            get
            {
                return (int)deliveryName;
            }
            set
            {
                deliveryName = value == 0 ? DeliveryType.Courier : DeliveryType.Department;
            }
        }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }

        
    }

    public enum PaymentType
    {
        Cash = 0,
        Cashless = 1
    }
    public enum PostType
    {
        Self = 0,
        NovaPoshta = 1,
        MeestExpress = 2,
        UkrPoshta = 3
    }
    public enum DeliveryType
    {
        Courier = 0,
        Department = 1
    }
}
