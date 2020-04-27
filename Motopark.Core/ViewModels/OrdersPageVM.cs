using Motopark.Core.Entities;
using Motopark.Core.Entities.CRM;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class OrdersPageVM
    {
        private HttpClient _client;
        public INavigation Navigation { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private IOrderService<Order> _orderService { get; set; }
        private IDeliveryService<Delivery> _deliveryService { get; set; }
        private IBasketService<Basket> _basketService { get; set; }
        private IProductService<Product> _productService { get; set; }
        public Page OrderByIDPage { get; set; }
        public Page OrderEditPage { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        private int ClickCount = 0;

        public OrdersPageVM(IOrderService<Order> orderService, IDeliveryService<Delivery> deliveryService, 
            IBasketService<Basket> basketService, IProductService<Product> productService)
        {
            this._orderService = orderService;
            this._deliveryService = deliveryService;
            this._basketService = basketService;
            this._productService = productService;
            _client = new HttpClient();
            DeleteCommand = new Command(DeleteMethod);
            EditCommand = new Command(EditMethod);
        }

        public async Task GetAllOrders()
        {
            Orders = new ObservableCollection<Order>(await _orderService.GetAll());
        }

        public async Task GoToOrderPageMethod(Order order)
        {
            if (ClickCount > 0 || Device.RuntimePlatform == Device.Android)
            {
                var delivery = await _deliveryService.GetByID(order.DeliveryID);
                var baskets = await _basketService.GetByBasketID(order.BasketID);
                var products = await _productService.GetAll();
                List<ProductBasket> productBaskets = new List<ProductBasket>();
                baskets.ToList().ForEach((basket) =>
                {
                    var product = products.FirstOrDefault(p => p.ID == basket.ProductID);
                    productBaskets.Add(new ProductBasket() { Count = basket.Count, ProductName = product.Name, ProductPrice = product.Price });
                });

                OrderByIDPage.BindingContext = new OrderByIDPageVM(_orderService) { Navigation = this.Navigation, Order = order, Baskets = new ObservableCollection<Basket>(baskets), Delivery = delivery,
                ProductBaskets = new ObservableCollection<ProductBasket>(productBaskets) };
                
                Navigation.PushAsync(OrderByIDPage);
            }
            else ClickCount++;
        }

        public async void DeleteMethod(object sender)
        {
            var order = (Order)sender;
            Orders.Remove(order);
            await _orderService.Delete(order.ID);
        }

        public async void EditMethod(object sender)
        {
            var order = (Order)sender;
            OrderEditPage.BindingContext = new OrderEditPageVM();
            ClickCount = 0;
            Navigation.PushAsync(OrderEditPage);
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var viewCell = (ListView)sender;
                viewCell.SelectedItem = null;
                var order = (Order)e.Item;
                await GoToOrderPageMethod(order);
            }
        }
    }
}
