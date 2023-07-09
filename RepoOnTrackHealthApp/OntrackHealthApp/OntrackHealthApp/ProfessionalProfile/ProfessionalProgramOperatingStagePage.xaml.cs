using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using OntrackHealthApp.ApiHelper.Extensions;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalProgramOperatingStagePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;
        private List<ScgStageViewModel> ScgStageViewModelList { get; set; }

        private string _procedureName { get; set; }

        public ProfessionalProgramOperatingStagePage(long? procedureId, string procedureName)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;
                ProcedureName.Text = procedureName;

                _procedureName = procedureName;

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                LoadDataAsync(procedureId);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async void LoadDataAsync(long? procedureId)
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                ProgramResourceViewModel programResourceViewModel = await professionalProfileRestApiService.ProfessionalProgramSurgicalConciergeStage(procedureId);

                if (programResourceViewModel != null)
                {
                    if (programResourceViewModel.SurgicalConciergeStageViewModels != null)
                    {
                        BuildSurgicalConciergeStage(programResourceViewModel.SurgicalConciergeStageViewModels);
                    }

                    App.HideUserDialogAsync();
                }
                else
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                App.HideUserDialogAsync();
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        private void BuildSurgicalConciergeStage(IEnumerable<ScgStageViewModel> surgicalConciergeStageViewModels)
        {
            if (surgicalConciergeStageViewModels.Count() == 0)
            {
                #region No Data Found
                NoStage.IsVisible = true;
                #endregion
            }
            else
            {
                #region Data List

                int scgStageCount = 1;
                var initialSurgicalConciergeStageViewModel = surgicalConciergeStageViewModels.OrderBy(c => c.DisplayOrder).ToList().FirstOrDefault();
                long initialScgStageId = initialSurgicalConciergeStageViewModel != null ? initialSurgicalConciergeStageViewModel.ScgStageId : 0;

                surgicalConciergeStageViewModels.OrderBy(c => c.DisplayOrder).ToList().ForEach(c =>
                {
                    c.ScgStageCount = scgStageCount.ToString() + ".";

                    string scgStageIcon = string.Empty;

                    //if (c.ScgStageId == initialScgStageId)
                    //{
                    //    scgStageIcon = "patientreportedoutcome/circle_green_light.png";
                    //}
                    //else
                    //{
                    //    scgStageIcon = "patientreportedoutcome/circle_gray_light.png";
                    //}

                    scgStageIcon = "patientreportedoutcome/circle_gray_light.png";

                    c.ScgStageIcon = scgStageIcon;

                    scgStageCount++;
                });

                surgicalConciergeStageViewModels = surgicalConciergeStageViewModels.OrderBy(c => c.DisplayOrder);

                ScgStageViewModelList = surgicalConciergeStageViewModels.ToList();
                SurgicalConciergeStageListView.ItemsSource = surgicalConciergeStageViewModels;

                #endregion
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
        }

        private async void SurgicalConciergeStageListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    var data = (ScgStageViewModel)e.SelectedItem;
                    if (data != null)
                    {
                        SurgicalConciergeStageListView.SelectedItem = null;

                        var scgStageViewModel = data;

                        if (scgStageViewModel.ScgStageIcon == "patientreportedoutcome/circle_gray_light.png")
                        {
                            scgStageViewModel.ScgStageIcon = "patientreportedoutcome/circle_green_light.png";
                        }
                        else
                        {
                            scgStageViewModel.ScgStageIcon = "patientreportedoutcome/circle_gray_light.png";
                        }

                        await Navigation.PushAsync(new ProfessionalProgramOperatingStageContentPage(_procedureName, scgStageViewModel));
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }

        }

        private async void SurgicalConciergeStageIconImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.ShowUserDialogDelayAsync();
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    ImageButton imageButton = sender as ImageButton;
                    if (imageButton != null)
                    {
                        long scgStageId = imageButton.ClassId.ToLong();
                        var scgStageViewModel = ScgStageViewModelList.FirstOrDefault(x => x.ScgStageId == scgStageId);

                        //SurgicalConciergeStageIconImage.Source = "patientreportedoutcome/circle_gray_light.png";

                        if (scgStageViewModel.ScgStageIcon == "patientreportedoutcome/circle_gray_light.png")
                        {
                            scgStageViewModel.ScgStageIcon = "patientreportedoutcome/circle_green_light.png";
                            imageButton.Source = "patientreportedoutcome/circle_green_light.png";
                        }
                        else
                        {
                            scgStageViewModel.ScgStageIcon = "patientreportedoutcome/circle_gray_light.png";
                            imageButton.Source = "patientreportedoutcome/circle_green_light.png";
                        }

                        await Navigation.PushAsync(new ProfessionalProgramOperatingStageContentPage(_procedureName, scgStageViewModel));
                    }

                    App.HideUserDialogAsync();
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }
    }
}