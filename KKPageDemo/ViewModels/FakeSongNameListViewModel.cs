using System.Collections.Generic;
using Windows.UI;

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

        ~FakeSongNameListViewModel()
        {
            Items.Clear();
            Items = null;
        }
    }
}
