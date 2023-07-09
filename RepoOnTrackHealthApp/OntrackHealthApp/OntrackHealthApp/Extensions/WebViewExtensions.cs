//using Android.Content;
//using Android.Runtime;
using OntrackHealthApp.UserControls;
using System;
using Xamarin.Forms;

namespace OntrackHealthApp.Extensions
{
    public static class WebViewExtensions
    {
        public static void ResizeToContentAddToLayout(this WebViewExtented webView, StackLayout parentLayout, string htmlCode)
        {
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = htmlCode;
            webView.Source = htmlSource;
            webView.BackgroundColor = Color.Transparent;
        }
    }

    //public class DynamicSizeWebView : Android.Webkit.WebView
    //{
    //    public EventHandler SizeChanged;

    //    bool _observeSizeChanges;
    //    public bool ObserveSizeChanges
    //    {
    //        get => _observeSizeChanges;
    //        set
    //        {
    //            if (_observeSizeChanges != value)
    //            {
    //                _observeSizeChanges = value;
    //                if (_observeSizeChanges)
    //                {
    //                    OnSizeChange();
    //                }
    //            }
    //        }
    //    }

    //    public DynamicSizeWebView(Context context) : base(context) { }

    //    protected DynamicSizeWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

    //    int _previousMesuredHeight = 0;

    //    public override void Invalidate()
    //    {
    //        base.Invalidate();
    //        if (ObserveSizeChanges)
    //        {
    //            OnSizeChange();
    //        }
    //    }

    //    void OnSizeChange()
    //    {
    //        var newHeight = ContentHeight;
    //        if (newHeight > 0 && _previousMesuredHeight != newHeight)
    //        {
    //            SizeChanged?.Invoke(this, EventArgs.Empty);
    //            _previousMesuredHeight = newHeight;
    //        }
    //    }
    //}
}
