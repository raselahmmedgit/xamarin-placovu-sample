using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using OntrackHealthApp.iOS.Renderers;
using OntrackHealthApp.UserControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlButton), typeof(HtmlButtonRenderer))]
namespace OntrackHealthApp.iOS.Renderers
{
    public class HtmlButtonRenderer:ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
            {
                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                var myHtmlData = NSData.FromString(Element.Text, NSStringEncoding.Unicode);
                Control.TitleLabel.Lines = 0;
                Control.SetAttributedTitle(new NSAttributedString(myHtmlData, attr, ref nsError),UIControlState.Normal);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Button.TextProperty.PropertyName)
            {
                if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
                {
                    var attr = new NSAttributedStringDocumentAttributes();
                    var nsError = new NSError();
                    attr.DocumentType = NSDocumentType.HTML;

                    var myHtmlData = NSData.FromString(Element.Text, NSStringEncoding.Unicode);
                    Control.TitleLabel.Lines = 0;
                    Control.SetAttributedTitle(new NSAttributedString(myHtmlData, attr, ref nsError), UIControlState.Normal);
                }
            }
        }
    }
}