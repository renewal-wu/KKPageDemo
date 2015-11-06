using Windows.UI.Xaml.Controls;

namespace KKPageDemo.Controls
{
    public class KKPageBase : ContentControl
    {
        public KKPageBase()
        {
            this.DefaultStyleKey = typeof(KKPageBase);
        }

        public string Header { get; set; } = string.Empty;
    }
}