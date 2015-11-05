using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
