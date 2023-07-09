using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ScgPathology
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScgPathologyPatientProfilePage : ContentPage
	{

        SurgicalConciergePatientProfilePageViewModel viewmodel;
        private readonly ITokenContainer _iTokenContainer;

        public ScgPathologyPatientProfilePage ()
		{
			InitializeComponent ();
            BindingContext = viewmodel = new SurgicalConciergePatientProfilePageViewModel();
            Title = viewmodel.PageTitle;
            _iTokenContainer = new TokenContainer();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewmodel.LoadPatientProfileWithProfessionalProceduresCommand.Execute(null);
            var ccc = viewmodel.PatientProfileWithProfessionalProcedures;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as PatientProfileWithProfessionalProcedureView;
            if (item == null)
                return;

            await Navigation.PushAsync(new ScgPathologyPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private async void btnAddNew_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                

            }
            catch
            {
                await DisplayAlert("Warning!", AppConstant.DisplayAlertErrorMessage, AppConstant.MessageOkButtonText);
            }
        }


        #region Top Menu Actions

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new AdminChangePassword());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {

                if (_iTokenContainer != null)
                {
                    _iTokenContainer.ClearApiToken();
                }
                DependencyService.Get<IToast>().SetSettingsForUserLogout();
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        #endregion
    }
}