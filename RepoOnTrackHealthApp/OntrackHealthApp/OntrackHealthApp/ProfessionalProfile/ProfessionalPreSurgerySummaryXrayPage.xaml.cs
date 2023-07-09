using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalPreSurgerySummaryXrayPage : CustomModalContentPage
    {
        PatientPreSurgerySummaryXrayViewModel _patientPreSurgerySummaryXrayViewModel;
        List<PatientPreSurgerySummaryXrayViewModel> _patientPreSurgerySummaryXrayViewModelList;

        public ProfessionalPreSurgerySummaryXrayPage(PatientPreSurgerySummaryXrayViewModel patientPreSurgerySummaryXrayViewModel)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryXrayViewModel = patientPreSurgerySummaryXrayViewModel;
        }

        public ProfessionalPreSurgerySummaryXrayPage(List<PatientPreSurgerySummaryXrayViewModel> patientPreSurgerySummaryViewModelList)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryXrayViewModelList = patientPreSurgerySummaryViewModelList;

            if (_patientPreSurgerySummaryXrayViewModelList.Any())
            {
                foreach (var patientPreSurgerySummaryXrayViewModel in _patientPreSurgerySummaryXrayViewModelList)
                {
                    StackLayout stackLayout = new StackLayout {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(5, 5, 5, 5),
                        Margin = new Thickness(0, 0, 0, 20)
                    };

                    Image image = new Image
                    {
                        Source = patientPreSurgerySummaryXrayViewModel.XrayImageSource,
                        Aspect = Aspect.Fill,
                        HeightRequest = 400,
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        //Margin = new Thickness(0,10),
                        BackgroundColor = Color.FromHex("#FFFFFF")
                    };

                    stackLayout.Children.Add(image);
                    XrayPageStackLayout.Children.Add(stackLayout);
                }

                XrayPageStackLayout.IsVisible = true;
                XrayPageNotFoundStackLayout.IsVisible = false;
            }
            else
            {
                XrayPageStackLayout.IsVisible = false;
                XrayPageNotFoundStackLayout.IsVisible = true;
            }
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}