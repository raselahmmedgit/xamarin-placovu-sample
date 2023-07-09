using Android.Content;
using OntrackHealthApp.Droid.Renderers;
using OntrackHealthApp.UserControls;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebViewExtented), typeof(WebViewExtentedAndroidRenderer))]
namespace OntrackHealthApp.Droid.Renderers
{
    public class WebViewExtentedAndroidRenderer : WebViewRenderer
    {
        public WebViewExtentedAndroidRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
            Control.SetWebViewClient(new ExtendedWebViewClient(Element as WebViewExtented));
           
        }

        public class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            WebViewExtented xwebView;
            public ExtendedWebViewClient(WebViewExtented webView)
            {
                xwebView = webView;
            }

            public override async void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                if (xwebView != null)
                {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
                    await Task.Delay(50);
                    xwebView.HeightRequest = view.ContentHeight;
                    var parentStackLayout =  xwebView.Parent as StackLayout;
                    if (parentStackLayout != null && parentStackLayout.GetType() == typeof(StackLayout))
                    {
                        parentStackLayout.IsVisible = true;
                    }
                    //(xwebView.Parent.Parent as ViewCell).ForceUpdateSize();
                }
               
                base.OnPageFinished(view, url);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {

            base.OnElementChanged(e);
            Control.SetBackgroundColor(new Android.Graphics.Color(255, 255, 255, 0));

            if (e.OldElement == null)
            {
                Control.SetWebViewClient(new ExtendedWebViewClient(e.NewElement as WebViewExtented));
                var nativeWebView = Control;
                nativeWebView.Settings.JavaScriptEnabled = true;
            }
        }
    }
}