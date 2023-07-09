using Xamarin.Forms;


namespace OntrackHealthApp.UserControls
{
    public class WebViewExtented : WebView
    {
        public static readonly BindableProperty AutoContentSizeProperty = BindableProperty.Create(nameof(AutoContentSize), typeof(bool), typeof(WebViewExtented), true);
        public bool AutoContentSize
        {
            get => (bool)GetValue(AutoContentSizeProperty);
            set => SetValue(AutoContentSizeProperty, value);
        }
    }
}
