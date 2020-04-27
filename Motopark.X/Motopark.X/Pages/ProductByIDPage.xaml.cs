using Motopark.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motopark.X.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductByIDPage : ContentPage
    {
        public ProductByIDPage()
        {
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            if (BindingContext is ProductByIDPageVM vm)
            {
                if (vm.ImageStreams.Count > 0)
                {
                    vm.ImageStreams.ForEach(async (Stream stream) =>
                    {
                        if (stream != null)
                            stream.Dispose();
                    });
                }
            }
        }
    }
}