using OntrackHealthApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]
namespace OntrackHealthApp.iOS.Renderers
{
    public class CustomNavigationRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBar.ShadowImage = new UIImage();
            NavigationBar.ClipsToBounds = true; // optional
            NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default); // optional
        }

    }
}