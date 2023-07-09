using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.UserControls;
using OntrackHealthApp.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.PatientProgressReportGraph
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PatientProgressReportGraphPage : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private PatientProgressReportGraphPageViewModel PatientProgressReportGraphPageViewModel { get; set; }

        private readonly IProcedureClient _iProcedureClient;

        public PatientProgressReportGraphPage ()
		{
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();

                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                PatientProgressReportGraphPageViewModel = new PatientProgressReportGraphPageViewModel();
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                _iProcedureClient = new ProcedureClient(apiClient);
                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void RenderPatientProgressGraph(PatientProgressReportGraphPageViewModel patientProgressReportGraphPageViewModel)
        {
            PatientEmailTemplateGraph.Children.Clear();
            foreach (var htmlContent in patientProgressReportGraphPageViewModel.PatientProgressReportViewModel.PatientProgressReportGraphHistoryViews)
            {
                ButtonExtended btnLine = new ButtonExtended
                {
                    Text = "Line",
                    SelectedDataItem = htmlContent.GraphName,
                    Margin = new Thickness(0, 5, 20, 0),
                    WidthRequest = 80,
                    BackgroundColor = Color.FromHex("#467494"),
                };
                btnLine.Clicked += BtnLine_Clicked;
                ButtonExtended btnBar = new ButtonExtended
                {
                    Text = "Bar",
                    SelectedDataItem = htmlContent.GraphName,
                    Margin = new Thickness(0, 5, 0, 0),
                    WidthRequest = 80,
                    BackgroundColor = Color.FromHex("#467494"),
                };
                btnBar.Clicked += BtnBar_Clicked;

                StackLayout stackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 0),
                };
                stackLayout.Children.Add(btnLine);
                stackLayout.Children.Add(btnBar);

                //var gridProgress = new Grid {Padding = new Thickness(0), Margin = new Thickness(0) };
                //gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(56, GridUnitType.Absolute) });
                //gridProgress.Children.Add(btnLine, 0, 1);
                //gridProgress.Children.Add(btnBar, 1, 1);

                WebView webView = new WebView
                {
                    ClassId = htmlContent.GraphName,
                    WidthRequest = 500,
                    HeightRequest = 500,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(0, 2, 0, 0),
                };
                HtmlLabel htmlLabel = new HtmlLabel
                {
                    Text = htmlContent.GraphDefaultContent,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(0, 2, 0, 0)
                };

                HtmlWebViewSource source = new HtmlWebViewSource
                {
                    Html = htmlContent.PatientEmailTemplateGraphHtml
                };
                webView.Source = source;

                PatientEmailTemplateGraph.Children.Add(stackLayout);
                PatientEmailTemplateGraph.Children.Add(webView);
                PatientEmailTemplateGraph.Children.Add(htmlLabel);
            }

        }

        private void BtnLine_Clicked(object sender, EventArgs e)
        {
            ButtonExtended btnLine = (ButtonExtended)sender;
            var view = PatientEmailTemplateGraph.Children.Where(x => x.GetType() == typeof(WebView) && x.ClassId == btnLine.SelectedDataItem.ToString()).FirstOrDefault();
            if (view != null)
            {
                var chartData = this.PatientProgressReportGraphPageViewModel.GeneratePatientProgressGraphSingle(btnLine.SelectedDataItem.ToString(), PatientProgressReportGraphPageViewModel.ChartType.Line);
                WebView webView = (WebView)view;
                HtmlWebViewSource source = new HtmlWebViewSource
                {
                    Html = chartData.PatientEmailTemplateGraphHtml
                };
                webView.Source = source;
            }
        }

        private void BtnBar_Clicked(object sender, EventArgs e)
        {
            ButtonExtended btnLine = (ButtonExtended)sender;
            var view = PatientEmailTemplateGraph.Children.Where(x => x.GetType() == typeof(WebView) && x.ClassId == btnLine.SelectedDataItem.ToString()).FirstOrDefault();
            if (view != null)
            {
                var chartData = this.PatientProgressReportGraphPageViewModel.GeneratePatientProgressGraphSingle(btnLine.SelectedDataItem.ToString(), PatientProgressReportGraphPageViewModel.ChartType.Bar);
                WebView webView = (WebView)view;
                HtmlWebViewSource source = new HtmlWebViewSource
                {
                    Html = chartData.PatientEmailTemplateGraphHtml
                };
                webView.Source = source;
            }
        }

        private void RenderPatientProgressResource(PatientProgressReportGraphPageViewModel patientProgressReportGraphPageViewModel)
        {
            var patientProgressReportResourceHistoryViews = patientProgressReportGraphPageViewModel.PatientProgressReportViewModel.PatientProgressReportResourceHistoryViews.OrderBy(o => o.DisplayOrder).ToList();
            PatientEmailTemplateResource.ItemsSource = patientProgressReportResourceHistoryViews;
        }

        private async void LoadDataAsyc()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    await PatientProgressReportGraphPageViewModel.LoadDataAsync();
                    if (PatientProgressReportGraphPageViewModel.HasPatientProgressReport)
                    {
                        PatientProgressReportGraphPageViewModel.PatientProgressReportViewModel.PatientEmailTemplateConclusion = PatientProgressReportGraphPageViewModel.PatientProgressReportViewModel.PatientEmailTemplateConclusion.ToString().Replace("</br>", Environment.NewLine);
                        this.BindingContext = PatientProgressReportGraphPageViewModel.PatientProgressReportViewModel;
                        RenderPatientProgressGraph(PatientProgressReportGraphPageViewModel);
                        RenderPatientProgressResource(PatientProgressReportGraphPageViewModel);
                        PatientProgressReportGraphPageContent.IsVisible = true;
                        PatientProgressReportGraphPageMessage.IsVisible = false;
                    }
                    else
                    {
                        PatientProgressReportGraphPageContent.IsVisible = false;
                        PatientProgressReportGraphPageMessage.IsVisible = true;
                    }

                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
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

            App.ShowUserDialogAsync();
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