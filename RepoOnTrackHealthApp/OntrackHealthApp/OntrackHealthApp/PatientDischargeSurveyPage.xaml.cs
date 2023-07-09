using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiService.Client;
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
    public partial class PatientDischargeSurveyPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private bool IsSubmitVisibled = false;

        PatientSurveyPageViewModel PatientSurveyPageViewModel { get; set; }
        private List<SurgicalResourcePatientProstatectomyLibraryPageViewModel> _dischargeResourceItems { set; get; }
        private PatientSurveyClient PatientSurveyClient;

        public PatientDischargeSurveyPage(PatientSurveyPageViewModel patientSurveyPageViewModel, List<SurgicalResourcePatientProstatectomyLibraryPageViewModel> dischargeResourceItems)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();

                _iTokenContainer = new TokenContainer();

                Title = _iTokenContainer.ApiPracticeName;
                //Subtitle = AppConstant.DischargeNotificationInfo;

                if (!_iTokenContainer.IsApiToken())
                {
                    //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                    App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
                }
                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
                PatientSurveyClient = new PatientSurveyClient(apiClient);

                PatientSurveyPageViewModel = patientSurveyPageViewModel;
                _dischargeResourceItems = dischargeResourceItems;
                NotificationTitle.Text = patientSurveyPageViewModel.NotificationTitle;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientDischargeSurveyPage()
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
                NotificationTitle.Text = "Check-In Program";
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

            string nextItem = string.Empty;
            if (t.Length == 2)
            {
                nextItem = t[1];
            }

            // Set Group Selected Value
            RadioButtonGroupView ch = radioButton.Parent as RadioButtonGroupView;
            ch.ClassId = value.ToString();
            //Hide All Element/Block
            HideAllNextBock();
            ShowNextBocks();

        }

        private void RadioButtonClickedCommand(object sender, object Value, RadioButtonGroupView parent)
        {
            var value = Value;

            var t = value.ToString().Split(':').ToArray();
            var selectedValue = t[0];

            string nextItem = string.Empty;
            if (t.Length >= 2)
            {
                nextItem = t[1];
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
                if (lbl.ClassId == value + "__tim")
                {
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
            if (t.Length >= 4)
            {
                nextItem = t[1];
            }
            // Set Group Selected Value
            RadioButtonGroupView ch = parent;

            bool isCkecked = false;

            foreach (var item in ch.Children.Where(x => x.GetType() == typeof(CheckBox)))
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
            if (patientSurveyPatientNotificationDetailViewModel != null)
            {
                string blockTwoClassId = "FirstBlock";
                bool showBlock = true;
                int loopOne = 0;

                var patientSurveyQuestionSetViewModel = patientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel;
                if (patientSurveyQuestionSetViewModel.HasQuestions)
                {
                    PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel = patientSurveyQuestionSetViewModel;
                    HtmlLabel labelNotificationHeader = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = patientSurveyPatientNotificationDetailViewModel.NotificationHeader,
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    SurveyDesc.Children.Add(labelNotificationHeader);

                    HtmlLabel labelQuestionSetName = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = patientSurveyQuestionSetViewModel.SurveyQuestionSetNamePatient,
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    SurveyDesc.Children.Add(labelQuestionSetName);

                    HtmlLabel labelDescription = new HtmlLabel()
                    {
                        FontSize = 19,
                        Text = patientSurveyQuestionSetViewModel.SurveyQuestionSetHeader,
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    SurveyDesc.Children.Add(labelDescription);

                    int totalQuestion = patientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels.Count();


                    foreach (var item in patientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels.OrderBy(x => x.QqnDisplayOrder))
                    {
                        RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                        foreach (var item2 in item.PatientSurveyQuestionDetailViewModels.OrderBy(x => x.DisplayOrder))
                        {

                            string classId = item2.SurveyQuestionDetailId.ToString() + ":Class_" + item2.NextQuestionId.ToString() + ":" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString();
                            if (item2.NextQuestionId == null)
                            {
                                classId = item2.SurveyQuestionDetailId.ToString() + ":Class_Submit:" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString(); ;
                            }

                            if (item.SurveyQuestionTypeId == 1 || item.SurveyQuestionTypeId == 2)
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
                                    TextColor = Color.FromHex("#222")
                                };
                                radioButtonGroupView.Children.Add(radio);
                                if (!string.IsNullOrEmpty(item2.OptionSuggestion))
                                {
                                    HtmlLabel lbl_two = new HtmlLabel
                                    {
                                        Text = item2.OptionSuggestion,
                                        FontFamily = "Fonts/georgia.ttf#georgia",
                                        FontSize = 13,
                                        Margin = new Thickness(30, 0, 0, 10),
                                        IsVisible = false,
                                        ClassId = classId + "__tim",
                                        TextColor = Color.FromHex("#222")
                                    };
                                    radioButtonGroupView.Children.Add(lbl_two);
                                }
                                BoxView bxRadio = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                radioButtonGroupView.Children.Add(bxRadio);
                            }
                            else if (item.SurveyQuestionTypeId == 4)
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
                                    CheckChangedCommand = new Command((tag) =>
                                    {
                                        CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                                    }),
                                    FontFamily = "Fonts/georgia.ttf#georgia",
                                    TextColor = Color.FromHex("#222")
                                };
                                radioButtonGroupView.Children.Add(check);
                                BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                radioButtonGroupView.Children.Add(bxCheck);
                            }
                            else if (item.SurveyQuestionTypeId == 3)
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
                        }

                        StackLayout qsn = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = 10,
                            BackgroundColor = Color.FromHex("#d6f7fe")
                        };
                        Label lblQsn = new Label
                        {
                            Text = item.SurveyQuestionText,
                            TextColor = Color.FromHex("#446377"),
                            FontSize = 18,
                            FontFamily = "Fonts/georgia.ttf#georgia"
                        };
                        qsn.Children.Add(lblQsn);


                        if (blockTwoClassId == "")
                        {
                            blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString();
                            showBlock = false;
                        }
                        StackLayout BlockTwo = new StackLayout { ClassId = blockTwoClassId, BackgroundColor = Color.FromHex("#f0fcfe"), IsVisible = showBlock };
                        if (item.SurveyQuestionTypeId == 4)
                        {
                            //BlockTwo.Margin = new Thickness(-15, 0, 0, 0);
                        }
                        BlockTwo.Children.Add(qsn);


                        StackLayout StackLayoutSlRadio = new StackLayout { ClassId = "slRadio", Padding = 2 };
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

                    Button buttonSubmit = new Button { Text = buttonText, BackgroundColor = Color.FromHex("#337ab7"), TextColor = Color.FromHex("#ffffff"), HeightRequest = 50, FontFamily = "Fonts/georgia.ttf#georgia", FontSize = 22 };
                    buttonSubmit.Clicked += async delegate { await SubmitButtonClickedAsync(); };

                    buttonGroupView.Children.Add(buttonSubmit);
                    buttonMainSlRadio.Children.Add(buttonGroupView);
                    buttonMain.Children.Add(buttonMainSlRadio);
                    StackLayoutMainBlock.Children.Add(buttonMain);

                }
                else
                {
                    HtmlLabel labelNotificationHeader = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = patientSurveyPatientNotificationDetailViewModel.NotificationHeader,
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    SurveyDesc.Children.Add(labelNotificationHeader);
                }

                StackLayoutDischargeResourceBlock.Children.Clear();
                StackLayoutDischargeResourceBlock.Children.Add(LoadDischargeResource());

            }
        }

        private StackLayout LoadDischargeResource()
        {
            LoadDischargeDemoData();
            StackLayout dischargeResourceContainer = new StackLayout() { Padding = 10, Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };
            if (_dischargeResourceItems != null && _dischargeResourceItems.Any())
            {
                foreach (var item in _dischargeResourceItems)
                {
                    Button button = new Button
                    {
                        Text = item.SurgicalResourcePatientProstatectomyLibraryShortName,
                        HeightRequest = 110,
                        BackgroundColor = Color.FromHex("#c4d8f9"),
                        Image = item.SurgicalResourcePatientProstatectomyLibraryIcon,
                        ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Top, 10),
                        FontSize = 15,
                        TextColor = Color.Black,
                        CornerRadius = 4,
                        ClassId = item.SurgicalResourcePatientProstatectomyLibraryId.ToString(),
                        BorderWidth = 2,
                        BorderColor = Color.FromHex("#000000")
                    };
                    button.Clicked += async (sender, args) => await Navigation.PushAsync(new PatientDischargeResourceViewPage(item));
                    dischargeResourceContainer.Children.Add(button);
                }
            }
            return dischargeResourceContainer;
        }

        private void LoadDischargeDemoData()
        {
            if (_dischargeResourceItems == null)
            {
                _dischargeResourceItems = new List<SurgicalResourcePatientProstatectomyLibraryPageViewModel>
                {
                    new SurgicalResourcePatientProstatectomyLibraryPageViewModel
                    {
                        SurgicalResourcePatientProstatectomyLibraryShortName="Activity",
                        SurgicalResourcePatientProstatectomyLibraryResourceContent="<h3>This is test activity html</h3>"
                    },
                    new SurgicalResourcePatientProstatectomyLibraryPageViewModel
                    {
                        SurgicalResourcePatientProstatectomyLibraryShortName="Diet",
                        SurgicalResourcePatientProstatectomyLibraryResourceContent="<h3>This is test diet html</h3>"
                    }
                };
            }
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

        public async Task SubmitButtonClickedAsync()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    await SaveForm();
                    await ReLoadSurveyWithNextWuestionSet();
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        public async Task SaveForm()
        {
            var viewModel = PatientSurveyPageViewModel.PatientSurveyProcedureNotificationViewModel;
            viewModel.PracticeProfileId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PracticeProfileId;
            viewModel.ProfessionalProfileId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.ProfessionalProfileId;
            viewModel.ProcedureId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.ProcedureId;
            viewModel.PatientNotificationDetailId = (long)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientNotificationDetailId;
            viewModel.NotificationId = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.NotificationId;
            viewModel.NotificationOrder = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.NotificationOrder;
            viewModel.PatientProcedureDetailId = (Guid)PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientProcedureDetailId;
            viewModel.SurveyQuestionSetId = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel.SurveyQuestionSetId;

            var dd = PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel.PatientSurveyQuestionViewModels;

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
                                    var details = dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().PatientSurveyQuestionDetailViewModels;

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
                                            }
                                        }
                                    }

                                }
                                else if (t[4].ToString() == "R")
                                {
                                    string selectedAnsware = t[0].ToString();
                                    long surveyQuestionId = Convert.ToInt64(t[3]);
                                    int setId = Convert.ToInt32(t[2]);
                                    dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                }
                                else if (t[4].ToString() == "T")
                                {
                                    Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
                                    if (editor != null)
                                    {
                                        string selectedAnsware = editor.Text;
                                        long surveyQuestionId = Convert.ToInt64(t[3]);
                                        int setId = Convert.ToInt32(t[2]);
                                        dd.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnsware;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            viewModel.PatientSurveyQuestionSetViewModels.Add(PatientSurveyPageViewModel.PatientSurveyQuestionSetViewModel);
            try
            {
                var data = await PatientSurveyClient.PostPatientSurvey(viewModel);
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        public async Task ReLoadSurveyWithNextWuestionSet()
        {
            var viewModel = PatientSurveyPageViewModel.PatientSurveyProcedureNotificationViewModel;
            var response = await PatientSurveyClient.GetNextPatientSurvey(viewModel.PracticeProfileId.ToString(), viewModel.ProfessionalProfileId.ToString(), viewModel.PatientProcedureDetailId.ToString(), viewModel.NotificationId.ToString());
            if (response.StatusIsSuccessful)
            {
                PatientSurveyQuestionSetViewModel patientSurveyQuestionSetViewModel = response.Data;
                viewModel.PatientSurveyQuestionSetViewModels.Clear();
                viewModel.PatientSurveyQuestionSetViewModels.Add(patientSurveyQuestionSetViewModel);
                PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel = patientSurveyQuestionSetViewModel;
                CreateForm();
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
            using (UserDialogs.Instance.Loading(""))
            {
                CreateForm();
            }
        }

        #region Top Menu Actions

        private async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    //App.Instance.ClearNavigationAndGoToPage(new MainPage());
                    await Navigation.PushAsync(new MainPage());
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnScheduleButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    NotificationListPageViewModel model = new NotificationListPageViewModel();
                    await model.ExecuteLoadCommandAsync();
                    await Navigation.PushAsync(new NotificationListPage(model));
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnPhysicianButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                await Navigation.PushAsync(new PhysicianProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnLocationButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                LocationPageNew page = new LocationPageNew();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnResourceButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                using (UserDialogs.Instance.Loading(""))
                {
                    ResourcePage page = new ResourcePage();
                    await page.LoadDataAsync();
                    await Navigation.PushAsync(page);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnOtherInfoButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new HospitalInfoPage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private async void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new ChangePasswordPage());
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
        private async void OnUpdatePatientProfileClicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                await Navigation.PushAsync(new UpdateProfilePage());
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }


        #endregion
    }
}