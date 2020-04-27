using Motopark.Core.Entities;
using Motopark.Core.IServices;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class CategoriesEditPageVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ICategoryService<Category> _categoryService { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public MediaFile ImageFile { get; set; }
        public ICommand MakePhotoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand UpdateCategoryCommand { get; set; }
        public IMedia CurrentContent { get; set; }

        private Category _targetCategory;
        private Category _parentCategory;
        private Image _categoryImage;
        private Stream _imageStream;
        private HttpClient _client;

        public Category TargetCategory 
        { 
            get
            {
                return _targetCategory;
            }
            set
            {
                _targetCategory = value;
                if (value.CategoryID != Guid.Empty)
                    _parentCategory = Categories.FirstOrDefault(c => c.ID == value.CategoryID);
                OnPropertyChanged(nameof(TargetCategory));
            }
        }
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
        public Image CategoryImage
        {
            get
            {
                return _categoryImage;
            }
            set
            {
                _categoryImage = value;
            }
        }
        public Stream ImageStream
        {
            get
            {
                return _imageStream;
            }
            set
            {
                _imageStream = value;
                if (value != null)
                {
                    var image = new Image();
                    image.Source = ImageSource.FromStream(() => { return value; });
                    CategoryImage = image;
                }
            }
        }

        public CategoriesEditPageVM()
        {
            MakePhotoCommand = new Command(MakePhotoMethod);
            PickPhotoCommand = new Command(PickPhotoMethod);
            UpdateCategoryCommand = new Command(UpdateCategoryMethod);
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

                CategoryImage.Source = ImageSource.FromFile(ImageFile.Path);
            }
        }

        public async void PickPhotoMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (CurrentContent.IsPickPhotoSupported)
                {
                    ImageFile = await CurrentContent.PickPhotoAsync();
                    CategoryImage.Source = ImageSource.FromFile(ImageFile.Path);
                }
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                var op = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                CategoryImage.Source = ImageSource.FromFile(op.FilePath);
            }
        }

        public async void UpdateCategoryMethod()
        {
            if (ParentCategory == null) _targetCategory.CategoryID = Guid.Empty;
            else _targetCategory.CategoryID = ParentCategory.ID;
            if (_targetCategory.Name == string.Empty)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название категории не может быть пустым", "ОК");
                return;
            }
            MediaFile categoryImage;
            if (ImageFile != null || !(CategoryImage.Source is StreamImageSource))
            {
                categoryImage = ImageFile;
                // HttpRequest
                var url = @"http://motopark-001-site1.itempurl.com/category/upload";

                HttpContent httpContent;
                var content = new MultipartFormDataContent();
                if (Device.RuntimePlatform == Device.WPF)
                {
                    var fullPath = Path.GetFullPath((CategoryImage.Source as FileImageSource).File);
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
                    _targetCategory.ImagePath = responseContent.ToString();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось отправить изображение", "ОК");
                    ImageFile = null;
                    CategoryImage.Source = string.Empty;
                    return;
                }
            }
            await _categoryService.Update(_targetCategory);
            await Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
