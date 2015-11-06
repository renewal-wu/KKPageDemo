using Windows.UI.Xaml.Controls;

namespace KKPageDemo.Controls
{
    public class KKPage : ContentControl
    {
        public KKPage()
        {
            this.DefaultStyleKey = typeof(KKPage);
        }

        public string Header { get; set; } = string.Empty;
    }
}