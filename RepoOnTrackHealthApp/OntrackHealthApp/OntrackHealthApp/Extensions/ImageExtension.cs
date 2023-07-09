using System;
using Xamarin.Forms;

namespace OntrackHealthApp.Extensions
{
    public static class ImageExtension
    {
        public static string ToProperImageSource(this string imageSource)
        {
           
            string imagePath;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    imagePath = imageSource;
                    break;
                case Device.iOS:
                    imagePath = imageSource;
                    break;
                case Device.UWP:
                    imagePath = "Images/" + imageSource;
                    break;
                case Device.WPF:
                    imagePath = "Images/" + imageSource;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return imagePath;
        }
    }
}
