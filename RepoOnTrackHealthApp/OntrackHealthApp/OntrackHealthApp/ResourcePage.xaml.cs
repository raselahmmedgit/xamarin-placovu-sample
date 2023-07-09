using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResourcePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private readonly IResourceClient _iResourceClient;
        private readonly IProcedureClient _iProcedureClient;

        private ResourcePageViewModel ResourcePageViewModel { get; set; }

        public ResourcePage ()
		{
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iResourceClient = new ResourceClient(apiClient);
                _iProcedureClient = new ProcedureClient(apiClient);
                BindingContext = ResourcePageViewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public async Task LoadDataAsync()
        {
            await App.ShowUserDialogDelayAsync();
            try
            {
                ResourcePageViewModel = new ResourcePageViewModel();
                var response = await _iResourceClient.ResourcePage();
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;
                    if (data != null) {
                        ResourceIndexPageViewModel resourceIndexPageViewModel = data;
                        var viewModelList = GetResourcePageViewModelList(resourceIndexPageViewModel);
                        ResourcePageViewModel.ResourceViewModels = viewModelList.ToList();

                        if (ResourcePageViewModel != null)
                        {
                            BindingContext = ResourcePageViewModel;
                            //resourceListViewCategory.ItemsSource = ResourcePageViewModel.ResourceViewModels;
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

        private IEnumerable<ResourceViewModel> GetResourcePageViewModelList(ResourceIndexPageViewModel resourceIndexPageViewModel)
        {
            var resourceViewModelList = new List<ResourceViewModel>();

            if (resourceIndexPageViewModel.PatientProcedureResourceViewModels != null)
            {
                foreach (var patientProcedureResourceViewModels in resourceIndexPageViewModel.PatientProcedureResourceViewModels)
                {
                    if (patientProcedureResourceViewModels.PatientResourceCategoryViewModels.Count() > 1)
                    {
                        int resourcePageViewModelId = 1;
                        int resourceDetailPageViewModelId = 1;
                        foreach (var patientResourceCategoryViewModels in patientProcedureResourceViewModels.PatientResourceCategoryViewModels)
                        {
                            Guid patientResourceCategoryId = patientResourceCategoryViewModels.PatientResourceCategoryId;
                            string patientResourceCategoryName = patientResourceCategoryViewModels.PatientResourceCategoryName;
                            int patientResourceCategoryDisplayOrder = patientResourceCategoryViewModels.DisplayOrder;

                            var resourceViewModel = new ResourceViewModel();
                            resourceViewModel.ResourceViewModelId = resourcePageViewModelId;
                            resourceViewModel.PatientResourceCategoryId = patientResourceCategoryId;
                            resourceViewModel.PatientResourceCategoryName = patientResourceCategoryName;
                            resourceViewModel.PatientResourceCategoryDisplayOrder = patientResourceCategoryDisplayOrder;

                            if (patientResourceCategoryViewModels.PatientPortalResourceViewModels != null && patientResourceCategoryViewModels.PatientPortalResourceViewModels.Any())
                            {
                                var resourceDetailViewModels = new List<ResourceDetailViewModel>();                                

                                foreach (var patientPortalResourceViewModel in patientResourceCategoryViewModels.PatientPortalResourceViewModels.Where(d => d.IsDeleted == false).OrderBy(c => c.DisplayOrder))
                                {
                                    Guid patientPortalResourceId = patientPortalResourceViewModel.PatientPortalResourceId;
                                    string patientPortalResourceName = patientPortalResourceViewModel.PatientPortalResourceName;
                                    var resourceDetailViewModel = new ResourceDetailViewModel();
                                    resourceDetailViewModel.PatientPortalResourceDetailId = resourceDetailPageViewModelId;
                                    resourceDetailViewModel.PatientPortalResourceId = resourceViewModel.ResourceViewModelId;
                                    resourceDetailViewModel.PatientPortalResourceName = patientPortalResourceName;
                                    resourceDetailViewModel.PatientPortalResourceContent = patientPortalResourceViewModel.ResourceContent;
                                    resourceDetailViewModel.ResourceContentCombineId = resourceViewModel.ResourceViewModelId.ToString() + ":" + resourceDetailPageViewModelId.ToString();
                                    resourceDetailViewModels.Add(resourceDetailViewModel);
                                    resourceDetailPageViewModelId++;
                                }

                                resourceViewModel.ResourceDetailViewModels = resourceDetailViewModels;
                            }

                            resourceViewModelList.Add(resourceViewModel);

                            resourcePageViewModelId++;

                        }

                    }
                    else
                    {
                        var resourceViewModel = new ResourceViewModel();

                        resourceViewModel.ResourceViewModelId = 1;
                        resourceViewModel.PatientResourceCategoryId = Guid.NewGuid();
                        resourceViewModel.PatientResourceCategoryName = "Document Name";
                        resourceViewModel.PatientResourceCategoryDisplayOrder = 1;

                        foreach (var patientResourceCategoryViewModels in patientProcedureResourceViewModels.PatientResourceCategoryViewModels)
                        {
                            var resourceDetailViewModels = new List<ResourceDetailViewModel>();

                            int resourceDetailPageViewModelId = 1;
                            
                            foreach (var patientPortalResourceViewModel in patientResourceCategoryViewModels.PatientPortalResourceViewModels.Where(d => d.IsDeleted == false).OrderBy(c => c.DisplayOrder))
                            {
                                Guid patientPortalResourceId = patientPortalResourceViewModel.PatientPortalResourceId;
                                string patientPortalResourceName = patientPortalResourceViewModel.PatientPortalResourceName;

                                var resourceDetailViewModel = new ResourceDetailViewModel();
                                resourceDetailViewModel.PatientPortalResourceDetailId = resourceDetailPageViewModelId;
                                resourceDetailViewModel.PatientPortalResourceId = resourceViewModel.ResourceViewModelId;
                                resourceDetailViewModel.PatientPortalResourceName = patientPortalResourceName;
                                resourceDetailViewModel.PatientPortalResourceContent = patientPortalResourceViewModel.ResourceContent;
                                resourceDetailViewModel.ResourceContentCombineId = resourceViewModel.ResourceViewModelId.ToString()+ ":" + resourceDetailPageViewModelId.ToString();
                                resourceDetailViewModels.Add(resourceDetailViewModel);
                                resourceDetailPageViewModelId++;
                            }

                            resourceViewModel.ResourceDetailViewModels = resourceDetailViewModels;
                        }

                        resourceViewModelList.Add(resourceViewModel);
                    }
                }
            }

            return resourceViewModelList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;

            //BtnResource.IsEnabled = false;
            //BtnResource.BackgroundColor = Color.FromHex("#f7a50f");
            //BtnResource.BorderColor = Color.FromHex("#f7a50f");
        }

        private async void ShowMore_ClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    var resourceDetailButton = sender as Button;
                    var resourceContentId = resourceDetailButton.ClassId.Split(':');
                    var resourcePageViewModelId = Convert.ToInt32(resourceContentId[0]);
                    var resourceDetailPageViewModelId = Convert.ToInt32(resourceContentId[1]);

                    List<ResourceDetailViewModel> resourceDetailViewModelList = new List<ResourceDetailViewModel>();

                    ResourcePageViewModel.ResourceViewModels.ForEach(x => {
                        x.ResourceDetailViewModels.ToList().ForEach(c => {
                            resourceDetailViewModelList.Add(new ResourceDetailViewModel
                            {
                                ResourceCategoryName = x.PatientResourceCategoryName,
                                PatientPortalResourceName = c.PatientPortalResourceName,
                                PatientPortalResourceDetailId = c.PatientPortalResourceDetailId,
                                PatientPortalResourceId = x.ResourceViewModelId,
                                PatientPortalResourceContent = c.PatientPortalResourceContent,
                            });
                        });
                    });

                    ResourceDetailPageViewModel resourceDetailPageViewModel = new ResourceDetailPageViewModel();

                    resourceDetailPageViewModel.ResourceDetailViewModels = resourceDetailViewModelList;
                    resourceDetailPageViewModel.PatientPortalResourceDetailId = resourceDetailPageViewModelId;

                    await Navigation.PushAsync(new ResourceDetailContentPage(resourceDetailPageViewModel));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
            
        }

        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    HomeButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //await Navigation.PushAsync(new MainPatientPage());
                UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                App.Instance.MainPage = new MenuPage(userIdentityModel);
            }
        }

        private async void OnResourceButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ResourceButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ResourceButton()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                ResourcePage resourcePage = new ResourcePage();
                await resourcePage.LoadDataAsync();
                await Navigation.PushAsync(resourcePage);
            }
        }

        private async void OnScheduleButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                var appMessage = await IsCurrentPatientProcedureDetail();
                if (appMessage.MessageType == AppMessageType.Success)
                {
                    await ScheduleButton();
                }
                else
                {
                    await DisplayAlert(string.Empty, AppConstant.NoProcedureMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async Task ScheduleButton()
        {
            //using (UserDialogs.Instance.Loading(""))
            //{
            //    //NotificationListPageViewModel model = new NotificationListPageViewModel();
            //    //await model.ExecuteLoadCommandAsync();
            //    //var notificationListPage = new NotificationListPage(model);

            //    //await Navigation.PushAsync(notificationListPage);

            //    //Navigation.InsertPageBefore(new MainPatientPage(), this);
            //    //await Navigation.PopToRootAsync();

            //    //MenuPatientPage menuPatientPage = new MenuPatientPage();
            //    //menuPatientPage.Detail = new NavigationPage(notificationListPage);

            //}

            //App.ShowUserDialogAsync();
            await Navigation.PushAsync(new NotificationListPageN());
        }

        private async Task<AppMessage> IsCurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }
            else
            {
                return appMessage = await CurrentPatientProcedureDetail();
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetail()
        {
            AppMessage appMessage = new AppMessage();

            using (UserDialogs.Instance.Loading(""))
            {
                if (string.IsNullOrEmpty(_iTokenContainer.CurrentPatientProcedureDetailId))
                {
                    return appMessage = await CurrentActiveProcedureWiseButtonShowHideAsync();
                }
                else
                {
                    return appMessage = await CurrentPatientProcedureDetailWiseButtonShowHideAsync();
                }
            }
        }

        private async Task<AppMessage> CurrentActiveProcedureWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentActiveProcedure = await _iProcedureClient.CurrentActiveProcedure();
            if (responseCurrentActiveProcedure.StatusIsSuccessful)
            {
                var data = responseCurrentActiveProcedure.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        private async Task<AppMessage> CurrentPatientProcedureDetailWiseButtonShowHideAsync()
        {
            AppMessage appMessage = new AppMessage();

            var responseCurrentPatientProcedureDetail = await _iProcedureClient.GetPatientProcedureDetail();
            if (responseCurrentPatientProcedureDetail.StatusIsSuccessful)
            {
                var data = responseCurrentPatientProcedureDetail.Data;

                if (data != null)
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = data.PatientProcedureDetailId.ToString();
                    _iTokenContainer.CurrentProcedureName = data.ProcedureName;

                    #region Physician, Location show and hide

                    if (data.IsSurgeryCompleted)
                    {
                    }
                    else
                    {
                    }

                    #endregion

                    return appMessage = SetAppMessage.SetSuccessMessage();
                }
                else
                {
                    _iTokenContainer.CurrentPatientProcedureDetailId = string.Empty;
                    _iTokenContainer.CurrentProcedureName = string.Empty;

                    #region Physician, Location show and hide

                    #endregion

                    return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
                }

            }
            else
            {
                return appMessage = SetAppMessage.SetErrorMessage(AppConstant.NoProcedureMessage);
            }

        }

        #endregion

    }
}