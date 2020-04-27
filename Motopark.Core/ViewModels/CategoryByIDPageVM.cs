using Motopark.Core.Entities;
using Motopark.Core.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;

namespace Motopark.Core.ViewModels
{
    public class CategoryByIDPageVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        private ICategoryService<Category> _categoryService { get; set; }
        private Category _category;
        private Category _parentCategory;
        private Image _categoryImage;
        private Stream _imageStream;
        private bool _isShowParent = true;
        private bool _isShowDescription = true;
        private bool _isShowImage = true;

        public Category Category { 
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                if (_category.Description == string.Empty) IsShowDescription = false;
                else IsShowDescription = true;
            } 
        }
        public Category ParentCategory { 
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
        public Image CategoryImage
        {
            get
            {
                return _categoryImage;
            }
            set
            {
                _categoryImage = value;
                if (value == null) IsShowImage = false;
                else IsShowImage = true;
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
        public bool IsShowImage
        {
            get
            {
                return _isShowImage;
            }
            set
            {
                _isShowImage = value;
                OnPropertyChanged(nameof(IsShowImage));
            }
        }

        public CategoryByIDPageVM(ICategoryService<Category> categoryService)
        {
            _categoryService = categoryService;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
