using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Extensions;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using OntrackHealthApp.UserControls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePacuPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergePacuPageViewModel _surgicalConciergePacuPageViewModel;
        private SurgicalConciergePatientViewModel _surgicalConciergePatientViewModel { get; set; }

        ScgPacuTimeModel _scgPacuTimeModel = new ScgPacuTimeModel();
        
        private SurgicalConciergeRestApiService restApiService;

        public SurgicalConciergePacuPage(SurgicalConciergePatientViewModel surgicalConciergeViewModel)
        {
            InitializeComponent();
            if (InternetConnectHelper.CheckConnection())
            {
                _iTokenContainer = new TokenContainer();
                if (_iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomPersonnel
               || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomNurse
               || _iTokenContainer.ApiUserRole == RoleIdConstants.OperatingRoomMD)
                {
                    Title = _iTokenContainer.ApiPracticeLocationName;
                }
                else
                {
                    Title = _iTokenContainer.ApiPracticeName;
                }

                _surgicalConciergePatientViewModel = surgicalConciergeViewModel;
                BindingContext = _surgicalConciergePacuPageViewModel = new SurgicalConciergePacuPageViewModel();

                _surgicalConciergePacuPageViewModel.PatientProfileId = surgicalConciergeViewModel.PatientProfileId;
                _surgicalConciergePacuPageViewModel.PatientProcedureDetailId = surgicalConciergeViewModel.PatientProcedureDetailId.ToString();
                restApiService = new SurgicalConciergeRestApiService();

                //ShowToolbar();

                PatientFullName.Text = surgicalConciergeViewModel.PatientFullName;
                ProcedureName.Text = surgicalConciergeViewModel.ProcedureName;
                ProfessionalName.Text = surgicalConciergeViewModel.ProfessionalName;

                MainContainer.IsVisible = false;

                LoadData();

            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadData()
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await _surgicalConciergePacuPageViewModel.ExecuteLoadSurgicalConciergePacuAsync();
                var data = _surgicalConciergePacuPageViewModel.ScgPacuCommentViewModels;
                
                _scgPacuTimeModel = await restApiService.GetScgPacuTime(_surgicalConciergePatientViewModel.ProfessionalProfileId.Value, _surgicalConciergePatientViewModel.ProcedureId.Value, _surgicalConciergePatientViewModel.PatientProfileId, _surgicalConciergePatientViewModel.PatientProcedureDetailId.Value);
                
                CreateForm(data);
            }
        }

        private void CreateForm(ObservableCollection<ScgPacuCommentViewModel> data)
        {
            MainContainer.Children.Clear();

            foreach (var item in data)
            {
                if (item.HasFreeTextBox == false)
                {
                    StackLayout firstContent = new StackLayout
                    {
                        Padding = new Thickness(2),
                        BackgroundColor = Color.FromHex("#e8edf1"),
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    Image img = new Image
                    {
                        Source = item.ScgPacuCommentImageName.ToProperImageSource()
                    };
                    firstContent.Children.Add(img);


                    StackLayout secontContent = new StackLayout
                    {
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    Label lbl = new Label
                    {
                        Text = item.ScgPacuCommentTitle,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 20,
                        HorizontalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    secontContent.Children.Add(lbl);

                    if (!string.IsNullOrEmpty(item.ScgPacuCommentText))
                    {
                        HtmlLabel label = new HtmlLabel();
                        label.Text = $"<html><body style=\"height:100%;font-size:16px\">{item.ScgPacuCommentText}</body></html>";
                        //var webView = new WebViewExtented();
                        //var htmlSource = new HtmlWebViewSource();
                        //htmlSource.Html = $"<html><body style=\"height:100%;font-size:16px\">{item.ScgPacuCommentText}</body></html>";
                        //webView.AutoContentSize = true;
                        //webView.Source = htmlSource;
                        secontContent.Children.Add(label);
                    }

                    Button submitButton = new Button
                    {
                        HeightRequest = 48,
                        CornerRadius = 24,
                        BackgroundColor = Color.FromHex("#004D80"),
                        TextColor = Color.FromHex("#FFF"),
                        FontSize = 16,
                        Text = "Send",
                        Margin = new Thickness(0, 5),
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    };
                    submitButton.Clicked += async (sender, args) => await btnTemplateText_ClickedAsync(sender, args, item.ScgPacuCommentId, item.ScgPacuCommentText, firstContent, lbl, item);

                    StackLayout fourthContent = new StackLayout
                    {
                        Padding = new Thickness(12, 6, 12, 0),
                        BackgroundColor = Color.Transparent,
                    };
                    fourthContent.Children.Add(submitButton);

                    StackLayout fifthContent = new StackLayout
                    {
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    fifthContent.Children.Add(firstContent);
                    fifthContent.Children.Add(secontContent);
                    fifthContent.Children.Add(fourthContent);

                    Frame outerContent = new Frame
                    {
                        BorderColor = Color.FromHex("#004D80"),
                        CornerRadius = 6,
                        BackgroundColor = Color.FromHex("#e8edf1"),
                        Margin = new Thickness(0, 0, 0, 20)
                    };

                    outerContent.Content = fifthContent;

                    MainContainer.Children.Add(outerContent);
                }
                else
                {
                    if (item.IsAdditionalComment)
                    {
                        CreateFormAddFreeText(item);
                    }
                    else
                    {
                        CreateFormAddNumericMixText(item);
                    }
                }
            }
            if (_scgPacuTimeModel.ORStartTime != null && _scgPacuTimeModel.EstimatedEndTime != null)
            {
                //PACUStartEndtTimeLayout.IsVisible = true;
                ORCompleted.Text =  _scgPacuTimeModel.ORStartTime;
                EstimatedFloorArrival.Text =  _scgPacuTimeModel.EstimatedEndTime;
            }
            
        }

        private void CreateFormAddFreeText(ScgPacuCommentViewModel data)
        {
            StackLayout MainContainerInner = new StackLayout { BackgroundColor = Color.Transparent };
            Frame outerContent = new Frame
            {
                BorderColor = Color.FromHex("#004D80"),
                CornerRadius = 6,
                BackgroundColor = Color.FromHex("#e8edf1"),
                Margin = new Thickness(0, 0, 0, 10)
            };
            StackLayout firstContent = new StackLayout
            {
                BackgroundColor = Color.FromHex("#e8edf1"),
                Padding = new Thickness(12, 0, 12, 12)
            };
            Image img = new Image
            {
                Source = data.ScgPacuCommentImageName.ToProperImageSource()
            };
            firstContent.Children.Add(img);

            StackLayout secondContent = new StackLayout
            {
                Padding = new Thickness(12, 0, 12, 12),
                BackgroundColor = Color.Transparent,
            };
            Label lbl = new Label
            {
                Text = "Type Message",
                TextColor = Color.FromHex("#000"),
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            secondContent.Children.Add(lbl);

            StackLayout thirdContent = new StackLayout
            {
                Padding = new Thickness(12),
                BackgroundColor = Color.Transparent,
            };
            MtiEditor editor = new MtiEditor
            {
                Text = "",
                FontSize = 20,
                HeightRequest = 100,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            thirdContent.Children.Add(editor);

            Button submitButton = new Button
            {
                HeightRequest = 48,
                CornerRadius = 24,
                BackgroundColor = Color.FromHex("#004D80"),
                TextColor = Color.FromHex("#FFF"),
                FontSize = 16,
                Text = "Add Comment",
                Margin = new Thickness(0, 5),
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            submitButton.Clicked += async (sender, args) => await BtnTemplateAndFreeText_ClickedAsync(sender, args, lbl, editor, firstContent, data);
            StackLayout StackLayoutSubmitButton = new StackLayout
            {
                Padding = new Thickness(12, 6, 12, 0),
                BackgroundColor = Color.Transparent,
            };
            StackLayoutSubmitButton.Children.Add(submitButton);

            StackLayout fourthContent = new StackLayout
            {
                BackgroundColor = Color.Transparent
            };

            fourthContent.Children.Add(firstContent);
            fourthContent.Children.Add(secondContent);
            fourthContent.Children.Add(thirdContent);
            fourthContent.Children.Add(StackLayoutSubmitButton);

            outerContent.Content = fourthContent;
            MainContainer.Children.Add(outerContent);
        }

        private void CreateFormAddNumericMixText(ScgPacuCommentViewModel data)
        {
            StackLayout MainContainerInner = new StackLayout { BackgroundColor = Color.Transparent };
            Frame outerContent = new Frame
            {
                BorderColor = Color.FromHex("#004D80"),
                CornerRadius = 6,
                BackgroundColor = Color.FromHex("#e8edf1"),
                Margin = new Thickness(0, 0, 0, 10)
            };
            StackLayout firstContent = new StackLayout
            {
                BackgroundColor = Color.FromHex("#e8edf1"),
                Padding = new Thickness(12, 0, 12, 12)
            };
            Image img = new Image
            {
                Source = data.ScgPacuCommentImageName.ToProperImageSource()
            };
            firstContent.Children.Add(img);

            StackLayout secondContent = new StackLayout
            {
                Padding = new Thickness(12, 0, 12, 12),
                BackgroundColor = Color.Transparent,
            };
            Label lbl = new Label
            {
                Text = data.ScgPacuCommentTextForSms,
                TextColor = Color.FromHex("#000"),
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            secondContent.Children.Add(lbl);

            StackLayout thirdContent = new StackLayout
            {
                Padding = new Thickness(12, 0, 12, 12),
                BackgroundColor = Color.Transparent,
            };
            MtiEntry editor = new MtiEntry
            {
                Text = "",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            thirdContent.Children.Add(editor);


            Button submitButton = new Button
            {
                HeightRequest = 48,
                CornerRadius = 24,
                BackgroundColor = Color.FromHex("#004D80"),
                TextColor = Color.FromHex("#FFF"),
                FontSize = 16,
                Text = "Send",
                Margin = new Thickness(0, 5),
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            submitButton.Clicked += async (sender, args) => await BtnTemplateAndNumericMixText_ClickedAsync(submitButton, args, editor, data.ScgPacuCommentId, data.ScgPacuCommentText, firstContent, lbl, data);

            StackLayout stackLayoutSubmitButton = new StackLayout
            {
                Padding = new Thickness(12, 6, 12, 0),
                BackgroundColor = Color.Transparent,
            };
            stackLayoutSubmitButton.Children.Add(submitButton);

            //Grid grdLayoutButton = new Grid();
            //grdLayoutButton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            //grdLayoutButton.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            //grdLayoutButton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            //grdLayoutButton.Children.Add(submitButton, 1, 0);

            StackLayout fourthContent = new StackLayout
            {
                BackgroundColor = Color.Transparent
            };

            fourthContent.Children.Add(firstContent);
            fourthContent.Children.Add(secondContent);
            fourthContent.Children.Add(thirdContent);
            fourthContent.Children.Add(stackLayoutSubmitButton);

            outerContent.Content = fourthContent;
            MainContainer.Children.Add(outerContent);
        }

        protected async Task btnTemplateText_ClickedAsync(object sender, EventArgs e, long scgPacuCommentId, string template, StackLayout firstContent, Label label, ScgPacuCommentViewModel selectedData)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                if (template == "")
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
                }
                else
                {
                    var submitButton = sender as Button;

                    SurgicalConciergePacuCommentParamViewModel paramModel = new SurgicalConciergePacuCommentParamViewModel();
                    paramModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
                    paramModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                    paramModel.ProcedureId = _surgicalConciergePatientViewModel.ProcedureId.ToLong();
                    //paramModel.ScgPacuCommentText = template;
                    paramModel.ScgPacuCommentText = string.Empty;
                    //paramModel.ScgPacuAdditionalComment = submitButton.ClassId;
                    paramModel.ScgPacuAdditionalComment = string.Empty;
                    paramModel.ScgPacuCommentId = selectedData.ScgPacuCommentId;
                    paramModel.HasFreeTextBox = selectedData.HasFreeTextBox;

                    var result = await _surgicalConciergePacuPageViewModel.ScgPacuCommentSendAsync(paramModel);
                    if (result)
                    {
                        //await DisplayAlert(AppConstant.ToastMessageTitle, AppConstant.PacuSuccessMessage, AppConstant.DisplayAlertErrorButtonText);
                        UtilHelper.ShowToastMessage(AppConstant.PacuSuccessMessage);

                        submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                        submitButton.TextColor = Color.FromHex("#000");
                        submitButton.Text = "Delivered";
                        submitButton.IsEnabled = false;

                        //firstContent.BackgroundColor = Color.FromHex("#004c7e");

                        //label.TextColor = Color.FromHex("#FFF");
                    }
                    else
                    {
                        UtilHelper.ShowToastMessage(AppConstant.PacuErrorMessage);
                    }
                }
            }
        }

        protected async Task btnAdditionalComment_ClickedAsync(object sender, EventArgs args, Button btnSource)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushModalAsync(new SurgicalConciergePacuAdditionalCommentPage(_iTokenContainer.ApiPracticeName, btnSource));
            }
        }

        protected async Task BtnTemplateAndFreeText_ClickedAsync(object sender, EventArgs e, Label label, MtiEditor editor, StackLayout firstContent, ScgPacuCommentViewModel selectedData)
        {
            bool result = false;

            if (editor.Text == "")
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);

            }
            else
            {
                SurgicalConciergePacuCommentParamViewModel paramModel = new SurgicalConciergePacuCommentParamViewModel();
                paramModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
                paramModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                paramModel.ProcedureId = _surgicalConciergePatientViewModel.ProcedureId.ToLong();
                paramModel.ScgPacuCommentId = selectedData.ScgPacuCommentId;
                paramModel.HasFreeTextBox = selectedData.HasFreeTextBox;
                string scgPacuCommentText = editor.Text?.Trim();
                paramModel.ScgPacuCommentText = scgPacuCommentText;
                paramModel.ScgPacuAdditionalComment = scgPacuCommentText;
                using (UserDialogs.Instance.Loading(""))
                {
                    result = await _surgicalConciergePacuPageViewModel.ScgPacuAdditionalCommentSendAsync(paramModel);
                }
                if (result)
                {
                    UtilHelper.ShowToastMessage(AppConstant.PacuSuccessMessage);
                    editor.Text = "";
                }
                else
                {
                    UtilHelper.ShowToastMessage(AppConstant.PacuErrorMessage);
                }
            }

        }

        protected async Task btnAdditionalCommentAndNumeric_ClickedAsync(object sender, EventArgs args, Button btnSource)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushModalAsync(new SurgicalConciergePacuAdditionalCommentPage(_iTokenContainer.ApiPracticeName, btnSource));
            }
        }

        protected async Task BtnTemplateAndNumericMixText_ClickedAsync(object sender, EventArgs e, MtiEntry editor, long scgPacuCommentId, string template, StackLayout firstContent, Label label, ScgPacuCommentViewModel selectedData)
        {
            bool result = false;
            if (editor.Text == "")
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NullReferenceExceptionError, AppConstant.DisplayAlertErrorButtonText);
            }
            else
            {
                var submitButton = sender as Button;

                SurgicalConciergePacuCommentParamViewModel paramModel = new SurgicalConciergePacuCommentParamViewModel();
                paramModel.PatientProcedureDetailId = _surgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid();
                paramModel.PatientProfileId = _surgicalConciergePatientViewModel.PatientProfileId;
                paramModel.ProcedureId = _surgicalConciergePatientViewModel.ProcedureId.ToLong();

                paramModel.ScgPacuAdditionalComment = string.Empty;
                paramModel.ScgPacuCommentId = selectedData.ScgPacuCommentId;
                paramModel.HasFreeTextBox = selectedData.HasFreeTextBox;
                //string scgPacuAdditionalComment = submitButton.ClassId;
                //paramModel.ScgPacuAdditionalComment = template;

                //paramModel.ScgPacuCommentText = template;
                paramModel.ScgPacuCommentText = string.Empty;

                string scgPacuCommentTextRoomNo = editor.Text?.Trim();
                paramModel.ScgPacuCommentTextRoomNo = scgPacuCommentTextRoomNo;
                //paramModel.ScgPacuAdditionalComment = scgPacuAdditionalComment;
                paramModel.ScgPacuAdditionalComment = string.Empty;
                using (UserDialogs.Instance.Loading(""))
                {
                    result = await _surgicalConciergePacuPageViewModel.ScgPacuCommentSendAsync(paramModel);
                }
                if (result)
                {
                    //await DisplayAlert(AppConstant.ToastMessageTitle, AppConstant.PacuSuccessMessage, AppConstant.DisplayAlertErrorButtonText);
                    UtilHelper.ShowToastMessage(AppConstant.PacuSuccessMessage);

                    submitButton.BackgroundColor = Color.FromHex("#f0ad4e");
                    submitButton.TextColor = Color.FromHex("#000");
                    submitButton.Text = "Delivered";
                    submitButton.IsEnabled = false;

                    editor.Text = "";

                    //firstContent.BackgroundColor = Color.FromHex("#004c7e");

                    //label.TextColor = Color.FromHex("#FFF");
                }
                else
                {
                    UtilHelper.ShowToastMessage(AppConstant.PacuErrorMessage);
                }
            }


        }

        protected async Task btnFreeFormCommentModalPage_ClickedAsync(object sender, EventArgs args)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                await Navigation.PushModalAsync(new SurgicalConciergePacuFreeFormCommentPage(_surgicalConciergePatientViewModel));
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
            MainContainer.IsVisible = true;
        }

        private void ShowToolbar()
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
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.PACU));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
            }
            else
            {
                var toolbarItem = new ToolbarItem("Recipient", null, () =>
                {
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.PACU));
                }, 0, 0);
                toolbarItem.Text = "Recipient";
                ToolbarItems.Add(toolbarItem);
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
                    Navigation.PushAsync(new SurgicalConciergeRecipientPage(_surgicalConciergePatientViewModel, (long)PracticeDivisionUnit.OperatingRoom));
                }
                catch (Exception)
                {
                    DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }
    }
}