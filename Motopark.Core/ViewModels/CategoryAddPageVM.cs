using Microsoft.Win32;
using Motopark.Core.Entities;
using Motopark.Core.IServices;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class CategoryAddPageVM : INotifyPropertyChanged
    {
        private Category _parentCategory;
        private string _name;
        private string _description;
        private HttpClient _client;

        public INavigation Navigation { get; set; }
        public ICategoryService<Category> _categoryService { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public Image Image { get; set; } = new Image();
        public MediaFile ImageFile { get; set; }
        public ICommand MakePhotoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public IMedia CurrentContent { get; set; }
        public Category ParentCategory
        {
            get
            {
                return _parentCategory;
            }
            set
            {
                _parentCategory = value;
                OnPropertyChanged(nameof(ParentCategory));
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public CategoryAddPageVM()
        {
            MakePhotoCommand = new Command(MakePhotoMethod);
            PickPhotoCommand = new Command(PickPhotoMethod);
            AddCategoryCommand = new Command(AddCategoryMethod);
            _client = new HttpClient();
        }

        public async void MakePhotoMethod()
        {
            if (CurrentContent.IsCameraAvailable && CurrentContent.IsTakePhotoSupported)
            {
                ImageFile = await CurrentContent.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (ImageFile == null)
                    return;

                Image.Source = ImageSource.FromFile(ImageFile.Path);
            }
        }

        public async void PickPhotoMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (CurrentContent.IsPickPhotoSupported)
                {
                    ImageFile = await CurrentContent.PickPhotoAsync();
                    Image.Source = ImageSource.FromFile(ImageFile.Path);
                }
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                var op = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                Image.Source = ImageSource.FromFile(op.FilePath);
            }
        }

        public async void AddCategoryMethod()
        {
            Category newCategory = new Category();
            if (ParentCategory == null) newCategory.CategoryID = Guid.Empty;
            else newCategory.CategoryID = ParentCategory.ID;
            if (Name != string.Empty) newCategory.Name = Name;
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название категории не может быть пустым", "ОК");
                return;
            }
            newCategory.Description = Description;
            MediaFile categoryImage;
            if (ImageFile != null || Image != null)
            {
                categoryImage = ImageFile;
                // HttpRequest
                var url = @"http://motopark-001-site1.itempurl.com/category/upload";

                HttpContent httpContent;
                var content = new MultipartFormDataContent();
                if (Device.RuntimePlatform == Device.WPF)
                {
                    var fullPath = Path.GetFullPath((Image.Source as FileImageSource).File);
                    var fileBytes = File.ReadAllBytes(fullPath);
                    var fileName = Path.GetFileName(fullPath);
                    httpContent = new ByteArrayContent(fileBytes);
                    content.Add(httpContent, "File", fileName);
                }
                else
                {
                    httpContent = new StreamContent(ImageFile.GetStream());
                    var fileName = Path.GetFileName(ImageFile.Path);
                    httpContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(httpContent);
                }

                

                HttpResponseMessage response = null;
                response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    newCategory.ImagePath = responseContent.ToString();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось отправить изображение", "ОК");
                    ImageFile = null;
                    Image.Source = string.Empty;
                    return;
                }
            }
            else newCategory.ImagePath = @"Resources\Images\default.png";
            await _categoryService.Add(newCategory);
            await Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
