using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NursePatientInfoPatientView : ContentPage
    {
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
        ObservableCollection<SurgicalConciergePatientViewModel> _surgicalConciergePatientViewModelList = new ObservableCollection<SurgicalConciergePatientViewModel>();

        public NursePatientInfoPatientView(long divisionId)
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

            LoadData();
        }

        public void LoadData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                this.BindingContext = new NursePatientInfoPatientDataService(this);
                _surgicalConciergePatientViewModelList = (this.BindingContext as NursePatientInfoPatientDataService).Items;
            }
        }

        public void ReLoadData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                this.BindingContext = new NursePatientInfoPatientDataService(this, true);
                _surgicalConciergePatientViewModelList = (this.BindingContext as NursePatientInfoPatientDataService).Items;
            }
        }

        private async void LoadPatientList(string searchText = null)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    PatientView.ItemsSource = _surgicalConciergePatientViewModelList;
                }
                else
                {
                    IEnumerable<SurgicalConciergePatientViewModel> patients = _surgicalConciergePatientViewModelList.Where(c => !string.IsNullOrEmpty(c.PatientFullName) && c.PatientFullName.ToLower().StartsWith(searchText.ToLower()));
                    PatientView.ItemsSource = patients;
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void PatientView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                    if (PatientView.SelectedItem == null)
                        return;

                    var selectedPatient = PatientView.SelectedItem as SurgicalConciergePatientViewModel;
                    await Navigation.PushAsync(new NursePatientInfoPatientSurvey(selectedPatient.PatientProcedureDetailId.ToGuid(), selectedPatient));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private void PatientView_Refreshing(object sender, EventArgs e)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                LoadPatientList();
                PatientView.EndRefresh();
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


    }
}