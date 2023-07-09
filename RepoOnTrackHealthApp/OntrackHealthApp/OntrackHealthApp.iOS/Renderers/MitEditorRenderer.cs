using OntrackHealthApp.Droid.Renderers;
using OntrackHealthApp.UserControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(MtiEditor), typeof(MitEditorRenderer))]
namespace OntrackHealthApp.Droid.Renderers
{
    public class MitEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Element != null && Control != null)
            {
                Control.TextContainerInset = new UIEdgeInsets(10f, 10f, 10f, 10f);
                Control.Layer.CornerRadius = 10f;
                Control.Layer.BorderWidth = .5f;
                Control.Layer.BorderColor = UIColor.FromRGB(200, 200, 200).CGColor;
            }
        }
    }
}
