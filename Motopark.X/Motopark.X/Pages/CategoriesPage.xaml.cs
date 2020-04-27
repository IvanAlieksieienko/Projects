using Motopark.Core.Entities;
using Motopark.Core.IServices;
using Motopark.Core.ViewModels;
using Nancy.TinyIoc;
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
    public partial class CategoriesPage : ContentPage
    {
        private IUnityContainer _container;
        private CategoriesPageVM _categoriesPageVM;

        public CategoriesPage(IUnityContainer container)
        {
            _container = container;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            _categoriesPageVM = _container.Resolve<CategoriesPageVM>();
            _categoriesPageVM.Navigation = this.Navigation;
            _categoriesPageVM.CategoryAddPage = new CategoryAddPage();
            _categoriesPageVM.CategoryByIDPage = new CategoryByIDPage();
            _categoriesPageVM.CategoryEditPage = new CategoriesEditPage();
            categories.ItemTapped += _categoriesPageVM.OnItemTapped;
            await _categoriesPageVM.GetAllCategories();
            BindingContext = _categoriesPageVM;
        }

        protected override async void OnDisappearing()
        {
            categories.ItemTapped -= _categoriesPageVM.OnItemTapped;
        }
    }
}