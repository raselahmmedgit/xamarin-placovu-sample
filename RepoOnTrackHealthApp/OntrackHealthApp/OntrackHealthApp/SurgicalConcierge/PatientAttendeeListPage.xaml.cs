using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
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
    public partial class PatientAttendeeListPage : ContentPage
    {
        ObservableCollection<SurgicalConciergePatientViewModel> patientList = new ObservableCollection<SurgicalConciergePatientViewModel>();
        IEnumerable<PatientAttendeeProfileViewModel> patientAttendeeList = new ObservableCollection<PatientAttendeeProfileViewModel>();
        SurgicalConciergeViewModel surgicalConciergeViewModel;
        private readonly ITokenContainer _iTokenContainer;

        public PatientAttendeeListPage(SurgicalConciergeViewModel surgicalConciergeViewModel = null)
        {
            _iTokenContainer = new TokenContainer();
            InitializeComponent();
            BindingContext = surgicalConciergeViewModel;
            this.surgicalConciergeViewModel = surgicalConciergeViewModel;
            patientAttendeeList = surgicalConciergeViewModel.PatientAttendeeProfileViewModels;
            InitializeListView();

        }
        protected override void OnAppearing()
        {
            InitializeListView();
            base.OnAppearing();

        }

        private void PatientAttendListViewView_Refreshing(object sender, EventArgs e)
        {
            PatientAttendeeListView.EndRefresh();
        }

        public void PatientAttendeeListRefresh()
        {
            InitializeListView();
            PatientAttendeeListView.EndRefresh();
        }

        public void InitializeListView()
        {
            PatientAttendeeListView.ItemsSource = patientAttendeeList;
        }
        public void UpdateListView(IEnumerable<PatientAttendeeProfileViewModel> patientAttendeeList)
        {
            this.patientAttendeeList = patientAttendeeList;
            surgicalConciergeViewModel.PatientAttendeeProfileViewModels = patientAttendeeList;
            this.BindingContext = surgicalConciergeViewModel;
            InitializeListView();
        }
        private async void PatientAttendeeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (PatientAttendeeListView.SelectedItem == null)
                    return;
                PatientAttendeeProfileViewModel patientAttendee = (PatientAttendeeProfileViewModel)PatientAttendeeListView.SelectedItem;
                var selection = await DisplayActionSheet("Choose Action", "Cancel", null, "Edit", "Delete");
                switch (selection)
                {
                    case "Edit":
                        EditAttendee(patientAttendee);
                        break;
                    case "Delete":
                        DeleteAttendee(patientAttendee);
                        break;

                }
                PatientAttendeeListView.SelectedItem = null;
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);

            }
            
        }

        private async void AddNewAttendee_OnClicked(object sender, EventArgs e)
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
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    List<CountryViewModel> countryList = await restService.GetCountryList();
                    await Navigation.PushModalAsync(new PatientAttendeeAddPage(this, surgicalConciergeViewModel.PatientProfileId, countryList));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
                
            }
        }

        private async void EditAttendee(PatientAttendeeProfileViewModel patientAttendee)
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
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    List<CountryViewModel> countryList = await restService.GetCountryList();
                    await Navigation.PushModalAsync(new PatientAttendeeEditPage(this, patientAttendee, countryList));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
                
            }
        }

        private async void DeleteAttendee(PatientAttendeeProfileViewModel patientAttendee)
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
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    ApiExecutionResult<PatientAttendeeProfileViewModel> apiExecutionResult = await restService.DeleteAttendee(patientAttendee);
                    if (apiExecutionResult.Success)
                    {
                        this.patientAttendeeList = apiExecutionResult.DataList;
                        surgicalConciergeViewModel.PatientAttendeeProfileViewModels = patientAttendeeList;
                        this.BindingContext = surgicalConciergeViewModel;
                        InitializeListView();
                        UtilHelper.ShowToastMessage("Attendee profile deleted successfully.");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage("Attendee profile delete failed. Please try again.");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
                
            }
        }

    }
}