using Windows.UI.Xaml.Controls;

namespace KKPageDemo.Controls
{
    public abstract class KKPage : Control
    {
        public KKPage()
        {
            this.DefaultStyleKey = typeof(KKPage);
        }

        public string Header { get; set; } = string.Empty;
    }
}