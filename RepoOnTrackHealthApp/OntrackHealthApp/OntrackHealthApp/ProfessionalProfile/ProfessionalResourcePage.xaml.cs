using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
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
	public partial class ProfessionalResourcePage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private readonly IResourceClient _iResourceClient;
        private readonly IProcedureClient _iProcedureClient;

        private string _procedureName { get; set; }

        private ResourcePageViewModel ResourcePageViewModel { get; set; }

        public ProfessionalResourcePage(long? procedureId, string procedureName)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = procedureName;
                ProcedureName.Text = procedureName;

                _procedureName = procedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iResourceClient = new ResourceClient(apiClient);
                _iProcedureClient = new ProcedureClient(apiClient);

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
                ResourcePageViewModel = new ResourcePageViewModel();
                var response = await _iResourceClient.ProfessionalResourcePage(procedureId);
                if (response.StatusIsSuccessful)
                {
                    var data = response.Data;
                    if (data != null)
                    {
                        ProgramResourceViewModel programResourceViewModel = data;
                        var viewModelList = GetResourcePageViewModelList(programResourceViewModel);
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

        private IEnumerable<ResourceViewModel> GetResourcePageViewModelList(ProgramResourceViewModel programResourceViewModel)
        {
            var resourceViewModelList = new List<ResourceViewModel>();

            if (programResourceViewModel.PatientProcedureResourceViewModels != null)
            {
                foreach (var patientProcedureResourceViewModels in programResourceViewModel.PatientProcedureResourceViewModels)
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
                                resourceDetailViewModel.ResourceContentCombineId = resourceViewModel.ResourceViewModelId.ToString() + ":" + resourceDetailPageViewModelId.ToString();
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
                    resourceDetailPageViewModel.ProcedureName = _procedureName;

                    await Navigation.PushAsync(new ProfessionalResourceDetailContentPage(resourceDetailPageViewModel));
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
                HomeButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            App.ShowUserDialogAsync();
            App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion
        
    }
}