
using CoreAnimation;
using CoreGraphics;
using OntrackHealthApp.iOS.Renderers;
using OntrackHealthApp.UserControls;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(GradiantStackLayout), typeof(GradiantStackLayoutRenderer))]

namespace OntrackHealthApp.iOS.Renderers
{
    public class GradiantStackLayoutRenderer : VisualElementRenderer<GradiantStackLayout>
    {
        internal string ColorsString { get; set; }

        internal Color[] GradiantColors
        {
            get
            {
                string[] hex = ColorsString.Split(',');
                Color[] colors = new Color[hex.Length];

                for (int i = 0; i < hex.Length; i++)
                {
                    colors[i] = Color.FromHex(hex[i].Trim());
                }

                return colors;
            }
        }

        private GradientColorStackMode Mode { get; set; }
        //public GradiantStackLayoutRenderer(Context ctx) : base(ctx) { }

        //public static UIColor ToUIColor(string hexString)
        //{
        //    hexString = hexString.Replace("#", "");

        //    if (hexString.Length == 3)
        //        hexString = hexString + hexString;

        //    if (hexString.Length != 6)
        //        throw new Exception("Invalid hex string");

        //    int red = Int32.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        //    int green = Int32.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        //    int blue = Int32.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

        //    return UIColor.FromRGB(red, green, blue);
        //}

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradiantStackLayout layout = (GradiantStackLayout)Element;
            if (layout != null)
            {
                ColorsString = layout.GradiantColors;
                CGColor[] colors = new CGColor[GradiantColors.Length];

                for (int i = 0, l = colors.Length; i < l; i++)
                {
                    colors[i] = GradiantColors[i].ToCGColor();
                }
                Mode = layout.GradiantMode;
                var gradientLayer = new CAGradientLayer();

                switch (Mode)
                {
                    default:
                    case GradientColorStackMode.ToRight:
                        gradientLayer.StartPoint = new CGPoint(0, 0.5);
                        gradientLayer.EndPoint = new CGPoint(1, 0.5);
                        break;
                    case GradientColorStackMode.ToLeft:
                        gradientLayer.StartPoint = new CGPoint(1, 0.5);
                        gradientLayer.EndPoint = new CGPoint(0, 0.5);
                        break;
                    case GradientColorStackMode.ToTop:
                        gradientLayer.StartPoint = new CGPoint(0.5, 0);
                        gradientLayer.EndPoint = new CGPoint(0.5, 1);
                        break;
                    case GradientColorStackMode.ToBottom:
                        gradientLayer.StartPoint = new CGPoint(0.5, 1);
                        gradientLayer.EndPoint = new CGPoint(0.5, 0);
                        break;
                    case GradientColorStackMode.ToTopLeft:
                        gradientLayer.StartPoint = new CGPoint(1, 0);
                        gradientLayer.EndPoint = new CGPoint(0, 1);
                        break;
                    case GradientColorStackMode.ToTopRight:
                        gradientLayer.StartPoint = new CGPoint(0, 1);
                        gradientLayer.EndPoint = new CGPoint(1, 0);
                        break;
                    case GradientColorStackMode.ToBottomLeft:
                        gradientLayer.StartPoint = new CGPoint(1, 1);
                        gradientLayer.EndPoint = new CGPoint(0, 0);
                        break;
                    case GradientColorStackMode.ToBottomRight:
                        gradientLayer.StartPoint = new CGPoint(0, 0);
                        gradientLayer.EndPoint = new CGPoint(1, 1);
                        break;
                }

                float cornerRadius = Element.CornerRadius;
                if (cornerRadius != -1f)
                {
                    gradientLayer.CornerRadius = cornerRadius;
                    NativeView.Layer.CornerRadius = cornerRadius;
                }
                if (Element.HasShadow)
                {
                    NativeView.Layer.ShadowRadius = 5;
                    NativeView.Layer.ShadowColor = UIColor.Black.CGColor;
                    NativeView.Layer.ShadowOpacity = 0.8f;
                    NativeView.Layer.ShadowOffset = new SizeF();
                }
                else
                {
                    gradientLayer.ShadowOpacity = 0;
                }

                gradientLayer.Frame = rect;
                gradientLayer.Colors = colors;
                NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }

        }
    }
}

