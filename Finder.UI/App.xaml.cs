using Finder.UI.Pages;
using Finder.Core.ViewModels;
using System.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Finder.Core.Services;
using Finder.Core.Interfaces.IServices;
using Finder.Core.Models;

namespace Finder.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();

            container.RegisterType<ISearchService<TaskModel>, SearchService>();

            var mainWindow = container.Resolve<MainWindow>();
            var mainVindowVM = container.Resolve<MainWindowVM>();
            var contentPage = container.Resolve<ContentPage>();
            var initialPage = container.Resolve<InitialPage>();
            mainVindowVM.CurrentView = initialPage;
            mainVindowVM.ContentPage = contentPage;
            mainWindow.DataContext = mainVindowVM;
            mainWindow.Show();
        }
    }
}
