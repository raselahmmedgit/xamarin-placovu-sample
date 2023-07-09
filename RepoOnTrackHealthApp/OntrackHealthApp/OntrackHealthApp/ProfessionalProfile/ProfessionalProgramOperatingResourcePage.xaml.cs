using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramOperatingResourcePage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private readonly ProfessionalProfileRestApiService _professionalProfileRestApiService;
        private string _procedureName { get; set; }
        private ProgramResourceViewModel programResourceViewModel;
        public ProfessionalProgramOperatingResourcePage(long? procedureId, string procedureName)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                _professionalProfileRestApiService = new ProfessionalProfileRestApiService();
                Title = _iTokenContainer.ApiPracticeName;

                //Subtitle = procedureName;
                ProcedureName.Text = procedureName;

                _procedureName = procedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

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
                programResourceViewModel = await _professionalProfileRestApiService.ProfessionalProgramSurgicalConciergeResource(procedureId);
                if (programResourceViewModel !=null)
                {
                    List<ProfessionalProgramResourceViewModel> surgicalResourceViewModels = new List<ProfessionalProgramResourceViewModel>();
                    List<SurgicalResourceDocumentViewModel> surgicalResourceDocumentViewModels = new List<SurgicalResourceDocumentViewModel>(programResourceViewModel.SurgicalConciergeDocumentViewModel.SurgicalResourceDocumentList);
                    if (surgicalResourceDocumentViewModels != null && surgicalResourceDocumentViewModels.Any())
                    {
                        //int paddingtop = 0;
                        foreach(var resource in surgicalResourceDocumentViewModels)
                        {
                            ProfessionalProgramResourceViewModel surgicalResourceViewModel = new ProfessionalProgramResourceViewModel()
                            {
                                ResourceTitle = resource.SurgicalResourceList.FirstOrDefault().ResourceTitle,
                                ResourceText = resource.SurgicalResourceList.FirstOrDefault().ResourceText,
                                //PaddingTop = paddingtop
                            };
                            surgicalResourceViewModels.Add(surgicalResourceViewModel);
                            //paddingtop++;
                        }
                        if (surgicalResourceViewModels.Count() > 0)
                        {
                             resourceListView.ItemsSource = surgicalResourceViewModels;
                        }
                        else
                        {
                            NoResource.IsVisible = true;
                        }
                    }
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
        }
    }
}