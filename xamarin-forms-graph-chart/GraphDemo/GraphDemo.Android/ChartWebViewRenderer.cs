using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GraphDemo.Chart;
using GraphDemo.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ChartWebView), typeof(ChartWebViewRenderer))]
namespace GraphDemo.Droid
{
    public class ChartWebViewRenderer: WebViewRenderer
    {
        public ChartWebViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
        }
        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }
    }
}