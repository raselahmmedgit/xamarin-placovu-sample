//using Android.Content;
//using OntrackHealthApp.Droid;
//using OntrackHealthApp.UserControls;
//using System;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
///*
//    Search: Add-gradient-background-to-layouts-in-xamarin-forms-visual-studio
//*/
//[assembly: ExportRenderer(typeof(GradiantFrame), typeof(GradiantFrameRenderer))]
//namespace OntrackHealthApp.Droid
//{
//    public class GradiantFrameRenderer : VisualElementRenderer<GradiantFrame>
//    {
//        internal string ColorsString { get; set; }
//        internal Color[] GradiantColors
//        {
//            get
//            {
//                string[] hex = ColorsString.Split(',');
//                Color[] colors = new Color[hex.Length];

//                for (int i = 0; i < hex.Length; i++)
//                {
//                    colors[i] = Color.FromHex(hex[i].Trim());
//                }

//                return colors;
//            }
//        }
//        private GradientColorStackMode Mode { get; set; }

//        public GradiantFrameRenderer(Context ctx) : base(ctx) { }

//        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
//        {
//            Android.Graphics.LinearGradient gradient;

//            int[] colors = new int[GradiantColors.Length];

//            for (int i = 0, l = GradiantColors.Length; i < l; i++)
//            {
//                colors[i] = GradiantColors[i].ToAndroid().ToArgb();
//            }

//            switch (Mode)
//            {
//                default:
//                case GradientColorStackMode.ToRight:
//                    gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToLeft:
//                    gradient = new Android.Graphics.LinearGradient(Width, 0, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToTop:
//                    gradient = new Android.Graphics.LinearGradient(0, Height, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToBottom:
//                    gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToTopLeft:
//                    gradient = new Android.Graphics.LinearGradient(Width, Height, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToTopRight:
//                    gradient = new Android.Graphics.LinearGradient(0, Height, Width, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToBottomLeft:
//                    gradient = new Android.Graphics.LinearGradient(Width, 0, 0, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//                case GradientColorStackMode.ToBottomRight:
//                    gradient = new Android.Graphics.LinearGradient(0, 0, Width, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
//                    break;
//            }

//            var paint = new Android.Graphics.Paint()
//            {
//                Dither = true,
//            };

//            paint.SetShader(gradient);
//            canvas.DrawPaint(paint);

//            base.DispatchDraw(canvas);
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<GradiantFrame> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null || Element == null)
//                return;

//            try
//            {
//                if (e.NewElement is GradiantFrame layout)
//                {
//                    ColorsString = layout.GradiantColors;
//                    Mode = layout.GradiantMode;
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
//            }
//        }
//    }
//}



using CoreAnimation;
using CoreGraphics;
using OntrackHealthApp.iOS.Renderers;
using OntrackHealthApp.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradiantFrame), typeof(GradiantFrameRenderer))]

namespace OntrackHealthApp.iOS.Renderers
{
    public class GradiantFrameRenderer : VisualElementRenderer<GradiantFrame>
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
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradiantFrame layout = (GradiantFrame)Element;

            CGColor[] colors = new CGColor[GradiantColors.Length];

            for (int i = 0, l = colors.Length; i < l; i++)
            {
                colors[i] = GradiantColors[i].ToCGColor();
            }

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

            gradientLayer.Frame = rect;
            gradientLayer.Colors = colors;

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}

