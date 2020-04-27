using SERP.Core.Services;
using SERP.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;

namespace SERP.Desktop
{
    public partial class MainWindow : Window
    {
        [Dependency]
        public SerpViewModel ViewModel
        {
            set { DataContext = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
