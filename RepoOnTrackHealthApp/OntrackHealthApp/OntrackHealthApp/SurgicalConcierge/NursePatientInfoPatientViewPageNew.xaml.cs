using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using OntrackHealthApp.UserControls;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NursePatientInfoPatientViewPageNew : ContentPage
    {
        public long PracticeDivisionDest = 0;
        public long PracticeDivisionUnitDest = 0;
        public string PracticeName { get; set; }
        public string ProfessionalName { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string DateofBirth { get; set; }
        public string PatientPhoneCode { get; set; }
        public string PatientPhone { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public DateTime? SelectedDate { get; set; }
        public DateTime? PastDay { get; set; }

        public string SelectedPracticeProfile { get; set; }
        public string SelectedProfessionalProfile { get; set; }
        public string SelectedProcedure { get; set; }

        public readonly ITokenContainer _iTokenContainer;
        private NursePatientInfoPatientViewPageViewModel PageViewModel;

        public NursePatientInfoPatientViewPageNew(long divisionId)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }

            PastDay = DateTime.Now;
            PracticeDivisionDest = divisionId;
            BindingContext = PageViewModel = new NursePatientInfoPatientViewPageViewModel(this);
            ReLoadData();
        }

        public NursePatientInfoPatientViewPageNew(long divisionId, DateTime? pastDay)
        {
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;

            if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
                || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
            {
                Title = _iTokenContainer.ApiPracticeLocationName;
            }
            else
            {
                Title = _iTokenContainer.ApiPracticeName;
            }

            //SelectedDate = pastDay;
            PastDay = pastDay;

            PracticeDivisionDest = divisionId;
            BindingContext = PageViewModel = new NursePatientInfoPatientViewPageViewModel(this);
            ReLoadData();
        }

        public void ReLoadData()
        {
            try
            {
                PageViewModel.ReDownloadSurgicalConciergePatientViews();
                PatientView.ItemsSource = PageViewModel.SurgicalConciergePatientViewModeslInfiniteScroll;
                PageViewModel.IsBusy = false;
            }
            catch { }
        }

        private async void ShowPatientSearchFilterModal(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }

                await Navigation.PushModalAsync(new NursePatientSearchFilterView(this));
            }
        }

        private async void ShowPatientSearchModal(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (!InternetConnectHelper.CheckConnection())
                {
                    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                    return;
                }

                await Navigation.PushModalAsync(new NursePatientSearchView(this));
            }
        }

        private async void ShowPatientCalendarSearchModal(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }

            await Navigation.PushModalAsync(new PatientCalendarSearchModal(this));
        }

        private async void ButtonDetailExtended_Clicked(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    var button = (ButtonExtended)sender;

                    if (button.SelectedDataItem == null)
                        return;

                    var selectedSurgicalConciergePatientViewModel = button.SelectedDataItem as SurgicalConciergePatientViewModel;

                    await Navigation.PushAsync(new NursePatientInfoPatientSurvey(selectedSurgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid(), selectedSurgicalConciergePatientViewModel));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


    }
}