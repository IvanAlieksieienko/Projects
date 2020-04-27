using Motopark.Core.Entities;
using Motopark.Core.Entities.CRM;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class OrderByIDPageVM : INotifyPropertyChanged
    {
        private string _orderStatus;
        private Order _order;
        private ObservableCollection<string> _statuses = new ObservableCollection<string>() { State.Pending.ToString(), State.Proccessing.ToString(), State.Ready.ToString() };
        public INavigation Navigation { get; set; }
        public Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OrderStatus = value.OrderState == 0 ? State.Pending.ToString() : value.OrderState == 1 ? 
                    State.Proccessing.ToString() : value.OrderState == 2 ? State.Ready.ToString() : State.Pending.ToString();
                OnPropertyChanged(nameof(Order));
            }
        }
        public string OrderStatus
        {
            get
            {
                return _orderStatus;
            }
            set
            {
                _orderStatus = value;
                Order.OrderState = value == State.Pending.ToString() ? (int)State.Pending : value == State.Proccessing.ToString() ? (int)State.Proccessing :
                    value == State.Ready.ToString() ? (int)State.Ready : (int)State.Pending;
                OnPropertyChanged(nameof(OrderStatus));
            }
        }

        public ObservableCollection<string> Statuses
        {
            get
            {
                return _statuses;
            }
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }
        public Delivery Delivery { get; set; }
        public ObservableCollection<Basket> Baskets { get; set; }
        public ObservableCollection<ProductBasket> ProductBaskets { get; set; }
        public ICommand SaveCommand { get; set; }
        public IOrderService<Order> _orderService { get; set; }

        public OrderByIDPageVM(IOrderService<Order> orderService)
        {
            _orderService = orderService;
            Baskets = new ObservableCollection<Basket>();
            ProductBaskets = new ObservableCollection<ProductBasket>();
            SaveCommand = new Command(SaveMethod);
        }

        public async void SaveMethod()
        {
            await _orderService.Update(Order);
            await Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
