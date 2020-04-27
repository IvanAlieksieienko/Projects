using Motopark.Core.Entities;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class ProductByIDPageVM
    {
        private ObservableCollection<Feature> _features;
        private Product _product;
        private Category _parentCategory;
        private List<Image> _productImages;
        private List<Stream> _imageStreams;
        private bool _isShowParent = true;
        private bool _isShowDescription = true;
        private bool _isShowFeatures = true;

        public INavigation Navigation { get; set; }
        public ObservableCollection<Feature> Features
        {
            get
            {
                return _features;
            }
            set
            {
                _features = value;
                if (value != null && value.Count != 0) IsShowFeatures = true;
                else IsShowFeatures = false;
            }
        }
        public Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
                if (_product.Description == string.Empty) IsShowDescription = false;
                else IsShowDescription = true;
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
                if (value == null) IsShowParent = false;
                else IsShowParent = true;
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
                        ProductImages.Add(image);
                    });
                }
            }
        }
        public List<Image> ProductImages
        {
            get
            {
                return _productImages;
            }
            set
            {
                _productImages = value;
            }
        }
        public bool IsShowParent
        {
            get
            {
                return _isShowParent;
            }
            set
            {
                _isShowParent = value;
                OnPropertyChanged(nameof(IsShowParent));
            }
        }
        public bool IsShowDescription
        {
            get
            {
                return _isShowDescription;
            }
            set
            {
                _isShowDescription = value;
                OnPropertyChanged(nameof(IsShowDescription));
            }
        }
        public bool IsShowFeatures
        {
            get
            {
                return _isShowFeatures;
            }
            set
            {
                _isShowFeatures = value;
                OnPropertyChanged(nameof(IsShowFeatures));
            }
        }

        public ProductByIDPageVM()
        {
            _imageStreams = new List<Stream>();
            _productImages = new List<Image>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
