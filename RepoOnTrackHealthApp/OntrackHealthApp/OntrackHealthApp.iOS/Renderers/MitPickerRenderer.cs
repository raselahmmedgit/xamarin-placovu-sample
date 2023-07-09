
using CoreGraphics;
using OntrackHealthApp.IOS.Renderers;
using OntrackHealthApp.UserControls;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MtiPicker), typeof(MtiPickerRenderer))]
namespace OntrackHealthApp.IOS.Renderers
{
    public class MtiPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (MtiPicker)Element;
                if (view.HeightRequest == -1)
                {
                    view.HeightRequest = 40;
                    view.CornerRadius = view.HeightRequest / 2;
                }

                Control.LeftView = new UIView(new CGRect(0, 0, 15, 0));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIView(new CGRect(0, 0, 15, 0));
                Control.RightViewMode = UITextFieldViewMode.Always;

                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;
                // Radius for the curves  
                Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                // Thickness of the Border Color  
                Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                // Thickness of the Border Width  
                Control.Layer.BorderWidth = view.BorderWidth;
                Control.ClipsToBounds = true;
            }
        }

    }
}
