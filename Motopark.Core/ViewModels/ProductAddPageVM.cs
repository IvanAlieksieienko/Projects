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
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class ProductAddPageVM
    {
        private ObservableCollection<Feature> _features;
        private Category _parentCategory;
        private string _name;
        private string _description;
        private double _price;
        private bool _isAvailable = true;

        private ObservableCollection<Image> _images;
        private HttpClient _client;

        public INavigation Navigation { get; set; }
        public ICategoryService<Category> _categoryService { get; set; }
        public IProductService<Product> _productService { get; set; }
        public IFeatureService<Feature> _featureService { get; set; }
        public IImageProductService<ImageProduct> _imageProductService { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public List<MediaFile> ImageFiles { get; set; }
        public ICommand MakePhotoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand AddFeatureCommand { get; set; }
        public IMedia CurrentContent { get; set; }
        public ObservableCollection<Image> Images
        {
            get
            {
                return _images;
            }
            set
            {

                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        }
        public ObservableCollection<Feature> Features
        {
            get
            {
                return _features;
            }
            set
            {
                _features = value;
                OnPropertyChanged(nameof(Features));
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
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public bool IsAvailable
        {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public ProductAddPageVM()
        {
            _client = new HttpClient();
            _images = new ObservableCollection<Image>();
            _features = new ObservableCollection<Feature>();
            ImageFiles = new List<MediaFile>();
            MakePhotoCommand = new Command(MakePhotoMethod);
            PickPhotoCommand = new Command(PickPhotoMethod);
            AddProductCommand = new Command(AddProductMethod);
            AddFeatureCommand = new Command(AddFeatureMethod);
        }

        public async void MakePhotoMethod()
        {
            if (CurrentContent.IsCameraAvailable && CurrentContent.IsTakePhotoSupported)
            {
                MediaFile imageFile = await CurrentContent.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (imageFile == null)
                    return;

                ImageFiles.Add(imageFile);
                Image image = new Image();
                image.Source = ImageSource.FromFile(imageFile.Path);
                Images.Add(image);
            }
        }

        public async void PickPhotoMethod()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (CurrentContent.IsPickPhotoSupported)
                {
                    MediaFile imageFile = await CurrentContent.PickPhotoAsync();
                    ImageFiles.Add(imageFile);
                    Image image = new Image();
                    image.Source = ImageSource.FromFile(imageFile.Path);
                    Images.Add(image);
                }
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                var op = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                MediaFile imageFile = new MediaFile(string.Empty, null);
                ImageFiles.Add(imageFile);
                Image image = new Image();
                image.Source = ImageSource.FromFile(op.FilePath);
                Images.Add(image);
            }
            
        }

        public async void AddProductMethod()
        {
            Product newProduct = new Product();
            if (ParentCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Выберите подкатегорию или создайте категорию 'Без категории' и добавьте продукт туда", "ОК");
                return;
            }
            if (Name == string.Empty)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название продукта не может быть пустым", "ОК");
                return;
            }
            if (Price == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Цена не может быть 0", "ОК");
                return;
            }
            newProduct.CategoryID = ParentCategory.ID;
            newProduct.Name = Name;
            newProduct.Description = Description;
            newProduct.Price = Price;
            newProduct.IsAvailable = IsAvailable;
            var responsePoduct = await _productService.Add(newProduct);

            int count = 0;
            if (ImageFiles.Count > 0 || Images.Count > 0)
            {
                for (int i = 0; i < ImageFiles.Count; i++)
                {
                    ImageProduct newImageProduct = new ImageProduct();
                    var url = @"http://motopark-001-site1.itempurl.com/product/upload";

                    HttpContent httpContent;
                    var content = new MultipartFormDataContent();
                    if (Device.RuntimePlatform == Device.WPF)
                    {
                        var fullPath = Path.GetFullPath((Images[i].Source as FileImageSource).File);
                        var fileBytes = File.ReadAllBytes(fullPath);
                        var fileName = Path.GetFileName(fullPath);
                        httpContent = new ByteArrayContent(fileBytes);
                        content.Add(httpContent, "File", fileName);
                    }
                    else
                    {
                        httpContent = new StreamContent(ImageFiles[i].GetStream());
                        var fileName = Path.GetFileName(ImageFiles[i].Path);
                        httpContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        content.Add(httpContent);
                    }

                    HttpResponseMessage response = null;
                    response = await _client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        newImageProduct.ImagePath = responseContent.ToString();
                        newImageProduct.IsFirst = count == 0;
                        newImageProduct.ProductID = responsePoduct.ID;
                        await _imageProductService.Add(newImageProduct);
                        count++;
                    }
                    else
                    {
                        newImageProduct.ImagePath = @"Resources\Images\default.png";
                        newImageProduct.IsFirst = count == 0;
                        newImageProduct.ProductID = responsePoduct.ID;
                        await _imageProductService.Add(newImageProduct);
                        count++;
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось отправить изображение", "ОК");
                        this.ImageFiles = new List<MediaFile>();
                        Images = new ObservableCollection<Image>();
                    }
                }

            }

            if (Features.Count > 0)
            {
                var features = Features.ToList();
                for (int i = 0; i < Features.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Features[i].FeatureName) && !string.IsNullOrEmpty(Features[i].FeatureValue))
                    {
                        Features[i].ProductID = responsePoduct.ID;
                        await _featureService.Add(Features[i]);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Характеристика или ее значение не может быть пустым!", "ОК");
                        return;
                    }
                }
            }
            await Navigation.PopAsync();
        }

        public byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageByteArray = null;
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                imageByteArray = new byte[reader.BaseStream.Length];
                for (int i = 0; i < reader.BaseStream.Length; i++)
                    imageByteArray[i] = reader.ReadByte();
            }
            return imageByteArray;
        }

        public async void AddFeatureMethod()
        {
            Features.Add(new Feature() { Position = Features.Count + 1, FeatureName = string.Empty, FeatureValue = string.Empty });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
