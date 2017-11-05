using DesktopApp.Services;
using DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IDisposable MySignalR { get; set; }

        public MainWindow()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel( new CommunicationService());
            this.DataContext = viewModel;
            InitializeComponent();
            this.Loaded += MyWindow_Loaded;
            //this.Closed += Window_Closing;
        }
        //public override void BeginInit()
        //{
        //    JsonTreeView.ItemsSource = null;
        //    JsonTreeView.Items.Clear();
        //    base.BeginInit();
        //}
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            JsonTreeView.ItemsSource = null;
            JsonTreeView.Items.Clear();
        }
        
            private void Window_Closing()
        {
            if (this.MySignalR != null)
            {
                this.MySignalR.Dispose();
                this.MySignalR = null;
            }
        }
    }
}
