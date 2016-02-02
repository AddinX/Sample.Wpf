using System.Windows;
using System.Windows.Controls;
using WPFSample.WPF.ViewModel;

namespace WPFSample.WPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        public MainWindow(MainWindowViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
