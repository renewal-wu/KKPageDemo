using KKPageDemo.Controls;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace KKPageDemo.ViewModels
{
    public class FakeSongNameListViewModel
    {
        public List<string> Items { get; set; } = new List<string>();

        public Color BackgroundColor { get; set; } = Colors.White;

        public FakeSongNameListViewModel()
        {
            for (int i = 0; i < 100; i++)
            {
                Items.Add(i.ToString());
            }
        }

        public async void OnItemClick(object sender, ItemClickEventArgs e)
        {
            await KKPageView.CurrentKKPageView.NavigateTo<DemoPage1>(this, new FakeSongNameListViewModel()
            {
                BackgroundColor = ColorHelper.GetRandomColor()
            }, false);
        }
    }
}
