using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace OntrackHealthApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CancerSummaryVolumePage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }


        public CancerSummaryVolumePage(PatientProstateCancerSummary patientProstateCancerSummary)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            PatientProstateCancerSummary = patientProstateCancerSummary;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (UserDialogs.Instance.Loading(""))
            {
                string str = "";
                if (PatientProstateCancerSummary != null)
                {
                    if (!string.IsNullOrEmpty(PatientProstateCancerSummary.MarginScore) && PatientProstateCancerSummary.MarginScore == "-")
                    {
                        str = "<strong>Your margins are not involved with cancer</strong>";
                    }
                    else if (!string.IsNullOrEmpty(PatientProstateCancerSummary.MarginScore) && PatientProstateCancerSummary.MarginScore == "+")
                    {
                        str = "<strong>Your margins are involved with cancer</strong>";
                    }
                    LblIsInvolved.Text = str;

                    if (PatientProstateCancerSummary.CancerInvolvement != null)
                    {
                        str = "<strong>" + PatientProstateCancerSummary.CancerInvolvement.ToString() + " % of your prostate was replaced by cancer</strong>";
                        LblVolume.Text = str;
                    }
                }
            }
        }
    }
}