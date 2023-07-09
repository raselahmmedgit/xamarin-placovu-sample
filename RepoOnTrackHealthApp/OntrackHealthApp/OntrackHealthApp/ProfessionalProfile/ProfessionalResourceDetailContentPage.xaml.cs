using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ViewModel;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalResourceDetailContentPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private readonly IResourceClient _iResourceClient;
        ResourceDetailPageViewModel _ResourceDetailPageViewModel;
        private int Total = 0;

        public ProfessionalResourceDetailContentPage(ResourceDetailPageViewModel resourceDetailPageViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = resourceDetailPageViewModel.ProcedureName;
                ProcedureName.Text = resourceDetailPageViewModel.ProcedureName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iResourceClient = new ResourceClient(apiClient);

                _ResourceDetailPageViewModel = resourceDetailPageViewModel;
                _ResourceDetailPageViewModel.CurrentResourceDetailViewModel = _ResourceDetailPageViewModel.ResourceDetailViewModels.FirstOrDefault(x => x.PatientPortalResourceDetailId == _ResourceDetailPageViewModel.PatientPortalResourceDetailId);

                //BindingContext = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel;
                var htmlWebViewSource = new HtmlWebViewSource
                {
                    Html = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceContentCustom
                };
                ResourceDetailContentPageWebView.Source = htmlWebViewSource;

                Total = _ResourceDetailPageViewModel.ResourceDetailViewModels.Count();

                MoveNext.IsVisible = true;
                MovePrev.IsVisible = true;

                var item = _ResourceDetailPageViewModel.PatientPortalResourceDetailId;
                if (item == 1)
                {
                    MovePrev.IsVisible = false;
                }
                else if (item == Total)
                {
                    MoveNext.IsVisible = false;
                }

            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
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
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
        }

        private void MoveFirstClickedAsync(object sender, EventArgs e)
        {
            _ResourceDetailPageViewModel.CurrentResourceDetailViewModel = _ResourceDetailPageViewModel.ResourceDetailViewModels.FirstOrDefault();
            _ResourceDetailPageViewModel.PatientPortalResourceDetailId = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceDetailId;

            //BindingContext = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel;
            var htmlWebViewSource = new HtmlWebViewSource
            {
                Html = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceContentCustom
            };
            ResourceDetailContentPageWebView.Source = htmlWebViewSource;

            MovePrev.IsVisible = false;
            MovePrev.TextColor = Color.FromHex("#FFF");

            MoveNext.IsVisible = true;
        }

        private void MovePrevClickedAsync(object sender, EventArgs e)
        {
            int item = _ResourceDetailPageViewModel.PatientPortalResourceDetailId;
            if (item > 1)
            {
                _ResourceDetailPageViewModel.PatientPortalResourceDetailId = item = item - 1;
                _ResourceDetailPageViewModel.CurrentResourceDetailViewModel = _ResourceDetailPageViewModel.ResourceDetailViewModels.FirstOrDefault(x => x.PatientPortalResourceDetailId == item);

                //BindingContext = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel;
                var htmlWebViewSource = new HtmlWebViewSource
                {
                    Html = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceContentCustom
                };
                ResourceDetailContentPageWebView.Source = htmlWebViewSource;

                MoveNext.IsVisible = true;
                MovePrev.IsVisible = true;

                if (item == 1)
                {
                    MovePrev.IsVisible = false;
                    //MovePrev.IsEnabled = false;
                    //MovePrev.TextColor = Color.FromHex("#FFF");
                }
            }
            else
            {
                //MovePrev.IsEnabled = false;
                //MovePrev.TextColor = Color.FromHex("#FFF");
                MovePrev.IsVisible = false;
                ToastHelper.ShowToastMessage("There is no more previous items!");
                MoveNext.IsVisible = true;
            }
        }

        private async void MoveListClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //using (UserDialogs.Instance.Loading(""))
            //{

            //    //ResourcePage page = new ResourcePage();
            //    //await page.LoadDataAsync();
            //    //await Navigation.PushAsync(page);
            //}
        }

        private void MoveNextClickedAsync(object sender, EventArgs e)
        {
            int item = _ResourceDetailPageViewModel.PatientPortalResourceDetailId;
            if (item < Total)
            {
                _ResourceDetailPageViewModel.PatientPortalResourceDetailId = item = item + 1;
                _ResourceDetailPageViewModel.CurrentResourceDetailViewModel = _ResourceDetailPageViewModel.ResourceDetailViewModels.FirstOrDefault(x => x.PatientPortalResourceDetailId == item);

                //BindingContext = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel;
                var htmlWebViewSource = new HtmlWebViewSource
                {
                    Html = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceContentCustom
                };
                ResourceDetailContentPageWebView.Source = htmlWebViewSource;

                MovePrev.IsVisible = true;
                MoveNext.IsVisible = true;

                if (item == Total)
                {
                    MoveNext.IsVisible = false;
                    //MoveNext.IsEnabled = false;
                    //MoveNext.TextColor = Color.FromHex("#FFF");
                }
            }
            else
            {
                MovePrev.IsVisible = true;
                MoveNext.IsVisible = false;
                ToastHelper.ShowToastMessage("There is no more next items!");
                //MoveNext.IsEnabled = false;
                //MoveNext.TextColor = Color.FromHex("#FFF");
            }
        }

        private void MoveLstClickedAsync(object sender, EventArgs e)
        {
            _ResourceDetailPageViewModel.CurrentResourceDetailViewModel = _ResourceDetailPageViewModel.ResourceDetailViewModels.LastOrDefault();
            _ResourceDetailPageViewModel.PatientPortalResourceDetailId = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceDetailId;

            //BindingContext = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel;
            var htmlWebViewSource = new HtmlWebViewSource
            {
                Html = _ResourceDetailPageViewModel.CurrentResourceDetailViewModel.PatientPortalResourceContentCustom
            };
            ResourceDetailContentPageWebView.Source = htmlWebViewSource;

            MovePrev.IsVisible = true;

            MoveNext.IsVisible = false;
            MoveNext.TextColor = Color.FromHex("#FFF");
        }

        private void ResourceDetailContentPageWebView_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                var resourceDetailContentPageWebView = sender as WebView;

                if (resourceDetailContentPageWebView != null)
                {
                    ResourceDetailContentPageWebView.HeightRequest = 600;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}