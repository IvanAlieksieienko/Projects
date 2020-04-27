using Nancy.TinyIoc;
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
    public partial class MainPage : MasterDetailPage
    {
        private IUnityContainer _unityContainer;
        public MainPage(IUnityContainer unityContainer)
        {
            InitializeComponent();
            _unityContainer = unityContainer;
            masterPage.listView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SideBarItem;
            if (item != null)
            {
                var page = (Page)_unityContainer.Resolve(item.TargetType);
                Detail = new NavigationPage(page);
                masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}