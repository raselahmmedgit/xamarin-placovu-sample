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
	public partial class CanceSummarySummaryPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }


        public CanceSummarySummaryPage(PatientProstateCancerSummary patientProstateCancerSummary)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            PatientProstateCancerSummary = patientProstateCancerSummary;
        }
        private void BindForm()
        {
            string str = "<html>"
                + "<head><meta charset=\"utf-8\" /><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"></head>"
                + "<body><p style='font-size:20px'>Your final pathology report:</p>";
            str += "<p><ul>";
            string stage = "Negative";
            if (PatientProstateCancerSummary != null)
            {
                stage = !string.IsNullOrEmpty(PatientProstateCancerSummary.StageScore) ? "Negative" : PatientProstateCancerSummary.StageScore;
            }
            str += "<li style='font-size:18px'>Stage: " + stage + "</li>";
            string grade = "N/A";
            if (PatientProstateCancerSummary != null)
            {
                grade = !string.IsNullOrEmpty(PatientProstateCancerSummary.GleasonScore) ? "N/A" : PatientProstateCancerSummary.GleasonScore;
            }
            str += "<li style='font-size:18px'>Grade: " + grade + "</li>";

            string cancerMargins = "N/A";
            if (PatientProstateCancerSummary != null)
            {
                if (PatientProstateCancerSummary != null && !string.IsNullOrEmpty(PatientProstateCancerSummary.MarginScore))
                {
                    if (PatientProstateCancerSummary.MarginScore == "-")
                    {
                        cancerMargins = "Nagative";
                    }
                    if (PatientProstateCancerSummary.MarginScore == "+")
                    {
                        cancerMargins = "Positive";
                    }
                }
            }
            
            str += "<li style='font-size:18px'>Margins: " + cancerMargins + "</li>";
            string cancerVolume = "N/A";
            if(PatientProstateCancerSummary != null)
            {
                cancerVolume = PatientProstateCancerSummary != null && PatientProstateCancerSummary.CancerInvolvement == null ? " N/A " : PatientProstateCancerSummary.CancerInvolvement.ToString() + "%";
            }
            str += "<li style='font-size:18px'>Cancer volume: " + cancerVolume + "</li>";

            str += "</ul></p>";

            str += "<p style='font-size:20px'>";
            str += "Here is an estimate of your prostate cancer recurrence and survival rate:";
            str += "</p>";

            str += "<p><ul>";
            if(PatientProstateCancerSummary != null)
            {
                str += "<li style='font-size:18px'>5 year recurrence free survival: " + (PatientProstateCancerSummary != null && PatientProstateCancerSummary.FiveYearsFactorScore != null ? PatientProstateCancerSummary.FiveYearsFactorScore : "N/A") + " </li>";
                str += "<li style='font-size:18px'>10 year prostate cancer free survival: " + (PatientProstateCancerSummary != null && PatientProstateCancerSummary.TenYearsFactorScore != null ? PatientProstateCancerSummary.TenYearsFactorScore : "N/A") + "</li>";
            }
            else
            {
                str += "<li style='font-size:18px'>5 year recurrence free survival: " + "N/A" + " </li>";
                str += "<li style='font-size:18px'>10 year prostate cancer free survival: " +  "N/A" + "</li>";
            }
            str += "</ul></p></body></html>";

            HtmlWebViewSource source = new HtmlWebViewSource();
            source.Html = str;
            WebViewExtBlockOne.Source = source;
        }
        protected override void OnAppearing()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                base.OnAppearing();
                BindForm();
            }
        }
    }
}