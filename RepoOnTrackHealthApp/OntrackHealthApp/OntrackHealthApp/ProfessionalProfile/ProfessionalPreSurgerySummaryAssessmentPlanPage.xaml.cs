using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalPreSurgerySummaryAssessmentPlanPage : CustomModalContentPage
    {
        PatientPreSurgerySummaryAssessmentPlanViewModel _patientPreSurgerySummaryAssessmentPlanViewModel;
        List<PatientPreSurgerySummaryAssessmentPlanViewModel> _patientPreSurgerySummaryAssessmentPlanViewModelList;

        public ProfessionalPreSurgerySummaryAssessmentPlanPage(PatientPreSurgerySummaryAssessmentPlanViewModel patientPreSurgerySummaryAssessmentPlanViewModel)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryAssessmentPlanViewModel = patientPreSurgerySummaryAssessmentPlanViewModel;

            if (_patientPreSurgerySummaryAssessmentPlanViewModel != null)
            {
                if (_patientPreSurgerySummaryAssessmentPlanViewModel.AssessmentPlanHtml != null)
                {
                    AssessmentPlanPageWebViewStackLayout.IsVisible = true;
                    AssessmentPlanPageNotFoundStackLayout.IsVisible = false;

                    var htmlWebViewSource = new HtmlWebViewSource
                    {
                        Html = _patientPreSurgerySummaryAssessmentPlanViewModel.AssessmentPlanHtml
                    };
                    AssessmentPlanPageWebView.Source = htmlWebViewSource;
                }
                else
                {
                    AssessmentPlanPageWebViewStackLayout.IsVisible = false;
                    AssessmentPlanPageNotFoundStackLayout.IsVisible = true;
                }
            }
        }

        public ProfessionalPreSurgerySummaryAssessmentPlanPage(List<PatientPreSurgerySummaryAssessmentPlanViewModel> patientPreSurgerySummaryAssessmentPlanViewModelList)
        {
            InitializeComponent();
            this._patientPreSurgerySummaryAssessmentPlanViewModelList = patientPreSurgerySummaryAssessmentPlanViewModelList;
        }

        private async void OnCancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}