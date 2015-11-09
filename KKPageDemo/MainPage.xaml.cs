using KKPageDemo.Controls;
using KKPageDemo.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace KKPageDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //#region Memory test
            //for (int i = 0; i < 100; i++)
            //{
            //    await MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //    {
            //        BackgroundColor = Colors.Yellow
            //    }, false);
            //}

            //for (int i = 0; i < 100; i++)
            //{
            //    await MainKKPageView.GoBack();
            //}

            //MainKKPageView.ClearHistory();

            //GC.Collect();
            //#endregion

            MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            {
                BackgroundColor = Colors.Yellow
            }, false);
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            await MainKKPageView.GoBack();
        }

        private void MainKKPageView_KKPageNavigated(object sender, EventArgs e)
        {

        }

        private void MainKKPageView_BackEnableChanged(object sender, bool e)
        {
            BackButton.IsEnabled = e;
        }
    }
}
