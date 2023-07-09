using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
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
    public partial class CancerSummaryPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IProcedureClient _iProcedureClient;
        private readonly IProstateCancerSummaryClient _iProstateCancerSummaryClient;

        private PatientProstateCancerSummary PatientProstateCancerSummary { get; set; }

        public CancerSummaryPage()
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            //Subtitle = _iTokenContainer.CurrentProcedureName;
            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iProcedureClient = new ProcedureClient(apiClient);

            _iProstateCancerSummaryClient = new ProstateCancerSummaryClient(apiClient);

            LoadDataAsync();
        }

        public CancerSummaryPage(PatientProstateCancerSummary patientProstateCancerSummary)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            //Subtitle = _iTokenContainer.CurrentProcedureName;
            ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

            PatientProstateCancerSummary = patientProstateCancerSummary;
        }

        public async void LoadDataAsync()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    var response = await _iProcedureClient.ActiveProcedures();
                    if (response.StatusIsSuccessful)
                    {
                        var responseTwo = await _iProstateCancerSummaryClient.GetProstateCancerSummaryNewAsync(_iTokenContainer.ApiPracticeProfileId.ToLong(), _iTokenContainer.ApiPatientProfileId.ToLong(), _iTokenContainer.CurrentPatientProcedureDetailId);
                        var patientProstateCancerSummary = responseTwo.Data;
                        if (patientProstateCancerSummary != null)
                        {
                            if (patientProstateCancerSummary.PatientProfileId > 0)
                            {
                                PatientProstateCancerSummary = patientProstateCancerSummary;
                            }
                            else
                            {
                                PatientProstateCancerSummary = new PatientProstateCancerSummary();
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        private async void BtnStage_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CancerSummaryStagePage(PatientProstateCancerSummary));
        }

        private async void BtnGrade_ClickedAsync(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new CancerSummaryGradePage(PatientProstateCancerSummary));
        }

        private async void BtnVolume_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CancerSummaryVolumePage(PatientProstateCancerSummary));
        }

        private async void BtnSummary_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CanceSummarySummaryPage(PatientProstateCancerSummary));
        }
    }
}