using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    [ContentProperty("SourceImage")]
    public class PlatformImageExtension : IMarkupExtension<string>
    {
        public string SourceImage { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (SourceImage == null)
                return null;

            string imagePath;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    imagePath = SourceImage;
                    break;
                case Device.iOS:
                    imagePath = SourceImage;
                    break;
                case Device.UWP:
                    imagePath = "Images/" + SourceImage;
                    break;
                case Device.WPF:
                    imagePath = "Images/" + SourceImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return imagePath;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}
