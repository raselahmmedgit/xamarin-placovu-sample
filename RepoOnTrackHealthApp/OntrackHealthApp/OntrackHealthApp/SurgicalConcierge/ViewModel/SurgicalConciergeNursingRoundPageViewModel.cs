using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.SurgicalConcierge.ViewModel
{
    public class SurgicalConciergeNursingRoundPageViewModel : ViewModelBase
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly SurgicalConciergeRestApiService _restApiService;
        ProfessionalNursingRoundPage _professionalNursingRoundPage;
        ProfessionalProgramNursingRoundPage _professionalProgramNursingRoundPage;
        SurgicalConciergeNursingRoundPage _surgicalConciergeNursingRoundPage;
        public SurgicalConciergeNursingRoundViewModel SurgicalConciergeNursingRoundViewModel { get; set; }
        public ObservableCollection<ScgNursingRoundTemplateCategoryApiViewModel> ScgNursingRoundTemplateCategoryApiViewModels { get; set; }
        public ObservableCollection<ScgNursingRoundTemplateCategoryDetailApiViewModel> ScgNursingRoundTemplateCategoryDetailApiViewModels { set; get; }
        public ObservableCollection<string> CountryViewModels { set; get; }
        public Command LoadSurgicalConciergeNursingRoundCommand { get; set; }
        public Command ScgNursingRoundSendNotificationCommand { get; set; }
        public SurgicalConciergeNursingRoundPageViewModel()
        {
            App.ShowUserDialog();
            _iTokenContainer = new TokenContainer();
            _restApiService = new SurgicalConciergeRestApiService();
        }

        public void LoadSurgicalConciergeNursingRound()
        {
            LoadSurgicalConciergeNursingRoundCommand = new Command(async (x) => await ExecuteSurgicalConciergeNursingRoundCommandAsync(x));
            ScgNursingRoundSendNotificationCommand = new Command(async () => await ExecuteScgNursingRoundSendNotificationCommandAsync());
            ScgNursingRoundTemplateCategoryApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryApiViewModel>();
        }

        public void LoadProfessionalNursingRound()
        {
            LoadSurgicalConciergeNursingRoundCommand = new Command(async (x) => await ExecuteProfessionalNursingRoundCommandAsync(x));
            ScgNursingRoundSendNotificationCommand = new Command(async () => await ExecuteScgProfessionalNursingRoundSendNotificationCommandAsync());
            ScgNursingRoundTemplateCategoryApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryApiViewModel>();
        }

        public void LoadProfessionalProgramNursingRound()
        {
            LoadSurgicalConciergeNursingRoundCommand = new Command(async (x) => await ExecuteProfessionalProgramNursingRoundCommandAsync(x));
            ScgNursingRoundSendNotificationCommand = new Command(async () => await ExecuteProfessionalProgramNursingRoundSendNotificationCommandAsync());
            ScgNursingRoundTemplateCategoryApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryApiViewModel>();
        }

        private async Task ExecuteProfessionalNursingRoundCommandAsync(object x)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                App.ShowUserDialog();
                CountryViewModels = new ObservableCollection<string>();
                SurgicalConciergeNursingRoundViewModel = new SurgicalConciergeNursingRoundViewModel();
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                SurgicalConciergeNursingRoundViewModel = await restApiService.GetSurgicalConciergeNursingRounds();
                foreach (var item in SurgicalConciergeNursingRoundViewModel.ScgNursingRoundTemplateCategoryApiViewModels)
                {
                    ScgNursingRoundTemplateCategoryDetailApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryDetailApiViewModel>();
                    foreach (var itemDetail in item.ScgNursingRoundTemplateCategoryDetailApiViewModels)
                    {
                        ScgNursingRoundTemplateCategoryDetailApiViewModels.Add(itemDetail);
                    }
                    ScgNursingRoundTemplateCategoryApiViewModels.Add(item);
                }
                var countryDistinct = SurgicalConciergeNursingRoundViewModel.CountryViewModels.OrderBy(c => c.PhoneCodeNumeric).Select(z => z.PhoneCode).Distinct();
                foreach (var itemCountry in countryDistinct)
                {
                    CountryViewModels.Add("+" + itemCountry.ToString());
                }
                _professionalNursingRoundPage = x as ProfessionalNursingRoundPage;
                await _professionalNursingRoundPage.CreateForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                App.HideUserDialog();
            }
        }

        private async Task ExecuteProfessionalProgramNursingRoundCommandAsync(object x)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                App.ShowUserDialog();
                CountryViewModels = new ObservableCollection<string>();
                SurgicalConciergeNursingRoundViewModel = new SurgicalConciergeNursingRoundViewModel();
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                SurgicalConciergeNursingRoundViewModel = await restApiService.GetSurgicalConciergeNursingRounds();
                foreach (var item in SurgicalConciergeNursingRoundViewModel.ScgNursingRoundTemplateCategoryApiViewModels)
                {
                    ScgNursingRoundTemplateCategoryDetailApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryDetailApiViewModel>();
                    foreach (var itemDetail in item.ScgNursingRoundTemplateCategoryDetailApiViewModels)
                    {
                        ScgNursingRoundTemplateCategoryDetailApiViewModels.Add(itemDetail);
                    }
                    ScgNursingRoundTemplateCategoryApiViewModels.Add(item);
                }
                var countryDistinct = SurgicalConciergeNursingRoundViewModel.CountryViewModels.OrderBy(c => c.PhoneCodeNumeric).Select(z => z.PhoneCode).Distinct();
                foreach (var itemCountry in countryDistinct)
                {
                    CountryViewModels.Add("+" + itemCountry.ToString());
                }
                _professionalProgramNursingRoundPage = x as ProfessionalProgramNursingRoundPage;
                await _professionalProgramNursingRoundPage.CreateForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                App.HideUserDialog();
            }
        }

        private async Task ExecuteSurgicalConciergeNursingRoundCommandAsync(object x)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                App.ShowUserDialog();
                CountryViewModels = new ObservableCollection<string>();
                SurgicalConciergeNursingRoundViewModel = new SurgicalConciergeNursingRoundViewModel();
                SurgicalConciergeRestApiService restApiService = new SurgicalConciergeRestApiService();
                SurgicalConciergeNursingRoundViewModel = await restApiService.GetSurgicalConciergeNursingRounds();
                foreach (var item in SurgicalConciergeNursingRoundViewModel.ScgNursingRoundTemplateCategoryApiViewModels)
                {
                    ScgNursingRoundTemplateCategoryDetailApiViewModels = new ObservableCollection<ScgNursingRoundTemplateCategoryDetailApiViewModel>();
                    foreach (var itemDetail in item.ScgNursingRoundTemplateCategoryDetailApiViewModels)
                    {
                        ScgNursingRoundTemplateCategoryDetailApiViewModels.Add(itemDetail);
                    }
                    ScgNursingRoundTemplateCategoryApiViewModels.Add(item);
                }
                var countryDistinct = SurgicalConciergeNursingRoundViewModel.CountryViewModels.OrderBy(c => c.PhoneCodeNumeric).Select(z => z.PhoneCode).Distinct();
                foreach (var itemCountry in countryDistinct)
                {
                    CountryViewModels.Add("+" + itemCountry.ToString());
                }
                _surgicalConciergeNursingRoundPage = x as SurgicalConciergeNursingRoundPage;
                await _surgicalConciergeNursingRoundPage.CreateForm();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                App.HideUserDialog();
            }
        }

        private async Task ExecuteScgNursingRoundSendNotificationCommandAsync()
        {
            try
            {
                App.ShowUserDialog();
                var result = await _restApiService.SurgicalConciergeNursingRoundSendNotification(SurgicalConciergeNursingRoundViewModel);
                App.HideUserDialog();
                if (result.Success)
                    _surgicalConciergeNursingRoundPage.ShowAlert(AppConstant.DisplayAlertTitle, result.Message);
                else
                {
                    _surgicalConciergeNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, result.Message);
                }
            }
            catch (Exception)
            {
                _surgicalConciergeNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage);// ErrorMessage = AppConstant.DisplayAlertErrorMessage; // await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }
        private async Task ExecuteScgProfessionalNursingRoundSendNotificationCommandAsync()
        {
            try
            {
                App.ShowUserDialog();
                var result = await _restApiService.SurgicalConciergeNursingRoundSendNotification(SurgicalConciergeNursingRoundViewModel);
                App.HideUserDialog();
                if (result.Success)
                    _professionalNursingRoundPage.ShowAlert(AppConstant.DisplayAlertTitle, result.Message);
                else
                {
                    _professionalNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, result.Message);
                }
            }
            catch (Exception)
            {
                _professionalNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage);// ErrorMessage = AppConstant.DisplayAlertErrorMessage; // await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }
        private async Task ExecuteProfessionalProgramNursingRoundSendNotificationCommandAsync()
        {
            try
            {
                App.ShowUserDialog();
                var result = await _restApiService.SurgicalConciergeNursingRoundSendNotification(SurgicalConciergeNursingRoundViewModel);
                App.HideUserDialog();
                if (result.Success)
                    _professionalProgramNursingRoundPage.ShowAlert(AppConstant.DisplayAlertTitle, result.Message);
                else
                {
                    _professionalProgramNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, result.Message);
                }
            }
            catch (Exception)
            {
                _professionalProgramNursingRoundPage.ShowAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage);// ErrorMessage = AppConstant.DisplayAlertErrorMessage; // await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }
    }
}
