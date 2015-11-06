using KKPageDemo.ViewModels;
using System.ComponentModel;

namespace KKPageDemo.Controls
{
    public partial class DemoPage1 : KKPage
    {
        // 假設是 ListView 樣式的頁面

        public FakeSongNameListViewModel ViewModel { get; set; }

        public DemoPage1()
        {
            InitializeComponent();

            this.Loaded += DemoPage1_Loaded;
            this.Unloaded += DemoPage1_Unloaded;
            this.DataContextChanged += DemoPage1_DataContextChanged;
            this.Header = "ListViewPage Demo";
        }

        private void DemoPage1_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            // for x:Bind 
            ViewModel = this.DataContext as FakeSongNameListViewModel;
        }

        private void DemoPage1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void DemoPage1_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
