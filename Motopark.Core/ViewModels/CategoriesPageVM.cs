using Motopark.Core.Entities;
using Motopark.Core.IServices;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class CategoriesPageVM
    {
        private HttpClient _client;
        private int ClickCount = 0;
        public INavigation Navigation { get; set; }
        public ICommand GoToAddPageCommand { get; set; }
        public ICommand GoToEditPageCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private ICategoryService<Category> _categoryService { get; set; }
        public Page CategoryAddPage { get; set; }
        public Page CategoryByIDPage { get; set; }
        public Page CategoryEditPage { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public CategoriesPageVM(ICategoryService<Category> categoryService)
        {
            this._categoryService = categoryService;
            _client = new HttpClient();
            GoToAddPageCommand = new Command(GoToAddPageMethod);
            EditCommand = new Command(EditMethod);
            DeleteCommand = new Command(DeleteMethod);
        }

        private async Task GoToCategoryPageMethod(Category category)
        {
            if (ClickCount > 0 || Device.RuntimePlatform == Device.Android)
            {
                var stream = await GetStreamImageAsync(category);
                var parentCategory = await _categoryService.GetByID(category.CategoryID); // deadlock
                CategoryByIDPage.BindingContext = new CategoryByIDPageVM(_categoryService) { Category = category, ParentCategory = parentCategory, ImageStream = stream, Navigation = this.Navigation };
                await Navigation.PushAsync(CategoryByIDPage);
            }
            else ClickCount++;
        }

        public async Task GetAllCategories()
        {
            Categories = new ObservableCollection<Category>(await _categoryService.GetAll());
        }

        private async void EditMethod(object sender)
        {
            var category = (Category)sender;
            var stream = await GetStreamImageAsync(category);
            if (Device.RuntimePlatform == Device.Android) 
                CategoryEditPage.BindingContext = new CategoriesEditPageVM() { Navigation = this.Navigation, Categories = new ObservableCollection<Category>(this.Categories), ImageStream = stream, CurrentContent = CrossMedia.Current, _categoryService = this._categoryService, TargetCategory = category };
            if (Device.RuntimePlatform == Device.WPF)
                CategoryEditPage.BindingContext = new CategoriesEditPageVM() { Navigation = this.Navigation, Categories = new ObservableCollection<Category>(this.Categories), ImageStream = stream, _categoryService = this._categoryService, TargetCategory = category };
            ClickCount = 0;
            await Navigation.PushAsync(CategoryEditPage);
        }

        private async void DeleteMethod(object sender)
        {
            var category = (Category)sender;
            var answer = await Application.Current.MainPage.DisplayAlert("Удаление", "Вы уверены, что хотите удалить " + category.Name + "?", "Да", "Нет");
            if (answer)
            {
                await _categoryService.Delete(category.ID);
                Categories.Remove(category);
            }
        }

        private async void GoToAddPageMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
                CategoryAddPage.BindingContext = new CategoryAddPageVM() { Navigation = this.Navigation, Categories = new ObservableCollection<Category>(this.Categories), CurrentContent = CrossMedia.Current, _categoryService = this._categoryService };
            if (Device.RuntimePlatform == Device.WPF)
                CategoryAddPage.BindingContext = new CategoryAddPageVM() { Navigation = this.Navigation, Categories = new ObservableCollection<Category>(this.Categories), _categoryService = this._categoryService };
            Navigation.PushAsync(CategoryAddPage);
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var viewCell = (ListView)sender;
                viewCell.SelectedItem = null;
                var category = (Category)e.Item;
                await GoToCategoryPageMethod(category);
            }
        }

        public async Task<Stream> GetStreamImageAsync(Category category)
        {
            if (!string.IsNullOrEmpty(category.ImagePath))
            {
                var url = @"http://motopark-001-site1.itempurl.com/image/files/category/" + category.ID;

                HttpResponseMessage response = null;
                response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsByteArrayAsync();
                    Stream ms = new MemoryStream(responseContent);
                    return ms;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить изображение", "ОК");
                    return null;
                }
            }
            return null;
        }
    }
}
