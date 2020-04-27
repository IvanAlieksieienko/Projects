using Motopark.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motopark.X.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        private IUnityContainer _container;
        private OrdersPageVM _ordersPageVM;

        public OrdersPage(IUnityContainer container)
        {
            _container = container;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            _ordersPageVM = _container.Resolve<OrdersPageVM>();
            _ordersPageVM.Navigation = this.Navigation;
            _ordersPageVM.OrderByIDPage = new OrderByIDPage();
            _ordersPageVM.OrderEditPage = new OrderEditPage();
            orders.ItemTapped += _ordersPageVM.OnItemTapped;
            await _ordersPageVM.GetAllOrders();
            BindingContext = _ordersPageVM;
        }

        protected override async void OnDisappearing()
        {
            orders.ItemTapped -= _ordersPageVM.OnItemTapped;
        }
    }
}