using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.UserControls;
using OntrackHealthApp.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientSurveyPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private bool IsSubmitVisibled  = false;
        //private string NextItem = "";
        PatientSurveyPageViewModel PatientSurveyPageViewModel { get; set; }

        private PatientSurveyClient PatientSurveyClient;

        public PatientSurveyPage(PatientNotificationShortView patientNotificationShortView)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();

                _iTokenContainer = new TokenContainer();

                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = _iTokenContainer.CurrentProcedureName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
                if (!_iTokenContainer.IsApiToken())
                {
                    //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                    App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                }
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                PatientSurveyClient = new PatientSurveyClient(apiClient);

                PatientSurveyPageViewModel = new PatientSurveyPageViewModel();
                PatientSurveyPageViewModel.NotificationId = patientNotificationShortView.NotificationId;
                //NotificationTitle.Text = patientNotificationShortView.NotificationTitle;
                BindingContext = PatientSurveyPageViewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientSurveyPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                if (!_iTokenContainer.IsApiToken())
                {
                    //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                    App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                }
                //NotificationTitle.Text = "Check-In Program";
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private void RadioButton_Clicked(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            var value = radioButton.Value;


            var t = value.ToString().Split(':').ToArray();
            var selectedValue = t[0];

            string NextItem = string.Empty;
            if (t.Length == 2)
            {
                NextItem = t[1];
            }

            // Set Group Selected Value
            RadioButtonGroupView ch = radioButton.Parent as RadioButtonGroupView;
            ch.ClassId = value.ToString();
            //Hide All Element/Block
            HideAllNextBock();
            ShowNextBocks();

        }

        private void RadioButtonClickedCommand(object sender, object Value, RadioButtonGroupView parent )
        {
            var value = Value;

            var t = value.ToString().Split(':').ToArray();
            var selectedValue = t[0];

            string NextItem = string.Empty;
            if (t.Length >= 2)
            {
                NextItem = t[1];
            }
            // Set Group Selected Value           
            parent.ClassId = value.ToString();
            //Hide All Element/Block
            HideAllNextBock();
            ShowNextBocks();

            var editor = parent.Children.Where(x => x.GetType() == typeof(HtmlLabel)).ToList();
            foreach (var item in editor)
            {
                var lbl = item as HtmlLabel;
                if (item != null)
                {
                    lbl.IsVisible = false;
                }
                if (lbl.ClassId == value + "__tim") {
                    lbl.IsVisible = true;
                }
            }
          
        }

        private void HideAllNextBock()
        {
            StackLayout block = Content.FindByName<StackLayout>("StackLayoutMainBlock");
            foreach (View i in (block.Children.Where(x => x.GetType() == typeof(StackLayout))))
            {
                if (i.ClassId != "FirstBlock")
                {
                    i.IsVisible = false;
                }
            }
        }

        private void ShowNextBocks()
        {
            //bool showSubmit = false;
            int loop = 0;
            int totalView = 0;
            string currentitem = "";
            StackLayout StackLayoutMainBlock = Content.FindByName<StackLayout>("StackLayoutMainBlock");

            List<View> _destViews = StackLayoutMainBlock.Children.Where(x => x.GetType() == typeof(StackLayout)).ToList();
            totalView = _destViews.Count();

            for (int i = 0; i < totalView; i++)
            {
                StackLayout stackLayoutChildBlock = _destViews[loop] as StackLayout;

                var labelGroupStackLayout = stackLayoutChildBlock.Children.FirstOrDefault() as StackLayout;
                labelGroupStackLayout.ClassId = string.Empty;
                if (stackLayoutChildBlock.Children.FirstOrDefault(x => x.ClassId == "slRadio") is StackLayout radioButtonGroupStackLayout)
                {
                    var radioButtonGroupView = radioButtonGroupStackLayout.Children.Where(x => x.GetType() == typeof(RadioButtonGroupView)).FirstOrDefault();// as RadioButtonGroupView;
                    if (radioButtonGroupView != null)
                    {
                        IsSubmitVisibled = false;
                        RadioButtonGroupView view = ((RadioButtonGroupView)radioButtonGroupView);
                        Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
                        if (!string.IsNullOrEmpty(view.ClassId))
                        {
                            var t = view.ClassId.ToString().Split(':').ToArray();
                            
                            if (t.Length >= 2)
                            {
                                currentitem = t[1];
                                StackLayout StackLayoutMainBlockFirstChild = StackLayoutMainBlock.Children.Where(x => x.ClassId == t[1]).First() as StackLayout;
                                StackLayoutMainBlockFirstChild.IsVisible = true;
                                labelGroupStackLayout.ClassId = "HasValue";                                
                                if (editor != null)
                                {
                                    IsSubmitVisibled = true;
                                }
                            }
                        }
                       
                    }
                }
                if(currentitem == "Class_Submit") { return; }
                loop++;
            }

        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox radioButton = sender as CheckBox;
            var value = radioButton.ClassId;
            var t = value.ToString().Split(':').ToArray();
            string nextItem = string.Empty;
            if (t.Length == 2)
            {
                nextItem = t[1];
            }
            // Set Group Selected Value
            RadioButtonGroupView ch = radioButton.Parent as RadioButtonGroupView;

            bool isCkecked = false;

            foreach (var item in ch.Children)
            {
                CheckBox checkbox = item as CheckBox;
                if (checkbox.IsChecked)
                {
                    isCkecked = true;
                }
            }
            if (isCkecked)
            {
                ch.ClassId = value;
            }
            else
            {
                ch.ClassId = string.Empty;
            }
            HideAllNextBock();
            ShowNextBocks();
        }

        private void CheckBoxCheckChangedCommand(object value, RadioButtonGroupView parent)
        {
            var t = value.ToString().Split(':').ToArray();
            string nextItem = string.Empty;
            if (t.Length >= 4 )
            {
                nextItem = t[1];
            }
            // Set Group Selected Value
            RadioButtonGroupView ch = parent;

            bool isCkecked = false;

            foreach (var item in ch.Children.Where(x=> x.GetType() == typeof(CheckBox)))
            {
                CheckBox checkbox = item as CheckBox;
                if (checkbox.IsChecked)
                {
                    isCkecked = true;
                }
            }
            if (isCkecked)
            {
                ch.ClassId = value.ToString();
            }
            else
            {
                ch.ClassId = string.Empty;
            }
            HideAllNextBock();
            ShowNextBocks();
        }

        private void CreateForm()
        {
            StackLayoutMainBlock.Children.Clear();
            SurveyDesc.Children.Clear();

            var patientSurveyPatientNotificationDetailViewModel = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel;
            if(patientSurveyPatientNotificationDetailViewModel != null)
            {
                string blockTwoClassId = "FirstBlock";
                bool showBlock = true;
                int loopOne = 0;

                var patientSurveyQuestionSetViewModel = patientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel;
                if (patientSurveyQuestionSetViewModel != null) { 
                    if (patientSurveyQuestionSetViewModel.HasQuestions) { 
                        PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel = patientSurveyQuestionSetViewModel;

                        HtmlLabel labelQuestionSetName = new HtmlLabel()
                        {
                            FontSize = 21,
                            Text = patientSurveyQuestionSetViewModel.SurveyQuestionSetNamePatient,
                            FontFamily = "Fonts/georgia.ttf#georgia",
                            TextColor = Color.FromHex("#222")
                        };
                        SurveyDesc.Children.Add(labelQuestionSetName);

                        HtmlLabel labelDescription = new HtmlLabel() {
                            FontSize = 19, Text = patientSurveyQuestionSetViewModel.SurveyQuestionSetHeader,
                            FontFamily = "Fonts/georgia.ttf#georgia",TextColor = Color.FromHex("#222")
                        };
                        SurveyDesc.Children.Add(labelDescription);

                        int totalQuestion = patientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels.Count();


                        foreach(var item in patientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels.OrderBy(x => x.QqnDisplayOrder))
                        {
                            RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView() ;
                        
                            foreach (var item2 in item.PatientSurveyQuestionDetailViewModels.OrderBy(x => x.DisplayOrder))
                            {

                                string classId = item2.SurveyQuestionDetailId.ToString() + ":Class_" + item2.NextQuestionId.ToString() + ":" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString();
                                if (item2.NextQuestionId == null) {
                                    classId = item2.SurveyQuestionDetailId.ToString() + ":Class_Submit:" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString(); ;
                                }

                                //if (item.SurveyQuestionTypeId == 1 || item.SurveyQuestionTypeId == 2)
                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt()
                                    || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt())
                                {
                                    classId = classId + ":R";
                                    RadioButton radio = new RadioButton
                                    {
                                        Text = item2.QuestionDetailText,
                                        CircleSize = 32,
                                        TextFontSize = 20,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Value = classId,
                                        Spacing = 5,
                                        ClickCommand = new Command((tag) =>
                                        {
                                            RadioButtonClickedCommand(tag, classId, radioButtonGroupView);
                                        }),
                                        FontFamily = "Fonts/georgia.ttf#georgia",
                                        TextColor = Color.FromHex("#222"),
                                    };
                                    radioButtonGroupView.Children.Add(radio);
                                    if (!string.IsNullOrEmpty(item2.OptionSuggestion))
                                    {
                                        HtmlLabel lbl_two = new HtmlLabel {
                                            Text = item2.OptionSuggestion,
                                            FontFamily = "Fonts/georgia.ttf#georgia",
                                            FontSize =13, Margin = new Thickness(30,0,0,10),
                                            IsVisible = false,
                                            ClassId = classId + "__tim",
                                            TextColor = Color.FromHex("#222")
                                        };
                                        radioButtonGroupView.Children.Add(lbl_two);
                                    }
                                    BoxView bxRadio = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                    radioButtonGroupView.Children.Add(bxRadio);
                                }
                                //else if (item.SurveyQuestionTypeId == 4)
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                                {
                                    //Checkbox
                                    classId = classId + ":C";
                                    CheckBox check = new CheckBox
                                    {
                                        Text = item2.QuestionDetailText,
                                        BoxSizeRequest = 26,
                                        TextFontSize = 20,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        ClassId = classId,
                                        Spacing = 5,
                                        IsChecked = false,
                                        CheckChangedCommand = new Command((tag) => {
                                            CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                                        }),
                                        FontFamily = "Fonts/georgia.ttf#georgia",
                                        TextColor = Color.FromHex("#222")
                                    };
                                    radioButtonGroupView.Children.Add(check);
                                    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                    radioButtonGroupView.Children.Add(bxCheck);
                                }
                                //else if (item.SurveyQuestionTypeId == 3)
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                                {
                                    //Textbox
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        HeightRequest = 100
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    //radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }
                                //else if (item.SurveyQuestionTypeId == 5)
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                                {
                                    //Date
                                    classId = classId + ":D";
                                    DatePicker datePicker = new DatePicker()
                                    {
                                        ClassId = classId,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        HeightRequest = 50
                                    };
                                    datePicker.DateSelected += DatePicker_DateSelected;
                                    //radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(datePicker);
                                }
                            }

                            StackLayout qsn = new StackLayout {
                                VerticalOptions =  LayoutOptions.StartAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Padding = 10,
                                BackgroundColor = Color.FromHex("#d6f7fe")
                            };
                            Label lblQsn = new Label {
                                Text = item.SurveyQuestionText,
                                TextColor = Color.FromHex("#446377"),
                                FontSize = 18,FontFamily = "Fonts/georgia.ttf#georgia"
                            };
                            qsn.Children.Add(lblQsn);


                            if (blockTwoClassId == "")
                            {
                                blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString();
                                showBlock = false;
                            }
                            StackLayout BlockTwo = new StackLayout { ClassId = blockTwoClassId, BackgroundColor = Color.FromHex("#f0fcfe") , IsVisible = showBlock };
                            if (item.SurveyQuestionTypeId == 4) {
                                //BlockTwo.Margin = new Thickness(-15, 0, 0, 0);
                            }
                            BlockTwo.Children.Add(qsn);


                            StackLayout StackLayoutSlRadio = new StackLayout { ClassId = "slRadio", Padding = new Thickness(10,2) };
                            StackLayoutSlRadio.Children.Add(radioButtonGroupView);                    
                   
                            BlockTwo.Children.Add(StackLayoutSlRadio);
                            blockTwoClassId = "";

                            StackLayoutMainBlock.Children.Add(BlockTwo);

                            loopOne += 1;
                        }
                        string buttonText = string.Empty;
                        if (patientSurveyQuestionSetViewModel.IsLastQuestionSet)
                        {
                            buttonText = "Submit";
                        }
                        else
                        {
                            buttonText = "Submit and Continue"; 
                        }

                        StackLayout buttonMain = new StackLayout { ClassId = "Class_Submit", IsVisible = false, Margin = new Thickness(0, 15) };
                        StackLayout buttonMainSlRadio = new StackLayout { ClassId = "slRadio", Padding = 2 };
                        RadioButtonGroupView buttonGroupView = new RadioButtonGroupView();

                        ButtonEditExtended buttonSubmit = new ButtonEditExtended { Text = buttonText, FontSize = 22 };
                        buttonSubmit.Clicked += async delegate { await SubmitButtonClickedAsync(); };

                        buttonGroupView.Children.Add(buttonSubmit);
                        buttonMainSlRadio.Children.Add(buttonGroupView);
                        buttonMain.Children.Add(buttonMainSlRadio);
                        StackLayoutMainBlock.Children.Add(buttonMain);
                    }
                    else
                    {
                        string msg = "Today's Check-In Program is now complete.";
                        if(patientSurveyPatientNotificationDetailViewModel.IsLastNotification)
                        {
                            msg = "Thank you for your participation. We wish you the best of health.";
                        }

                        StackLayout stackMsg = new StackLayout { Margin = new Thickness(0, 40), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, BackgroundColor = Color.FromHex("#d6f7fe"), Padding = new Thickness(20) };
                        Label lblMsg = new Label { FontSize = 22, Text = msg, Margin = new Thickness(0,0,0,40) };
                        stackMsg.Children.Add(lblMsg);


                        ButtonEditExtended buttonGoHome = new ButtonEditExtended { Text = "Home", FontSize = 22 };
                        buttonGoHome.Clicked +=  (s, e) => {

                            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
                            App.Current.MainPage = new MenuPage(userIdentityModel);
                            //await Navigation.PushAsync(new MainPatientPage());                        
                        }; //delegate { await SubmitButtonClickedAsync(); };

                        stackMsg.Children.Add(buttonGoHome);
                        StackLayoutMainBlock.Children.Add(stackMsg);
                        SurveyDesc.IsVisible = false;
                    }
                }
            }
            App.HideUserDialogAsync();
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        { 
            if (IsSubmitVisibled) return;
            Editor editor = sender as Editor; 
            RadioButtonGroupView ch = editor.Parent as RadioButtonGroupView;


            var value = editor.ClassId;
            var t = value.ToString().Split(':').ToArray();

            string nextItem = string.Empty;
            if (t.Length == 2)
            {
                nextItem = t[1];
            }

            // Set Group Selected Value

            ch.ClassId = value.ToString();
            //Hide All Element/Block
            HideAllNextBock();
            ShowNextBocks();
            editor.Focus();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (IsSubmitVisibled) return;
            DatePicker datePicker = sender as DatePicker;
            RadioButtonGroupView ch = datePicker.Parent as RadioButtonGroupView;


            var value = datePicker.ClassId;
            var t = value.ToString().Split(':').ToArray();

            string nextItem = string.Empty;
            if (t.Length == 2)
            {
                nextItem = t[1];
            }

            // Set Group Selected Value

            ch.ClassId = value.ToString();
            //Hide All Element/Block
            HideAllNextBock();
            ShowNextBocks();
        }

        public async Task SubmitButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await SaveForm();
                    await ReLoadSurveyWithNextQuestionSet();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        //public async Task SaveForm()
        //{
        //    var viewModel = PatientSurveyPageViewModel.PatientSurveyProcedureNotificationViewModel;
        //    viewModel.PracticeProfileId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PracticeProfileId;
        //    viewModel.ProfessionalProfileId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.ProfessionalProfileId;
        //    viewModel.ProcedureId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.ProcedureId;
        //    viewModel.PatientNotificationDetailId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientNotificationDetailId;
        //    viewModel.NotificationId = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.NotificationId;
        //    viewModel.NotificationOrder= PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.NotificationOrder;
        //    viewModel.PatientProcedureDetailId = (Guid)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientProcedureDetailId;
        //    viewModel.SurveyQuestionSetId = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel.SurveyQuestionSetId;

        //    var dd = PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels;

        //    StackLayout StackLayoutMainBlock = Content.FindByName<StackLayout>("StackLayoutMainBlock");
        //    foreach (View i in (StackLayoutMainBlock.Children.Where(x => x.GetType() == typeof(StackLayout))))
        //    {
        //        StackLayout stackLayoutChildBlock = i as StackLayout;
        //        var labelGroupStackLayout = stackLayoutChildBlock.Children.FirstOrDefault() as StackLayout;
        //        string HasValue = labelGroupStackLayout.ClassId;

        //        if (stackLayoutChildBlock.Children.FirstOrDefault(x => x.ClassId == "slRadio") is StackLayout radioButtonGroupStackLayout)
        //        {
        //            var radioButtonGroupView = radioButtonGroupStackLayout.Children.Where(x => x.GetType() == typeof(RadioButtonGroupView)).FirstOrDefault();

        //            if (radioButtonGroupView != null)
        //            {
        //                RadioButtonGroupView view = ((RadioButtonGroupView)radioButtonGroupView);

        //                if (!string.IsNullOrEmpty(view.ClassId))
        //                {
        //                    //item2.SurveyQuestionDetailId.ToString() + ":Class_" + item2.NextQuestionId.ToString() + ":" + item2.SurveyQuestionSetId.ToString()
        //                    var t = view.ClassId.ToString().Split(':').ToArray();
        //                    if (t.Length >= 5)
        //                    {
        //                        if(t[4].ToString() == "C")
        //                        {
        //                            long surveyQuestionId = Convert.ToInt64(t[3]);
        //                            var details = dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().PatientSurveyQuestionDetailViewModels;

        //                            foreach (var item in view.Children.Where(x => x.GetType() == typeof(CheckBox)))
        //                            {
        //                                CheckBox checkbox = item as CheckBox;
        //                                if (checkbox.IsChecked)
        //                                {
        //                                    var itemSelected = checkbox.ClassId.ToString().Split(':').ToArray();

        //                                    long surveyQuestionDetailId = Convert.ToInt64(itemSelected[0]);
        //                                    long checkboxSurveyQuestionId = Convert.ToInt64(itemSelected[3]);
        //                                    int checkboxSetId = Convert.ToInt32(itemSelected[2]);                                        
        //                                    if(details != null)
        //                                    {
        //                                        details.Where(x => x.SurveyQuestionDetailId == surveyQuestionDetailId).FirstOrDefault().IsSelected = true;
        //                                    }
        //                                }
        //                            }

        //                        }
        //                        else if (t[4].ToString() == "R")
        //                        {
        //                            string selectedAnsware = t[0].ToString();
        //                            long surveyQuestionId = Convert.ToInt64(t[3]);
        //                            int setId = Convert.ToInt32(t[2]);
        //                            dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
        //                        }
        //                        else if (t[4].ToString() == "T")
        //                        {
        //                            Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
        //                            if (editor != null)
        //                            {
        //                                string selectedAnsware = editor.Text;
        //                                long surveyQuestionId = Convert.ToInt64(t[3]);
        //                                int setId = Convert.ToInt32(t[2]);
        //                                dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    viewModel.PatientSurveyQuestionSetViewModels.Add(PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel);
        //    try
        //    {
        //        //var data =  await PatientSurveyClient.PostPatientSurvey(viewModel);
        //    }
        //    catch (Exception)
        //    {
        //        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
        //    }        
        //}


        public async Task SaveForm()
        {
            var notificationDetail = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel;
            
            var viewModel = new PatientSurveyProcedureNotificationCreateModel();
            viewModel.PrId = (long)notificationDetail.PracticeProfileId;
            viewModel.ProId = (long)notificationDetail.ProfessionalProfileId;
            viewModel.ProcId = (long)notificationDetail.ProcedureId;
            viewModel.PaNoDtId = (long)notificationDetail.PatientNotificationDetailId;
            viewModel.NoId = notificationDetail.NotificationId;
            viewModel.Order = notificationDetail.NotificationOrder;
            viewModel.ProcdId = (Guid)notificationDetail.PatientProcedureDetailId;
            viewModel.SetId = notificationDetail.PatientSurveyQuestionSetViewModel.SurveyQuestionSetId;
            viewModel.PaId = _iTokenContainer.ApiPatientProfileId.ToLong();

            var patientSurveyQuestionViewModels = PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels;

            StackLayout StackLayoutMainBlock = Content.FindByName<StackLayout>("StackLayoutMainBlock");
            foreach (View i in (StackLayoutMainBlock.Children.Where(x => x.GetType() == typeof(StackLayout))))
            {
                StackLayout stackLayoutChildBlock = i as StackLayout;
                var labelGroupStackLayout = stackLayoutChildBlock.Children.FirstOrDefault() as StackLayout;
                string HasValue = labelGroupStackLayout.ClassId;

                if (stackLayoutChildBlock.Children.FirstOrDefault(x => x.ClassId == "slRadio") is StackLayout radioButtonGroupStackLayout)
                {
                    var radioButtonGroupView = radioButtonGroupStackLayout.Children.Where(x => x.GetType() == typeof(RadioButtonGroupView)).FirstOrDefault();

                    if (radioButtonGroupView != null)
                    {
                        RadioButtonGroupView view = ((RadioButtonGroupView)radioButtonGroupView);

                        if (!string.IsNullOrEmpty(view.ClassId))
                        {
                            //item2.SurveyQuestionDetailId.ToString() + ":Class_" + item2.NextQuestionId.ToString() + ":" + item2.SurveyQuestionSetId.ToString()
                            var t = view.ClassId.ToString().Split(':').ToArray();
                            if (t.Length >= 5)
                            {
                                if (t[4].ToString() == "C")
                                {
                                    long surveyQuestionId = Convert.ToInt64(t[3]);
                                    var details = patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().PatientSurveyQuestionDetailViewModels;

                                    foreach (var item in view.Children.Where(x => x.GetType() == typeof(CheckBox)))
                                    {
                                        CheckBox checkbox = item as CheckBox;
                                        if (checkbox.IsChecked)
                                        {
                                            var itemSelected = checkbox.ClassId.ToString().Split(':').ToArray();

                                            long surveyQuestionDetailId = Convert.ToInt64(itemSelected[0]);
                                            long checkboxSurveyQuestionId = Convert.ToInt64(itemSelected[3]);
                                            int checkboxSetId = Convert.ToInt32(itemSelected[2]);
                                            if (details != null)
                                            {
                                                details.Where(x => x.SurveyQuestionDetailId == surveyQuestionDetailId).FirstOrDefault().IsSelected = true;
                                                details.Where(x => x.SurveyQuestionDetailId == surveyQuestionDetailId).FirstOrDefault().SelectedAnsware = surveyQuestionDetailId.ToString();
                                            }
                                        }
                                    }

                                }
                                else if (t[4].ToString() == "R")
                                {
                                    string selectedAnsware = t[0].ToString();
                                    long surveyQuestionId = Convert.ToInt64(t[3]);
                                    int setId = Convert.ToInt32(t[2]);
                                    //bool checkIfDependentQuestion = patientSurveyQuestionViewModels.Where(x => x.PatientSurveyQuestionDetailViewModels.Any(y => y.NextQuestionId == surveyQuestionId && y.SurveyQuestionDetailId != Convert.ToInt64(x.SelectedAnsware))).Any();
                                    //if (!checkIfDependentQuestion)
                                    //{
                                    //    patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                    //}
                                    //else
                                    //{
                                    //    patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = null;
                                    //}

                                    patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                }
                                else if (t[4].ToString() == "T")
                                {
                                    Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
                                    if (editor != null)
                                    {
                                        string selectedAnsware = editor.Text;
                                        long surveyQuestionId = Convert.ToInt64(t[3]);
                                        int setId = Convert.ToInt32(t[2]);
                                        patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                    }

                                }
                                else if (t[4].ToString() == "D")
                                {
                                    DatePicker datePicker = view.Children.Where(x => x.GetType() == typeof(DatePicker)).FirstOrDefault() as DatePicker;
                                    if (datePicker != null)
                                    {
                                        DateTime selectedAnswareDate = datePicker.Date;
                                        //string selectedAnsware = selectedAnswareDate.ToString("dd/MMM/yyyy hh:mm tt");
                                        string selectedAnsware = selectedAnswareDate.ToString("MM/dd/yyyy");
                                        long surveyQuestionId = Convert.ToInt64(t[3]);
                                        int setId = Convert.ToInt32(t[2]);
                                        patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            foreach(var item in patientSurveyQuestionViewModels)
            {
                PatientSurveyQuestionCreateModel modelOne = new PatientSurveyQuestionCreateModel();
                modelOne.SeAns = item.SelectedAnsware;
                modelOne.QsnId = item.SurveyQuestionId;
                modelOne.SetId = item.SurveyQuestionSetId;
                modelOne.SqTypeId = item.SurveyQuestionTypeId;

                foreach (var detail in item.PatientSurveyQuestionDetailViewModels)
                {
                    PatientSurveyQuestionDetailCreateModel modelTwo = new PatientSurveyQuestionDetailCreateModel();
                    modelTwo.Sa = detail.SelectedAnsware;
                    modelTwo.SqdId = detail.SurveyQuestionDetailId;
                    modelTwo.SqId = detail.SurveyQuestionId;
                    modelTwo.SetId = detail.SurveyQuestionSetId;
                    modelTwo.Dv = detail.DefaultValue;
                    modelTwo.DnnSetId = detail.DoNotShowNextSetId;
                    modelTwo.DnnTime = detail.DoNotShowNextTime;
                    modelTwo.Qdv = detail.QuestionDetailValue;

                    if (modelOne.SqTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                    {
                        if (detail.IsSelected)
                        {
                            modelTwo.Sel = true;
                        }
                    }
                    else
                    {
                        if (detail.SurveyQuestionDetailId.GetValueOrDefault().ToString() == modelOne.SeAns)
                        {
                            modelTwo.Sel = true;
                        }
                    }
                    modelOne.PatientSurveyQuestionDetailCreateModels.Add(modelTwo);
                }
                viewModel.PatientSurveyQuestionCreateModels.Add(modelOne);
            }            
            try
            {
                PatientSurveyClientNew patientSurveyClientNew = new PatientSurveyClientNew();
                var data = await patientSurveyClientNew.PostPatientSurvey(viewModel);
                //var data =  await PatientSurveyClient.PostPatientSurvey(viewModel);
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        public async Task ReLoadSurveyWithNextQuestionSet()
        {
            var viewModel = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel;
            var response = await  PatientSurveyClient.GetNextPatientSurvey(viewModel.PracticeProfileId.ToString(), viewModel.ProfessionalProfileId.ToString(), viewModel.PatientProcedureDetailId.ToString(), viewModel.NotificationId.ToString());
            if (response.StatusIsSuccessful)
            {
                PatientSurveyQuestionSetViewModel patientSurveyQuestionSetViewModel = response.Data;
                PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel = patientSurveyQuestionSetViewModel;
                CreateForm();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            //ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
            App.ShowUserDialogAsync();
            await PatientSurveyPageViewModel.ExecuteLoadPatientSurveyNotificationDetailCommandAsync();
            CreateForm();
        }

    }
}