using Android.Content;
using Android.OS;
using Android.Text;
using Android.Widget;
using OntrackHealthApp.Droid;
using OntrackHealthApp.UserControls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace OntrackHealthApp.Droid
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        public HtmlLabelRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);            
            var view = (HtmlLabel)Element;
            string txt = view.Text == null ? "" : view.Text.ToString();
            if (view == null) return;
            Control.SetText(FromHtml(txt), TextView.BufferType.Spannable);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Control?.SetText(FromHtml(Element.Text), TextView.BufferType.Spannable);
            }
        }

        public static ISpanned FromHtml(string source)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.N)
            {
                // noinspection deprecation
                return Html.FromHtml(source);
            }
            return Html.FromHtml(source, FromHtmlOptions.ModeLegacy);
        }
    }
}