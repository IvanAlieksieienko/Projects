using Motopark.Core.Entities;
using Motopark.Core.IServices;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class ProductsPageVM
    {
        private HttpClient _client;
        private int ClickCount = 0;
        public INavigation Navigation { get; set; }
        public ICommand GoToAddPageCommand { get; set; }
        public ICommand GoToEditPageCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private IProductService<Product> _productService { get; set; }
        private ICategoryService<Category> _categoryService { get; set; }
        private IFeatureService<Feature> _featureService { get; set; }
        private IImageProductService<ImageProduct> _imageProductService { get; set; }
        public Page ProductAddPage { get; set; }
        public Page ProductByIDPage { get; set; }
        public Page ProductEditPage { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public ProductsPageVM(IProductService<Product> productService, ICategoryService<Category> categoryService, IFeatureService<Feature> featureService, IImageProductService<ImageProduct> imageProductService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _featureService = featureService;
            _client = new HttpClient();
            _imageProductService = imageProductService;
            GoToAddPageCommand = new Command(GoToAddPageMethod);
            EditCommand = new Command(EditMethod);
            DeleteCommand = new Command(DeleteMethod);
        }

        private async Task GoToProductPageMethod(Product product)
        {
            if (ClickCount > 0 || Device.RuntimePlatform == Device.Android)
            {
                var streams = await GetStreamImageAsync(product);
                var parentCategory = await _categoryService.GetByID(product.CategoryID); // deadlock
                var productFeatues = await _featureService.GetByProductID(product.ID);
                ProductByIDPage.BindingContext = new ProductByIDPageVM() { Product = product, ParentCategory = parentCategory, ImageStreams = streams, Features = new ObservableCollection<Feature>(productFeatues), Navigation = this.Navigation };
                await Navigation.PushAsync(ProductByIDPage);
            }
            else ClickCount++;
        }

        public async Task GetAllProducts()
        {
            Products = new ObservableCollection<Product>(await _productService.GetAll());
            Categories = new ObservableCollection<Category>(await _categoryService.GetAll());
        }

        private async void EditMethod(object sender)
        {
            if (Navigation.NavigationStack.Contains(ProductByIDPage))
            {
                Navigation.RemovePage(ProductByIDPage);
            }
            var product = (Product)sender;
            var streams = await GetStreamImageAsync(product);
            var categories = await _categoryService.GetAll();
            var parentCategory = categories.FirstOrDefault(c => c.ID == product.CategoryID);
            var productFeatues = await _featureService.GetByProductID(product.ID);
            if (Device.RuntimePlatform == Device.Android)
            {
                ProductEditPage.BindingContext = new ProductsEditPageVM()
                {
                    TargetProduct = product,
                    Categories = new ObservableCollection<Category>(categories),
                    Features = new ObservableCollection<Feature>(productFeatues),
                    Navigation = this.Navigation,
                    ParentCategory = parentCategory,
                    _categoryService = _categoryService,
                    _featureService = _featureService,
                    _imageProductService = _imageProductService,
                    ImageStreams = streams,
                    _productService = _productService,
                    CurrentContent = CrossMedia.Current
                };
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                ProductEditPage.BindingContext = new ProductsEditPageVM()
                {
                    TargetProduct = product,
                    Categories = new ObservableCollection<Category>(categories),
                    Features = new ObservableCollection<Feature>(productFeatues),
                    Navigation = this.Navigation,
                    ParentCategory = parentCategory,
                    _categoryService = _categoryService,
                    _featureService = _featureService,
                    _imageProductService = _imageProductService,
                    ImageStreams = streams,
                    _productService = _productService
                };
            }
            ClickCount = 0;
            await Navigation.PushAsync(ProductEditPage);
        }

        private async void DeleteMethod(object sender)
        {
            var product = (Product)sender;
            var answer = await Application.Current.MainPage.DisplayAlert("Удаление", "Вы уверены, что хотите удалить " + product.Name + "?", "Да", "Нет");
            if (answer)
            {
                await _productService.Delete(product.ID);
                Products.Remove(product);
            }
        }

        private void GoToAddPageMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                ProductAddPage.BindingContext = new ProductAddPageVM()
                {
                    Navigation = this.Navigation,
                    Categories = this.Categories,
                    _categoryService = this._categoryService,
                    _featureService = this._featureService,
                    _imageProductService = this._imageProductService,
                    _productService = this._productService,
                    CurrentContent = CrossMedia.Current
                };
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                ProductAddPage.BindingContext = new ProductAddPageVM()
                {
                    Navigation = this.Navigation,
                    Categories = this.Categories,
                    _categoryService = this._categoryService,
                    _featureService = this._featureService,
                    _imageProductService = this._imageProductService,
                    _productService = this._productService
                };
            }
            Navigation.PushAsync(ProductAddPage);
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var viewCell = (ListView)sender;
                viewCell.SelectedItem = null;
                var product = (Product)e.Item;
                await GoToProductPageMethod(product);
            }
        }

        public async Task<List<Stream>> GetStreamImageAsync(Product product)
        {
            var streamList = new List<Stream>();
            var url = @"http://motopark-001-site1.itempurl.com/image/files/product/" + product.ID;

            HttpResponseMessage response = null;
            response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseList = JsonConvert.DeserializeObject<List<byte[]>>(responseContent);
                for (int i = 0; i < responseList.Count; i++)
                {
                    Stream ms = new MemoryStream(responseList[i]);
                    streamList.Add(ms);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить изображение", "ОК");
            }
            return streamList;
        }
    }
}

