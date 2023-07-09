using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
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
    public partial class CancerSummaryGradePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProstateCancerSummaryClient _iProstateCancerSummaryClient;
        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }
        private long PracticeProfileId { get; set; }
        private long PatientProfileId { get; set; }

        public CancerSummaryGradePage(PatientProstateCancerSummary patientProstateCancerSummary)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            App.Instance.ChangeTitle("ssssssssssssssss");

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProstateCancerSummaryClient = new ProstateCancerSummaryClient(apiClient);

            this.PracticeProfileId = _iTokenContainer.ApiPracticeProfileId.ToLong();
            this.PatientProfileId = _iTokenContainer.ApiPatientProfileId.ToLong();
            PatientProstateCancerSummary = patientProstateCancerSummary;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //var response = await _iProstateCancerSummaryClient.GetProstateCancerSummaryAsync(PracticeProfileId, PatientProfileId);
            //PatientProstateCancerSummary = response.Data;
            using (UserDialogs.Instance.Loading(""))
            {
                string str = "<div style='font-size:22px'><p>Grade describes how aggressive the cancer cells look under the microscope. Traditionally, prostate cancers were graded using the Gleason grading system on a scale of 1 to 5. Gleason 1 cancers tend to grow very slowly whereas Gleason grade 5 cancers are more aggressive and grow rapidly.</p>";
                str += "<p>Often there is more than one grade in a given specimen. The most dominant grade seen under the microscope will be listed first and the next most dominant grade in the specimen listed second. For example, a cancer specimen may have a dominant grade of 3 and a second most dominant grade of 3. The Gleason Score would therefore be a 3 + 3 = 6.</p>";
                str += "<p>Today, pathologist agree that Gleason Grade 1 and 2 cancers do not exist. Therefore the lowest grade seen on a cancer specimen is a Gleason grade 3.</p></div>";

                LblCancerSummeryGradeOne.Text = str;

                string strYourGrade = string.Empty;
                string strYourGradeResult = string.Empty;

                if (PatientProstateCancerSummary != null && PatientProstateCancerSummary.GleasonScoreOne != null && PatientProstateCancerSummary.GleasonScoreTwo != null)
                {
                    strYourGrade = "<p>Your Gleason Score is " + PatientProstateCancerSummary.GleasonScoreOne.ToString() + " + " + PatientProstateCancerSummary.GleasonScoreTwo.ToString() + " = " + (PatientProstateCancerSummary.GleasonScoreOne + PatientProstateCancerSummary.GleasonScoreTwo).ToString() + "</p>";
                    strYourGradeResult = (PatientProstateCancerSummary.GleasonScoreOne + PatientProstateCancerSummary.GleasonScoreTwo).ToString();
                }
                else
                {
                    strYourGrade = "<p>Your Gleason Score is  N/A </p>";
                    strYourGradeResult = "0";
                }

                LblCancerSummeryGradeResult.Text = strYourGrade;
                LblResult.Text = strYourGradeResult;
                if (PatientProstateCancerSummary != null)
                {
                    if (PatientProstateCancerSummary.GleasonScoreOne == 3)
                    {
                        LblOneThree.BackgroundColor = Color.FromHex("#e83555");
                    }
                    if (PatientProstateCancerSummary.GleasonScoreOne == 4)
                    {
                        LblOneFour.BackgroundColor = Color.FromHex("#e83555");
                    }
                    if (PatientProstateCancerSummary.GleasonScoreOne == 5)
                    {
                        LblOneFive.BackgroundColor = Color.FromHex("#e83555");
                    }

                    if (PatientProstateCancerSummary.GleasonScoreTwo == 3)
                    {
                        LblTwoThree.BackgroundColor = Color.FromHex("#e83555");
                    }
                    if (PatientProstateCancerSummary.GleasonScoreTwo == 4)
                    {
                        LblTwoFour.BackgroundColor = Color.FromHex("#e83555");
                    }
                    if (PatientProstateCancerSummary.GleasonScoreTwo == 5)
                    {
                        LblTwoFive.BackgroundColor = Color.FromHex("#e83555");
                    }
                }
            }
        }
    }
}