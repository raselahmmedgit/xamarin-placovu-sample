using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomModalContentPage : ContentPage
	{
		public CustomModalContentPage ()
		{
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FullScreen);
            InitializeComponent ();
		}
        public static readonly BindableProperty FormattedTitleProperty = BindableProperty.Create(nameof(FormattedTitle), typeof(FormattedString), typeof(CustomContentPage), null);

        public FormattedString FormattedTitle
        {
            get { return (FormattedString)GetValue(FormattedTitleProperty); }
            set
            {
                SetValue(FormattedTitleProperty, value);
            }
        }

        public static readonly BindableProperty FormattedSubtitleProperty = BindableProperty.Create(nameof(FormattedSubtitle), typeof(FormattedString), typeof(CustomContentPage), null);

        public FormattedString FormattedSubtitle
        {
            get { return (FormattedString)GetValue(FormattedSubtitleProperty); }
            set
            {
                SetValue(FormattedSubtitleProperty, value);
            }
        }

        public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(CustomContentPage), null);


        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set
            {
                SetValue(SubtitleProperty, value);
            }
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
        }
	}
}