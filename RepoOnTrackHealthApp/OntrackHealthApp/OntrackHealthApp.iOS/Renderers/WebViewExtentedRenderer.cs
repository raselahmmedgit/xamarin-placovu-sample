using CoreGraphics;
using OntrackHealthApp.iOS.Renderers;
using OntrackHealthApp.UserControls;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebViewExtented), typeof(WebViewExtentedRenderer))]
namespace OntrackHealthApp.iOS.Renderers
{
    public class WebViewExtentedRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            this.BackgroundColor = UIColor.Clear;
            Delegate = new ExtendedUIWebViewDelegate(this);
        }
    }

    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
        WebViewExtentedRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(WebViewExtentedRenderer _webViewRenderer = null)
        {
            webViewRenderer = _webViewRenderer ?? new WebViewExtentedRenderer();
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            var xwebView = webViewRenderer.Element as WebViewExtented;
            if (xwebView != null && xwebView.AutoContentSize)
            {
                if (xwebView != null)
                {
                    await Task.Delay(300); // wait here till content is rendered
                    xwebView.HeightRequest = (double)webView.ScrollView.ContentSize.Height;
                }
            }
        }

        //private static void SetWebViewExtentedHeight(WebViewExtented xwebView)
        //{
        //    double xwebViewHeightRequest = 0;

        //    var p1 = xwebView.Parent;

        //    if (p1 != null)
        //    {

        //        var p2 = xwebView.Parent.Parent;

        //        if (p2 != null)
        //        {
        //            if (p2.GetType() == typeof(ScrollView))
        //            {
        //                var sv = (ScrollView)p2;

        //                if (sv != null)
        //                {
        //                    xwebViewHeightRequest = (double)sv.ContentSize.Height;
        //                }
        //            }
        //            else
        //            {
        //            }
        //        }
        //        else
        //        {
        //            if (p1.GetType() == typeof(ScrollView))
        //            {
        //                var sv = (ScrollView)p1;

        //                if (sv != null)
        //                {
        //                    xwebViewHeightRequest = (double)sv.ContentSize.Height;
        //                }
        //            }
        //            else
        //            {
        //            }
        //        }
        //    }

        //    if (xwebViewHeightRequest > 0)
        //    {
        //        xwebView.HeightRequest = xwebViewHeightRequest;
        //    }
        //    else
        //    {
        //        xwebView.HeightRequest = 500;
        //    }
        //}
    }
}