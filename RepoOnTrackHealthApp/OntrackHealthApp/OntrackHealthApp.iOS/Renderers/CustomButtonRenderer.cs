using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using OntrackHealthApp.iOS;
using OntrackHealthApp.UserControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ButtonMultiLine), typeof(CustomButtonRenderer))]
namespace OntrackHealthApp.iOS
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.Lines = 0;
                Control.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}