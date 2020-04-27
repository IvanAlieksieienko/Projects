using System;
using System.Collections.Generic;
using System.Text;

namespace Motopark.Core.Entities
{
    public class Order
    {
        public Guid ID { get; set; }
        public Guid BasketID { get; set; }
        public Guid DeliveryID { get; set; }
        public double TotalPrice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public DateTime CreationTime { get; set; }
        private State _orderState;
        public int OrderState
        {
            get
            {
                return (int)_orderState;
            }
            set
            {
                _orderState = value == 0 ? State.Pending : value == 1 ? State.Proccessing : value == 2 ? State.Ready : State.Pending;
            }
        }
    }

    public enum State
    {
        Pending = 0,
        Proccessing = 1,
        Ready = 2
    }
}
