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
    public partial class ProductsPage : ContentPage
    {
        private IUnityContainer _container;
        private ProductsPageVM _productsPageVM;

        public ProductsPage(IUnityContainer container)
        {
            _container = container;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            _productsPageVM = _container.Resolve<ProductsPageVM>();
            _productsPageVM.Navigation = this.Navigation;
            _productsPageVM.ProductAddPage = new ProductAddPage();
            _productsPageVM.ProductByIDPage = new ProductByIDPage();
            _productsPageVM.ProductEditPage = new ProductsEditPage();
            products.ItemTapped += _productsPageVM.OnItemTapped;
            await _productsPageVM.GetAllProducts();
            BindingContext = _productsPageVM;
        }

        protected override async void OnDisappearing()
        {
            products.ItemTapped -= _productsPageVM.OnItemTapped;
        }
    }
}