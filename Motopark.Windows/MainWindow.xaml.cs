using Xamarin.Forms.Platform.WPF;
using Xamarin.Forms;

namespace Motopark.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            Xamarin.Forms.Forms.Init();

            LoadApplication(new Motopark.X.App());
        }
    }
}
