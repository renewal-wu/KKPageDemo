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
            #region Memory test
            for (int i = 0; i < 100; i++)
            {
                await MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
                {
                    BackgroundColor = Colors.Yellow
                }, false);
            }

            for (int i = 0; i < 100; i++)
            {
                await MainKKPageView.GoBack();
            }

            MainKKPageView.ClearHistory();

            GC.Collect();
            #endregion

            //MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //{
            //    BackgroundColor = Colors.Yellow
            //}, false);

            //Task.Delay(TimeSpan.FromSeconds(4)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //    {
            //        BackgroundColor = Colors.Red
            //    }, false));
            //});

            //Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //    {
            //        MainKKPageView.SetPrimaryPageWidth(470);

            //        MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //        {
            //            BackgroundColor = Colors.Green
            //        }, false);
            //    });
            //});

            //Task.Delay(TimeSpan.FromSeconds(6)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //    {
            //        BackgroundColor = Colors.Blue
            //    }, false));
            //});

            //Task.Delay(TimeSpan.FromSeconds(8)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MainKKPageView.GoBack());
            //});

            //Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MainKKPageView.GoBack());
            //});

            //Task.Delay(TimeSpan.FromSeconds(12)).ContinueWith(t =>
            //{
            //    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => MainKKPageView.NavigateTo<DemoPage1>(null, new FakeSongNameListViewModel()
            //    {
            //        BackgroundColor = Colors.Brown
            //    }, true));
            //});
        }
    }
}
