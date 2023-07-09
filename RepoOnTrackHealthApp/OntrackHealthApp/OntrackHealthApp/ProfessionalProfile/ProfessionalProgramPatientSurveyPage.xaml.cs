using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.UserControls;
using OntrackHealthApp.ViewModel;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfessionalProgramPatientSurveyPage : ContentPage
	{
		
        private readonly ITokenContainer _iTokenContainer;
        private bool IsSubmitVisibled = false;
        //private string NextItem = "";
        PatientSurveyPageViewModel PatientSurveyPageViewModel { get; set; }

        private PatientSurveyClient PatientSurveyClient;

        
        private ProgramDetailViewModel programDetailViewModel;

        public ProfessionalProgramPatientSurveyPage(PatientNotificationShortView patientNotificationShortView,ProgramDetailViewModel programDetailViewModel)
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
                this.programDetailViewModel = programDetailViewModel;

                PatientSurveyPageViewModel = new PatientSurveyPageViewModel();
                PatientSurveyPageViewModel.NotificationId = patientNotificationShortView.NotificationId;
                //BindingContext = PatientSurveyPageViewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }
        

        

        private void CreateForm()
        {
            StackLayoutMainBlock.Children.Clear();
            SurveyDesc.Children.Clear();

            string blockTwoClassId = "FirstBlock";
            bool showBlock = true;
            int loopOne = 0;
                
            var patientSurveyQuestionSetViewModel = programDetailViewModel.SurveyQuestionSets;
            foreach (var SurveyQuestionSet in patientSurveyQuestionSetViewModel)
            {
                var patientSurveyQuestionSetViewModels = programDetailViewModel.SurveyQuestionSets;

                HtmlLabel labelQuestionSetName = new HtmlLabel()
                {
                    FontSize = 21,
                    Text = SurveyQuestionSet.SurveyQuestionSetName,
                    FontFamily = "Fonts/georgia.ttf#georgia",
                    TextColor = Color.FromHex("#fff"),
                    BackgroundColor = Color.FromHex("#548fe8")
                };
                StackLayoutMainBlock.Children.Add(labelQuestionSetName);

                var Questions = programDetailViewModel.SurveyQuestions.Where(m => m.SurveyQuestionSetId == SurveyQuestionSet.SurveyQuestionSetId).ToList();

                foreach (var item in Questions.OrderBy(x => x.DisplayOrder))
                {
                    var SurveyQuestionDetail = programDetailViewModel.SurveyQuestionDetails.Where(m => m.SurveyQuestionId == item.SurveyQuestionId).ToList();
                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();
                    foreach (var item2 in SurveyQuestionDetail.OrderBy(x => x.DisplayOrder))
                    {

                               
                        if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt()
                            || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt())
                        {
                            RadioButton radio = new RadioButton
                            {
                                Text = item2.QuestionDetailText,
                                CircleSize = 32,
                                TextFontSize = 20,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                IsDisabled = true,
                                Spacing = 5,
                                FontFamily = "Fonts/georgia.ttf#georgia",
                                TextColor = Color.FromHex("#222"),
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
                                    IsVisible = true,
                                    TextColor = Color.FromHex("#222")
                                };
                                radioButtonGroupView.Children.Add(lbl_two);
                            }
                            BoxView bxRadio = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                            radioButtonGroupView.Children.Add(bxRadio);
                        }
                        else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                        {
                                   
                            CheckBox check = new CheckBox
                            {
                                Text = item2.QuestionDetailText,
                                BoxSizeRequest = 26,
                                TextFontSize = 20,
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                IsDisabled = true,
                                Spacing = 5,
                                IsChecked = false,
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
                                    
                            Editor editor = new Editor()
                            {
                                Text = "",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 100
                            };
                            editor.TextChanged += Editor_TextChanged; ;
                            radioButtonGroupView.Children.Add(editor);
                        }
                        //else if (item.SurveyQuestionTypeId == 5)
                        else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                        {
                                    
                            DatePicker datePicker = new DatePicker()
                            {
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                HeightRequest = 50
                            };
                            datePicker.DateSelected += DatePicker_DateSelected;
                            radioButtonGroupView.Children.Add(datePicker);
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
                    StackLayout BlockTwo = new StackLayout { ClassId = blockTwoClassId, BackgroundColor = Color.FromHex("#f0fcfe"), IsVisible = true };
                    if (item.SurveyQuestionTypeId == 4)
                    {
                        //BlockTwo.Margin = new Thickness(-15, 0, 0, 0);
                    }
                    BlockTwo.Children.Add(qsn);


                    StackLayout StackLayoutSlRadio = new StackLayout { ClassId = "slRadio", Padding = new Thickness(10, 2) };
                    StackLayoutSlRadio.Children.Add(radioButtonGroupView);

                    BlockTwo.Children.Add(StackLayoutSlRadio);
                    blockTwoClassId = "";

                    StackLayoutMainBlock.Children.Add(BlockTwo);

                    loopOne += 1;
            }
                string buttonText = string.Empty;

                    //StackLayout buttonMain = new StackLayout { ClassId = "Class_Submit", IsVisible = false, Margin = new Thickness(0, 15) };
                    //StackLayout buttonMainSlRadio = new StackLayout { ClassId = "slRadio", Padding = 2 };
                    //RadioButtonGroupView buttonGroupView = new RadioButtonGroupView();

                    //ButtonEditExtended buttonSubmit = new ButtonEditExtended { Text = buttonText, FontSize = 22 };
                    //buttonSubmit.Clicked += async delegate { await SubmitButtonClickedAsync(); };

                    //buttonGroupView.Children.Add(buttonSubmit);
                    //buttonMainSlRadio.Children.Add(buttonGroupView);
                    //buttonMain.Children.Add(buttonMainSlRadio);
                    //StackLayoutMainBlock.Children.Add(buttonMain);
                
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
            //HideAllNextBock();
            //ShowNextBocks();
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
            ////Hide All Element/Block
            //HideAllNextBock();
            //ShowNextBocks();
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