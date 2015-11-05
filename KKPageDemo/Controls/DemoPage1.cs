using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKPageDemo.Controls
{
    public class DemoPage1 : KKPage
    {
        // 假設是 ListView 樣式的頁面

        public DemoPage1()
        {
            this.DefaultStyleKey = typeof(DemoPage1);
            this.Loaded += DemoPage1_Loaded;
            this.Unloaded += DemoPage1_Unloaded;
            this.Header = "ListViewPage Demo";
        }

        private void DemoPage1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            object parameter = this.DataContext;
            // 在這邊還原 ViewModel
        }

        private void DemoPage1_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 在這邊清除 ViewModel
        }
    }
}
