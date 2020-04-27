using Motopark.Core.Entities;
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
    public partial class CategoryByIDPage : ContentPage
    {
        public CategoryByIDPage()
        {
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            if (BindingContext is CategoryByIDPageVM vm)
            {
                if (vm.ImageStream != null)
                    vm.ImageStream.Dispose();
            }
        }
    }
}