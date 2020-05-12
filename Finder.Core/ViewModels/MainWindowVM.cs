using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;
using Finder.Core.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Unity;
using Finder.Core.Models;
using System.Windows;

namespace Finder.Core.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private IUnityContainer _container;
        public ICommand FileNavigationCommand { get; set; }
        public ICommand ContentNavigationCommand { get; set; }
        public ICommand RegExNavigationCommand { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        public UserControl ContentPage { get; set; }


        public MainWindowVM(IUnityContainer container)
        {
            _container = container;
            FileNavigationCommand = new Command(FileNavigationMethod);
            ContentNavigationCommand = new Command(ContentNavigationMethod);
            RegExNavigationCommand = new Command(RegExNavigationMethod);
        }

        private async void FileNavigationMethod(object sender)
        {
            var contentPageVM = _container.Resolve<ContentPageVM>();
            contentPageVM.CreateNewTask(SearchMethod.File);
            ContentPage.DataContext = contentPageVM;
            CurrentView = ContentPage;
        }

        private async void ContentNavigationMethod(object sender)
        {
            var contentPageVM = _container.Resolve<ContentPageVM>();
            contentPageVM.CreateNewTask(SearchMethod.Content);
            ContentPage.DataContext = contentPageVM;
            CurrentView = ContentPage;
        }

        private async void RegExNavigationMethod(object sender)
        {
            var contentPageVM = _container.Resolve<ContentPageVM>();
            contentPageVM.CreateNewTask(SearchMethod.RegEx);
            ContentPage.DataContext = contentPageVM;
            CurrentView = ContentPage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
