using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using OntrackHealthApp.Droid.SurgicalConcierge;
//using Android.App;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalOperatingPage : ContentPage
    {
        //private List<SurgicalConciergeStageViewModel> stageList;
        private SurgicalConciergeViewModel SurgicalConciergeViewModel;
        private SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel;
        private readonly ITokenContainer _iTokenContainer;
        private ObservableCollection<SurgicalConciergeStageViewModel> SurgicalConciergeStageViewModelList { get; set; }

        private SurgicalConciergeStageViewModel CurrentActiveSurgicalConciergeStageViewModel { get; set; }

        public ProfessionalOperatingPage(SurgicalConciergeViewModel surgicalConciergeViewModel, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = null)
        {
            InitializeComponent();

            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                SurgicalConciergeViewModel = surgicalConciergeViewModel;
                SurgicalConciergePatientViewModel = surgicalConciergePatientViewModel;

                //ShowToolbar();

                PatientFullName.Text = SurgicalConciergePatientViewModel.PatientFullName;
                ProcedureName.Text = surgicalConciergeViewModel.ProcedureName;
                ProfessionalName.Text = surgicalConciergeViewModel.ProfessionalName;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }

        }
        public void LoadData()
        {
            if (SurgicalConciergePatientViewModel != null)
            {
                this.BindingContext = SurgicalConciergePatientViewModel;
            }
            if (SurgicalConciergeViewModel.SurgicalConciergeDetailViewModels != null)
            {
                //uncomment
                //SpeechHelper.speechHelperconciergeDetail = this;
                if (SurgicalConciergeViewModel.SurgicalConciergeDetailViewModels != null)
                {
                    //uncomment
                    //SpeechHelper.SurgicalConciergeStageList = SurgicalConciergeViewModel.SurgicalConciergeDetailViewModels.ToList();
                    SurgicalConciergeStageViewModelList = new ObservableCollection<SurgicalConciergeStageViewModel>(SurgicalConciergeViewModel.SurgicalConciergeDetailViewModels.ToList());
                }
                //uncomment
                //SpeechHelper.LeonardoSpeechTextList = SurgicalConciergeViewModel.LeonardSpeechTextViewModels != null ? SurgicalConciergeViewModel.LeonardSpeechTextViewModels.ToList() : null;
            }
            LoadSurgicalConciergeStageList(SurgicalConciergeViewModel);

        }

        private void LoadSurgicalConciergeStageList(SurgicalConciergeViewModel surgicalConciergeViewModel)
        {
            SurgicalConciergeStageViewModelList = new ObservableCollection<SurgicalConciergeStageViewModel>(surgicalConciergeViewModel.SurgicalConciergeDetailViewModels.ToList());
            CreateFormT(SurgicalConciergeStageViewModelList);
            //if (CurrentActiveSurgicalConciergeStageViewModel != null)
            //{
            //    CommentButton.IsVisible = true;
            //    CommentButtonStackLayout.IsVisible = true;
            //}
        }

        private async Task StartSurgicalStage(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                var button = sender as Button;
                var selectedStage = button.BindingContext as SurgicalConciergeStageViewModel;

                var answer = await DisplayAlert("Confirm Start", "Are you sure you want to start this stage? In this case it will end any previous started stage.", "Start", "Cancel");
                if (answer)
                {
                    String successResult = await ExecuteSurgicalStageStart(selectedStage);
                    UtilHelper.ShowToastMessage(successResult);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        public async Task<String> ExecuteSurgicalStageStart(SurgicalConciergeStageViewModel selectedStage)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                String successResult;
                ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
                SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                apiExecutionResult = await restService.StartSurgicalConciergeStage(selectedStage.ScgStageProfessionalProcedureId, selectedStage.PatientProfileId.Value, selectedStage.PatientProcedureDetailId.ToString());
                if (apiExecutionResult.Success)
                {
                    SpeechHelper.SurgicalConciergeStageList = apiExecutionResult.DataList;
                    SurgicalConciergeStageViewModelList = new ObservableCollection<SurgicalConciergeStageViewModel>(apiExecutionResult.DataList.ToList());
                    CreateFormT(SurgicalConciergeStageViewModelList);
                    successResult = "Stage has been started successfully.";
                    if (selectedStage.StageId == (long)Enums.NerveSparingEnum.StageId)
                    {
                        List<SurveyQuestionDetail> surveyQuestionDetails = new List<SurveyQuestionDetail>();
                        surveyQuestionDetails = await restService.GetSurveyQuestionDetails((long)Enums.NerveSparingEnum.SurveyQuestionId);
                        await Navigation.PushModalAsync(new ProfessionalNerveSparingSurvey(surveyQuestionDetails, SurgicalConciergePatientViewModel));
                    }
                }
                else
                {
                    successResult = apiExecutionResult.Message;
                }
                return successResult;
            }
        }
        private async Task EndSurgicalStage(object sender, EventArgs e)
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            try
            {
                var button = sender as Button;
                var selectedStage = button.BindingContext as SurgicalConciergeStageViewModel;

                var answer = await DisplayAlert("Confirm Stop", "Are you sure you want to end this stage?", "End", "Cancel");
                if (answer)
                {
                    String successResult = await ExcuteSurgicalStageEnd(selectedStage);
                    UtilHelper.ShowToastMessage(successResult);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }


        }

        public async Task<string> ExcuteSurgicalStageEnd(SurgicalConciergeStageViewModel selectedStage)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                String successResult;
                ApiExecutionResult<SurgicalConciergeStageViewModel> apiExecutionResult = new ApiExecutionResult<SurgicalConciergeStageViewModel>();
                SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                apiExecutionResult = await restService.StopSurgicalConciergeStage(selectedStage.ScgStageProfessionalProcedureId, selectedStage.PatientProfileId.Value, selectedStage.PatientProcedureDetailId.ToString());
                if (apiExecutionResult.Success)
                {
                    SpeechHelper.SurgicalConciergeStageList = apiExecutionResult.DataList;
                    SurgicalConciergeStageViewModelList = new ObservableCollection<SurgicalConciergeStageViewModel>(apiExecutionResult.DataList.ToList());
                    CreateFormT(SurgicalConciergeStageViewModelList);
                    successResult = "Stage has been ended successfully.";
                }
                else
                {
                    successResult = apiExecutionResult.Message;
                }
                return successResult;
            }
        }

        private async void GetSurgicalComment(object sender, EventArgs e)
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
                    SurgicalConciergeStageCommentViewModel surgicalConciergeStageCommentViewModel = new SurgicalConciergeStageCommentViewModel();
                    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                    surgicalConciergeStageCommentViewModel = await restService.GetSurgicalConciergeStageComment(CurrentActiveSurgicalConciergeStageViewModel.ScgStageProfessionalProcedureId
                        , CurrentActiveSurgicalConciergeStageViewModel.PatientProfileId.Value
                        , CurrentActiveSurgicalConciergeStageViewModel.PatientProcedureDetailId.ToString());

                    await Navigation.PushModalAsync(new ProfessionalCommentView(surgicalConciergeStageCommentViewModel));
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }

            }

        }

        private void ShowToolbar()
        {
            try
            {
                var toolbarItemToBeRemoved = ToolbarItems.FirstOrDefault(c => c.Text.Equals("Recipient"));
                if (toolbarItemToBeRemoved != null)
                    ToolbarItems.Remove(toolbarItemToBeRemoved);

                if (Device.RuntimePlatform == Device.iOS)
                {
                    // move layout under the status bar
                    this.Padding = new Thickness(0, 20, 0, 0);

                    var toolbarItem = new ToolbarItem("Recipient", null, () =>
                    {
                        // stop speech recognizer
                        //SpeechHelper.StopSpeechRecognize();
                        //LeonardoButton.Text = "Start Leonardo";

                        Navigation.PushAsync(new ProfessionalRecipientPage(SurgicalConciergePatientViewModel, (long)PracticeDivisionUnit.OperatingRoom));

                    }, 0, 0);
                    toolbarItem.Text = "Recipient";
                    ToolbarItems.Add(toolbarItem);
                }
                else
                {
                    var toolbarItem = new ToolbarItem("Recipient", null, () =>
                    {
                        // stop speech recognizer
                        //SpeechHelper.StopSpeechRecognize();
                        //LeonardoButton.Text = "Start Leonardo";

                        Navigation.PushAsync(new ProfessionalRecipientPage(SurgicalConciergePatientViewModel, (long)PracticeDivisionUnit.OperatingRoom));

                    }, 0, 0);
                    toolbarItem.Text = "Recipient";
                    ToolbarItems.Add(toolbarItem);

                }
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private async void LeonardoButton_Clicked(object sender, EventArgs e)
        {
            //if (!InternetConnectHelper.CheckConnection())
            //{
            //    UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
            //    return;
            //}
            //if (!AndroidHelper.IsSpeechRecordingSupported())
            //{
            //    var answer = await DisplayAlert("Install Google Home", "You need to install google home to continue with leonardo","Install","Cancel");
            //    if (answer)
            //    {
            //        AndroidHelper.OpenPlayStoreForGoogleHome();
            //    }
            //    else
            //    {
            //        UtilHelper.ShowToastMessage(AppConstants.LeonardoIsNotSupported);
            //    }

            //    return;
            //}

            //using (UserDialogs.Instance.Loading(""))
            //{
            //    try
            //    {
            //        string buttonText = LeonardoButton.Text;
            //        if (buttonText.Equals("Start Leonardo"))
            //        {
            //            SpeechHelper.StartSpeechRecognize();
            //            LeonardoButton.Text = "Stop Leonardo";
            //        }
            //        else
            //        {
            //            SpeechHelper.StopSpeechRecognize();
            //            LeonardoButton.Text = "Start Leonardo";
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            //    }

            //}
        }
        protected override bool OnBackButtonPressed()
        {
            //SpeechHelper.StopSpeechRecognize();
            return base.OnBackButtonPressed();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            if (InternetConnectHelper.CheckConnection())
            {
                //CreateFormT(SurgicalConciergeStageViewModelList);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }
        protected override void OnDisappearing()
        {
            try
            {
                //SpeechHelper.StopSpeechRecognize();
                //LeonardoButton.Text = "Start Leonardo";
                base.OnDisappearing();
            }
            catch (Exception)
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        private void CreateForm(ObservableCollection<SurgicalConciergeStageViewModel> surgicalConciergeStageViewModelList)
        {
            MainContainer.Children.Clear();

            foreach (var item in surgicalConciergeStageViewModelList)
            {

                StackLayout firstContent = new StackLayout
                {
                    Padding = new Thickness(10),
                    BackgroundColor = Color.FromHex("#e8edf1")
                };
                StackLayout secontContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Label lbl = new Label
                {
                    Text = item.StageName,
                    TextColor = Color.FromHex("#000"),
                    FontSize = 20,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                secontContent.Children.Add(lbl);

                StackLayout thirdContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    MinimumWidthRequest = 100
                };

                Button submitButton = new Button
                {
                    HeightRequest = 46,
                    CornerRadius = 10,
                    BackgroundColor = Color.FromHex("#004D80"),
                    TextColor = Color.FromHex("#FFF"),
                    FontSize = 16,
                    Text = "Send",
                    Margin = new Thickness(0, 5),
                    WidthRequest = 100
                };

                #region If Else

                bool hasStarted = false;

                if (item.HasStarted && item.HasEnded)
                {
                    // Marke as Completed
                }
                else if (item.HasStarted && item.HasEnded == false)
                {
                    // Marke as Started
                    hasStarted = true;
                }

                if (item.IsCompleted == false && hasStarted == false)
                {
                    submitButton.Clicked += async (sender, args) => await BtnTemplateText_ClickedAsync(sender, args, item.StageId, item.StageName);
                }
                else
                {
                    CurrentActiveSurgicalConciergeStageViewModel = item;

                    submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                    submitButton.Text = "Delivered";

                    firstContent.BackgroundColor = Color.FromHex("#004c7e");

                    lbl.TextColor = Color.FromHex("#FFF");
                }

                #endregion

                thirdContent.Children.Add(submitButton);

                Grid grdLayout = new Grid();
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.Children.Add(thirdContent, 1, 0);

                firstContent.Children.Add(secontContent);
                firstContent.Children.Add(grdLayout);

                StackLayout outerContent = new StackLayout
                {
                    Padding = new Thickness(2),
                    Margin = new Thickness(6, 4),
                    BackgroundColor = Color.FromHex("#004D80")
                };
                outerContent.Children.Add(firstContent);
                MainContainer.Children.Add(outerContent);
            }

        }

        private void CreateFormT(ObservableCollection<SurgicalConciergeStageViewModel> surgicalConciergeStageViewModelList)
        {
            StackLayout MainContainerInner = new StackLayout { BackgroundColor = Color.Transparent };
            foreach (var item in surgicalConciergeStageViewModelList)
            {
                Frame outerContent = new Frame
                {
                    BorderColor = Color.FromHex("#004D80"),
                    CornerRadius = 6,
                    BackgroundColor = Color.FromHex("#e8edf1"),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                StackLayout firstContent = new StackLayout
                {
                    Padding = new Thickness(12, 0, 12, 12),
                    BackgroundColor = Color.Transparent,
                };

                Label lbl = new Label
                {
                    Text = item.StageName,
                    TextColor = Color.FromHex("#000"),
                    FontSize = 20,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                firstContent.Children.Add(lbl);
                StackLayout secondContent = new StackLayout
                {
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                Button submitButton = new Button
                {
                    HeightRequest = 46,
                    CornerRadius = 23,
                    BackgroundColor = Color.FromHex("#004D80"),
                    TextColor = Color.FromHex("#FFF"),
                    FontSize = 16,
                    Text = "Send",
                    Margin = new Thickness(0, 5),
                    WidthRequest = 120
                };
                #region If Else

                bool hasStarted = false;

                if (item.HasStarted && item.HasEnded)
                {
                    // Marke as Completed
                }
                else if (item.HasStarted && item.HasEnded == false)
                {
                    // Marke as Started
                    hasStarted = true;
                }

                if (item.IsCompleted == false && hasStarted == false)
                {
                    submitButton.Clicked += async (sender, args) => await BtnTemplateText_ClickedAsync(sender, args, item.StageId, item.StageName);
                }
                else
                {
                    CurrentActiveSurgicalConciergeStageViewModel = item;
                    submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                    submitButton.Text = "Delivered";
                    outerContent.BackgroundColor = Color.FromHex("#004c7e");
                    lbl.TextColor = Color.FromHex("#FFF");
                }

                #endregion
                Grid grdLayout = new Grid();
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                grdLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grdLayout.Children.Add(submitButton, 1, 0);
                secondContent.Children.Add(grdLayout);

                StackLayout thirdContent = new StackLayout { };
                thirdContent.Children.Add(firstContent);
                thirdContent.Children.Add(secondContent);
                outerContent.Content = thirdContent;
                MainContainerInner.Children.Add(outerContent);
            }
            MainContainer.Children.Clear();
            MainContainer.Children.Add(MainContainerInner);

            var firstSurgicalconciergeDetailViewModel = SurgicalConciergeStageViewModelList.OrderBy(o => o.DisplayOrder).
                FirstOrDefault(item => item.HasStarted == true && !item.StageId.Equals((int)ScgStageIdEnum.PatientIsInOperatingRoom));

            if (firstSurgicalconciergeDetailViewModel != null)
            {
                //OperatingStartEndtTimeLayout.IsVisible = true;
                StartTime.Text = firstSurgicalconciergeDetailViewModel.StartCSTDateTime.Value.ToString("hh:mm tt") + "   ";
                EstimatedEndTime.Text = firstSurgicalconciergeDetailViewModel.StartCSTDateTime.Value.AddMinutes((int)DefaultEstimatedTime.OperatingRoom).ToString("hh:mm tt");
            }
        }

        protected async Task BtnTemplateText_ClickedAsync(object sender, EventArgs e, long stageId, string stageName)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (stageId == 0)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
                else
                {
                    var surgicalConciergeStageViewModel = SurgicalConciergeStageViewModelList.FirstOrDefault(item => item.StageId == stageId);

                    if (surgicalConciergeStageViewModel.HasStarted == false)
                    {
                        //var answer = await DisplayAlert("Confirm Start", "Are you sure you want to start this stage? In this case it will end any previous started stage.", "Start", "Cancel");
                        //if (answer)
                        //{

                        //}

                        String successResult = await ExecuteSurgicalStageStart(surgicalConciergeStageViewModel);
                        UtilHelper.ShowToastMessage(successResult);
                        CurrentActiveSurgicalConciergeStageViewModel = surgicalConciergeStageViewModel;

                        //if (CommentButton.IsVisible == false)
                        //{
                        //    CommentButton.IsVisible = true;
                        //    CommentButtonStackLayout.IsVisible = true;
                        //}
                    }
                    else
                    {

                    }
                }
            }
        }


        private void RecipientButton_Clicked(object sender, EventArgs e)
        {
            // Surgical Concierge Patient Add...
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    Navigation.PushAsync(new ProfessionalRecipientPage(SurgicalConciergePatientViewModel, (long)PracticeDivisionUnit.OperatingRoom));
                }
                catch (Exception)
                {
                    DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }
        //private void AttendeeAddButton_Clicked(object sender, EventArgs e)
        //{
        //    // Surgical Concierge Patient Add...
        //    if (!InternetConnectHelper.CheckConnection())
        //    {
        //        UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
        //        return;
        //    }
        //    using (UserDialogs.Instance.Loading(""))
        //    {
        //        try
        //        {
        //            Navigation.PushModalAsync(new SurgicalConceirgePatientAdd(this, true));
        //        }
        //        catch (Exception)
        //        {
        //            DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //        }
        //    }

        //}

        public void RebindData(SurgicalConciergeViewModel surgicalConciergeViewModel, SurgicalConciergePatientViewModel patient = null)
        {
            this.SurgicalConciergePatientViewModel = patient;
            this.SurgicalConciergeViewModel = surgicalConciergeViewModel;
            this.LoadData();
            ShowToolbar();
        }
    }
}