using Motopark.Core.Entities;
using Motopark.Core.IServices;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class ProductsEditPageVM
    {
        private Category _parentCategory;
        private Product _targetProduct;
        private ObservableCollection<Feature> _features;
        private ObservableCollection<Image> _images;
        private List<Stream> _imageStreams;
        private HttpClient _client;

        public INavigation Navigation { get; set; }
        public ICategoryService<Category> _categoryService { get; set; }
        public IProductService<Product> _productService { get; set; }
        public IFeatureService<Feature> _featureService { get; set; }
        public IImageProductService<ImageProduct> _imageProductService { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand MakePhotoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand EditProductCommand { get; set; }
        public ICommand AddFeatureCommand { get; set; }
        public ICommand ChangeCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
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
        public List<Stream> ImageStreams
        {
            get
            {
                return _imageStreams;
            }
            set
            {
                _imageStreams = value;
                if (value != null && value.Count > 0)
                {
                    value.ForEach(async (Stream imageStream) =>
                    {
                        var image = new Image();
                        image.Source = ImageSource.FromStream(() => { return imageStream; });
                        Images.Add(image);
                    });
                }
            }
        }
        public Product TargetProduct
        {
            get
            {
                return _targetProduct;
            }
            set
            {
                _targetProduct = value;
                OnPropertyChanged(nameof(TargetProduct));
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

        public ProductsEditPageVM()
        {
            _client = new HttpClient();
            _images = new ObservableCollection<Image>();
            _imageStreams = new List<Stream>();
            _features = new ObservableCollection<Feature>();
            MakePhotoCommand = new Command(MakePhotoMethod);
            PickPhotoCommand = new Command(PickPhotoMethod);
            EditProductCommand = new Command(EditProductMethod);
            AddFeatureCommand = new Command(AddFeatureMethod);
            ChangeCommand = new Command(ChangeMethod);
            DeleteCommand = new Command(DeleteMethod);
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

                ImageStreams.Add(imageFile.GetStream());
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
                    ImageStreams.Add(imageFile.GetStream());
                    Image image = new Image();
                    image.Source = ImageSource.FromFile(imageFile.Path);
                    Images.Add(image);
                }
            }
            if (Device.RuntimePlatform == Device.WPF)
            {
                var op = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                var stream = new MemoryStream();
                ImageStreams.Add(stream);
                Image image = new Image();
                image.Source = ImageSource.FromFile(op.FilePath);
                Images.Add(image);
            }
        }

        public async void EditProductMethod()
        {
            Product newProduct = new Product();
            if (ParentCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Выберите подкатегорию или создайте категорию 'Без категории' и добавьте продукт туда", "ОК");
                return;
            }
            else TargetProduct.CategoryID = ParentCategory.ID;
            if (TargetProduct.Name == string.Empty)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Название продукта не может быть пустым", "ОК");
                return;
            }
            if (TargetProduct.Price == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Цена не может быть 0", "ОК");
                return;
            }
            var responsePoduct = await _productService.Update(TargetProduct);

            int count = 0;
            if (Images.Count > 0)
            {
                var imageProducts = (await _imageProductService.GetByProductID(responsePoduct.ID)).ToList();
                for (int i = 0; i < Images.Count; i++)
                {
                    ImageProduct newImageProduct = new ImageProduct();
                    var url = @"http://motopark-001-site1.itempurl.com/product/upload";

                    if (Images[i].Source is FileImageSource source)
                    {
                        using (var fstream = File.OpenRead(Path.GetFullPath(source.File)))
                        {
                            HttpContent fileStreamContent = new StreamContent(fstream);
                            var fileName = Path.GetFileName(source.File);
                            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                            var content = new MultipartFormDataContent();
                            content.Add(fileStreamContent);

                            HttpResponseMessage response = null;
                            response = await _client.PostAsync(url, content);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                if (i <= (imageProducts.Count - 1))
                                {
                                    imageProducts[i].ImagePath = responseContent.ToString();
                                    await _imageProductService.Update(imageProducts[i]);

                                }
                                else
                                {
                                    newImageProduct.ImagePath = responseContent.ToString();
                                    newImageProduct.IsFirst = count == 0;
                                    newImageProduct.ProductID = responsePoduct.ID;
                                    await _imageProductService.Add(newImageProduct);
                                }
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
                                Images = new ObservableCollection<Image>();
                            }
                        }
                    }
                    
                }

            }

            if (Features.Count > 0)
            {
                var initProductFeatures = await _featureService.GetByProductID(responsePoduct.ID);
                var features = Features.ToList();
                for (int i = 0; i < Features.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Features[i].FeatureName) && !string.IsNullOrEmpty(Features[i].FeatureValue))
                    {
                        if (Features[i].ID != Guid.Empty)
                        {
                            if (initProductFeatures.FirstOrDefault(p => p.ID == Features[i].ID) != null)
                            {
                                await _featureService.Update(Features[i]);
                            }
                        }
                        else
                        {
                            Features[i].ProductID = responsePoduct.ID;
                            await _featureService.Add(Features[i]);
                        }
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

        public async void ChangeMethod(object sender)
        {
            var image = (Image)sender;
            if (Device.RuntimePlatform == Device.WPF)
            {
                var op = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
                var stream = new MemoryStream();
                ImageStreams.Add(stream);
                Images[Images.IndexOf(image)].Source = ImageSource.FromFile(op.FilePath);
            }
            else
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Изменение", "Как вы хотите изменить?", "Снять на камеру", "Выбрать из галерии");
                if (answer)
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

                        ImageStreams[Images.IndexOf(image)] = imageFile.GetStream();
                        Images[Images.IndexOf(image)].Source = ImageSource.FromFile(imageFile.Path);
                    }
                }
                else
                {
                    if (CurrentContent.IsPickPhotoSupported)
                    {
                        MediaFile imageFile = await CurrentContent.PickPhotoAsync();
                        ImageStreams[Images.IndexOf(image)] = imageFile.GetStream();
                        Images[Images.IndexOf(image)].Source = ImageSource.FromFile(imageFile.Path);
                    }
                }
            }
        }

        public async void DeleteMethod(object sender)
        {
            var image = (Image)sender;
            Images.Remove(image);
        }

        public async void AddFeatureMethod()
        {
            Features.Add(new Feature() { Position = Features.Count + 1, FeatureName = string.Empty, FeatureValue = string.Empty });
        }

        public async Task<Stream> GetStreamFromImageSourceAsync(StreamImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageSource.Stream != null)
            {
                return await imageSource.Stream(cancellationToken);
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
