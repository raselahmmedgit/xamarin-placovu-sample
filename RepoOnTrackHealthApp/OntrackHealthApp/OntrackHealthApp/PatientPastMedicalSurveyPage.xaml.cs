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
    public partial class PatientPastMedicalSurveyPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private bool IsSubmitVisibled = false;
        public PatientSurveyPageViewModel PatientSurveyPageViewModel { get; set; }
        PatientNotificationShortView PatientNotificationShortView { get; set; }
        private PatientSurveyClient PatientSurveyClient;
        public PatientPastMedicalSurveyPage(PatientNotificationShortView patientNotificationShortView)
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
        public PatientPastMedicalSurveyPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;
                ProcedureName.Text = _iTokenContainer.CurrentProcedureName;
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
                        string id = "";
                        foreach (var item2 in item.PatientSurveyQuestionDetailViewModels.OrderBy(x => x.DisplayOrder))
                        {

                            string classId = item2.SurveyQuestionDetailId.ToString() + ":Class_" + item2.NextQuestionId.ToString() + ":" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString();
                            if (item2.NextQuestionId == null)
                            {
                                classId = item2.SurveyQuestionDetailId.ToString() + ":Class_Submit:" + item2.SurveyQuestionSetId.ToString() + ":" + item.SurveyQuestionId.ToString(); ;
                            }

                            if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyAllergies.ToInt())
                            {
                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //< input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type = "hidden" value = "@item2.DoNotShowNextSetId" />
                                }

                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt() || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt())
                                {
                                    //id = Guid.NewGuid().ToString();
                                    //var pastMedicalSurveyAllergiesSubmitButton = ("showSubmitButton_" + Model.NotificationId);

                                    if (item2.DefaultValue == "True")
                                    {
                                        classId = classId + ":R";
                                        RadioButton radio = new RadioButton
                                        {
                                            Text = item2.QuestionDetailText,
                                            CircleSize = 32,
                                            TextFontSize = 18,
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
                                            Color = Color.FromHex("#222"),
                                            Padding = new Thickness(10, 0)
                                        };
                                        radioButtonGroupView.Children.Add(radio);
                                        //< div style = "padding:5px" >

                                        //     < input class="magic-radio" data-nextwrapper="@nextwrapper" data-submitbutton="@pastMedicalSurveyAllergiesSubmitButton"
                                        //           onclick="patientPastMedicalSurvey.ShowHideQuestionAllergiesYes(this, 'replaceQuestionSet_@Model.NotificationId')" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SelectedAnsware" type="radio" value="@item2.SurveyQuestionDetailId" data-oldvalue="@item2.SurveyQuestionDetailId" data-tvalue="@item2.SubmitButtonValue" />
                                        //    <label for="@id">
                                        //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                        //    </label>
                                        //</div>
                                    }
                                    else if (item2.DefaultValue == "False")
                                    {
                                        classId = classId + ":R";
                                        RadioButton radio = new RadioButton
                                        {
                                            Text = item2.QuestionDetailText,
                                            CircleSize = 32,
                                            TextFontSize = 18,
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
                                            Color = Color.FromHex("#222"),
                                            Padding = new Thickness(10, 0)
                                        };
                                        radioButtonGroupView.Children.Add(radio);
                                        //pastMedicalSurveyAllergiesQuestionDetailNo = ("showPastMedicalSurveyAllergiesQuestion_" + item.SurveyQuestionSetId.ToString() + "_" + item.SurveyQuestionId.ToString());
                                        //<div style = "padding:5px" >
                                        //    < input class="magic-radio" data-nextwrapper="@pastMedicalSurveyAllergiesQuestionDetailNo" data-submitbutton="@pastMedicalSurveyAllergiesSubmitButton"
                                        //           onclick="patientPastMedicalSurvey.ShowHideQuestionAllergiesNo(this, 'replaceQuestionSet_@Model.NotificationId')" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SelectedAnsware" type="radio" value="@item2.SurveyQuestionDetailId" data-oldvalue="@item2.SurveyQuestionDetailId" data-tvalue="@item2.SubmitButtonValue" />
                                        //    <label for="@id">
                                        //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                        //    </label>
                                        //</div>
                                    }

                                }
                                else
                                {
                                    if (item2.DefaultValueType == Enums.SurveyQuestionDetailDefaultValueTypeForEnum.String.ToDescriptionAttr())
                                    {
                                        id = Guid.NewGuid().ToString();
                                        classId = classId + ":T";
                                        Editor editor = new Editor()
                                        {
                                            ClassId = classId,
                                            Text = "",
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            HorizontalOptions = LayoutOptions.FillAndExpand,
                                            Placeholder = item2.QuestionDetailText
                                        };
                                        editor.TextChanged += Editor_TextChanged; ;
                                        radioButtonGroupView.ClassId = classId;
                                        radioButtonGroupView.Children.Add(editor);
                                        //<div class="row" style="padding:5px">
                                        //    <div class="form-group">
                                        //        <label class="control-label col-md-4" style="margin-top:5px;font-size: 16px;">@item2.QuestionDetailText</label>
                                        //        <div class="col-md-6">

                                        //            <input type = "text" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].SelectedAnsware"
                                        //                   class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />

                                        //        </div>
                                        //        <div class="col-md-2"></div>
                                        //    </div>
                                        //</div>
                                    }
                                    else if (item2.DefaultValueType == Enums.SurveyQuestionDetailDefaultValueTypeForEnum.Button.ToDescriptionAttr())
                                    {
                                        //<div class="row">
                                        //    <div class="form-group">
                                        //        <div class="col-md-10"></div>
                                        //        <div class="col-md-2 text-right surveyquestiondetail-btn-add">
                                        //            <input type = "button" class="btn btn-sm btn-secondary" data-nextwrapper="@nextwrapper" onclick="patientPastMedicalSurvey.ShowHideQuestionAllergiesAndMedications(this, 'replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" value="@item2.QuestionDetailText" />
                                        //        </div>
                                        //    </div>
                                        //</div>
                                        ButtonExtended buttonAdd = new ButtonExtended { Text = "Add", Margin = new Thickness(60, 0, 60, 0) };

                                        buttonAdd.Clicked += async delegate { await AddButtonClickedAsync(item2.NextQuestionId); };
                                        radioButtonGroupView.Children.Add(buttonAdd);
                                    }
                                }

                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications.ToInt())
                            {
                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //<input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type="hidden" value="@item2.DoNotShowNextSetId" />
                                }

                                if (item2.DefaultValueType == Enums.SurveyQuestionDetailDefaultValueTypeForEnum.String.ToDescriptionAttr())
                                {
                                    id = Guid.NewGuid().ToString();
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Placeholder = item2.QuestionDetailText

                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                    //<div class="row" style="padding:5px">
                                    //    <div class="form-group">
                                    //        <label class="control-label col-md-4" style="margin-top:5px;font-size: 16px;">@item2.QuestionDetailText</label>
                                    //        <div class="col-md-6">

                                    //            <input type = "text" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].SelectedAnsware"
                                    //                   class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />

                                    //        </div>
                                    //        <div class="col-md-2"></div>
                                    //    </div>
                                    //</div>
                                }
                                else if (item2.DefaultValueType == Enums.SurveyQuestionDetailDefaultValueTypeForEnum.Button.ToDescriptionAttr())
                                {
                                    ButtonExtended buttonAdd = new ButtonExtended { Text = "Add", Margin = new Thickness(60, 0, 60, 0) };

                                    buttonAdd.Clicked += async delegate { await AddButtonClickedAsync(item2.NextQuestionId); };
                                    radioButtonGroupView.Children.Add(buttonAdd);
                                    //<div class="row">
                                    //    <div class="form-group">
                                    //        <div class="col-md-10"></div>
                                    //        <div class="col-md-2 text-right surveyquestiondetail-btn-add">
                                    //            <input type = "button" class="btn btn-sm btn-secondary" data-nextwrapper="@nextwrapper" onclick="patientPastMedicalSurvey.ShowHideQuestionAllergiesAndMedications(this, 'replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" value="@item2.QuestionDetailText" />
                                    //        </div>
                                    //    </div>
                                    //</div>
                                }

                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt())
                            {
                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //<input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type="hidden" value="@item2.DoNotShowNextSetId" />
                                }

                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    var years = Enumerable.Range(DateTime.Now.Year - 80, 81)
                                        .OrderByDescending(x => x);

                                    classId = classId + ":YD";
                                    Picker yearPicker = new Picker
                                    {
                                        Title = "Select a Year",
                                        ClassId = classId,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        Margin = new Thickness(60, 0, 0, 0)
                                    };

                                    foreach (var year in years)
                                    {
                                        yearPicker.Items.Add(year.ToString());
                                    }

                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(yearPicker);


                                    //classId = classId + ":D";
                                    //DatePicker datePicker = new DatePicker()
                                    //{
                                    //    ClassId = classId,
                                    //    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    //    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    //    HeightRequest = 45
                                    //};
                                    //datePicker.DateSelected += DatePicker_DateSelected;
                                    //radioButtonGroupView.ClassId = classId;
                                    //radioButtonGroupView.Children.Add(datePicker);
                                    //<div class="row hidden @nextwrapperPastSurgicalAndMedicalHistoryQuestionDetail" style="padding:5px">
                                    //    <div class="form-group">
                                    //        <div class="col-md-2"></div>
                                    //        <label class="control-label col-md-4 text-right" style="margin-top:5px;font-size: 16px;">@item2.QuestionDetailText</label>
                                    //        <div class="col-md-6">
                                    //            <div class='input-group date dp_SurveyQuestionDetailYear' id='dp_SurveyQuestionDetailYear_@item2.SurveyQuestionDetailId'>
                                    //                <input type = "text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" maxlength="4" />
                                    //                <span class="input-group-addon">
                                    //                    <span class="glyphicon glyphicon-calendar"></span>
                                    //                </span>
                                    //            </div>
                                    //        </div>
                                    //    </div>
                                    //</div>
                                }
                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt())
                            {
                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //<input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type="hidden" value="@item2.DoNotShowNextSetId" />
                                }

                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
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
                                        TextColor = Color.FromHex("#222"),
                                        Color = Color.FromHex("#222"),
                                        //Type = CheckBox.CheckType.Check,
                                        Padding = new Thickness(10, 0),
                                        Margin = new Thickness(60, 0, 0, 0)
                                    };
                                    radioButtonGroupView.Children.Add(check);
                                    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                    radioButtonGroupView.Children.Add(bxCheck);
                                    //id = Guid.NewGuid().ToString();
                                    //<div class="row hidden @nextwrapperPastSurgicalAndMedicalHistoryQuestionDetail" style="padding:5px">
                                    //    <div class="col-md-2"></div>
                                    //    <div class="col-md-10">
                                    //        <div style = "padding:5px" >
                                    //            < input class="magic-checkbox" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected"
                                    //                   data-nextwrapper="@nextwrapper" onclick="patientPastMedicalSurvey.ShowHideQuestionDetailPastSurgicalAndMedicalHistory(this, 'replaceQuestionSet_@Model.NotificationId')" type="checkbox" value="true">
                                    //            <label for="@id">
                                    //                @Html.Raw(item2.QuestionDetailText)
                                    //            </label>
                                    //            <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected" type="hidden" value="false">
                                    //        </div>
                                    //    </div>
                                    //</div>

                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Placeholder = "Type of relation",
                                        Margin = new Thickness(40, 0, 0, 0)
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                    //<div class="row" style="padding:5px">
                                    //    <div class="col-md-2"></div>
                                    //    <div class="col-md-7">
                                    //        <div style = "padding:5px" >
                                    //            < input type="text" placeholder="Type of relation" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //        </div>
                                    //    </div>
                                    //    <div class="col-md-3"></div>
                                    //</div>
                                }
                            }
                            else if ((item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyGeneralInformation.ToInt()) && (item.SurveyQuestionId < Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationPneumoniaVaccine.ToInt()))
                            {

                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //<input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type="hidden" value="@item2.DoNotShowNextSetId" />
                                }
                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt() || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    classId = classId + ":R";
                                    RadioButton radio = new RadioButton
                                    {
                                        Text = item2.QuestionDetailText,
                                        CircleSize = 32,
                                        TextFontSize = 18,
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
                                        Color = Color.FromHex("#222"),
                                        Padding = new Thickness(10, 0)
                                    };
                                    radioButtonGroupView.Children.Add(radio);
                                    //<div style = "padding:5px" >
                                    //    < input class="magic-radio" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SelectedAnsware" type="radio" value="@item2.SurveyQuestionDetailId" data-oldvalue="@item2.SurveyQuestionDetailId" data-tvalue="@item2.SubmitButtonValue" />
                                    //    <label for="@id">
                                    //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //    </label>
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
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
                                        TextColor = Color.FromHex("#222"),
                                        Color = Color.FromHex("#222"),
                                        //Type = CheckBox.CheckType.Check,
                                        Padding = new Thickness(10, 0)
                                    };
                                    radioButtonGroupView.Children.Add(check);
                                    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                    radioButtonGroupView.Children.Add(bxCheck);
                                    //<div style = "padding:5px" >
                                    //    < input class="magic-checkbox" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected"
                                    //           type="checkbox" value="true">
                                    //    <label for="@id">
                                    //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //    </label>
                                    //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected" type="hidden" value="false">
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                                {
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                    //<div style = "padding-bottom:10px" >
                                    //    < textarea style="resize: none;" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId"></textarea>
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    //<div class="row" style="padding:5px">
                                    //    <div class="form-group">
                                    //        <label class="control-label col-md-4" style="margin-top:5px;font-size: 16px;">@item.SurveyQuestionText</label>
                                    //        <div class="col-md-6">
                                    //            <div class='input-group date dp_SurveyQuestionDetailDate' id='dp_SurveyQuestionDetailDate_@item2.SurveyQuestionDetailId'>
                                    //                <input type = "text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //                <span class="input-group-addon">
                                    //                    <span class="glyphicon glyphicon-calendar"></span>
                                    //                </span>
                                    //            </div>
                                    //        </div>
                                    //        <div class="col-md-2"></div>
                                    //    </div>
                                    //</div>
                                    classId = classId + ":D";
                                    DatePicker datePicker = new DatePicker()
                                    {
                                        ClassId = classId,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        HeightRequest = 45
                                    };
                                    datePicker.DateSelected += DatePicker_DateSelected;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(datePicker);
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Email.ToInt())
                                {
                                    //var surveyQuestionDetailEmailId = ("SurveyQuestionDetailEmail_" + item2.SurveyQuestionDetailId);
                                    //<div style = "padding-bottom:10px" >
                                    //    < input type="text" data-validid="@surveyQuestionDetailEmailId" onblur="patientPastMedicalSurvey.ShowHideQuestionGeneralInformationEmail(this, 'replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //    <div class="field-validation-valid text-danger survey-email-valid" style="display: none;" id="@surveyQuestionDetailEmailId">Email is not valid.</div>
                                    //</div>
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Phone.ToInt())
                                {
                                    var surveyQuestionDetailPhoneId = ("SurveyQuestionDetailPhone_" + item2.SurveyQuestionDetailId);
                                    var surveyQuestionDetailPhoneCodeId = ("SurveyQuestionDetailPhoneCode_" + item2.SurveyQuestionDetailId);
                                    var surveyQuestionDetailPhoneCountryIdId = ("SurveyQuestionDetailPhoneCountryId_" + item2.SurveyQuestionDetailId);
                                    //<div class="row" style="padding-bottom:10px">
                                    //    <div class="form-group">
                                    //        <div class="col-md-12">
                                    //            <div class="phone-control">
                                    //                <div class="btn-group country-control" data-numbercontrol="@surveyQuestionDetailPhoneId" data-phonecode="@surveyQuestionDetailPhoneCodeId" data-countryid="@surveyQuestionDetailPhoneCountryIdId">
                                    //                    <button class="btn dropdown-toggle btn-sm" type="button" data-toggle="dropdown" aria-expanded="false">
                                    //                        <span class="selected-country-flag flag flag-16 flag-@ViewBag.PhoneCountryIso"></span>
                                    //                        @*<i class="fa fa-angle-down"></i>*@
                                    //                    </button>
                                    //                    @*@{Html.RenderPartial("~/Views/Partial/_CountryCodeDropdownList.cshtml", Model.CountryViewModels);}*@
                                    //                </div>
                                    //                <div class="number-code-control">
                                    //                    @ViewBag.PhoneCode
                                    //                </div>
                                    //                <input type = "hidden" id="@surveyQuestionDetailPhoneCodeId" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnswarePhoneCode", loop_one)" value="@ViewBag.PhoneCode" />
                                    //                <input type = "hidden" id="@surveyQuestionDetailPhoneCountryIdId" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnswarePhoneCountryId", loop_one)" value="@ViewBag.PhoneCountryId" />
                                    //                <input type = "text" id="@surveyQuestionDetailPhoneId" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea SurveyQuestionDetailPhone"
                                    //                       style="border: none;" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //            </div>
                                    //        </div>
                                    //    </div>
                                    //</div>
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Keyboard = Keyboard.Telephone,
                                        Placeholder = "+10234567890"
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.TextEncrypt.ToInt())
                                {
                                    if (item.SurveyQuestionId == Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationHeight.ToInt())
                                    {
                                        //<div class="row" style="padding:5px">
                                        //    <div class="form-group">
                                        //        <div class="col-md-2">
                                        //            <input type = "text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                        //        </div>
                                        //        <div class="col-md-2" style="margin-top:5px;font-size: 16px;">ft</div>
                                        //        <div class="col-md-2">
                                        //            <input type = "text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnswareTwo", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                        //        </div>
                                        //        <div class="ccol-md-2" style="margin-top:5px;font-size: 16px;">in</div>
                                        //        <div class="ccol-md-4"></div>
                                        //    </div>
                                        //</div>
                                        StackLayout generalInfoHeightStackLayout = new StackLayout();
                                        generalInfoHeightStackLayout.Orientation = StackOrientation.Horizontal;
                                        generalInfoHeightStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

                                        classId = classId + ":T";
                                        Editor editor = new Editor()
                                        {
                                            ClassId = "T-FT",
                                            Text = "",
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            HorizontalOptions = LayoutOptions.FillAndExpand,
                                            Placeholder = "ft"
                                        };
                                        editor.TextChanged += Editor_TextChanged;

                                        Editor secondEditor = new Editor()
                                        {
                                            ClassId = "T-IN",
                                            Text = "",
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            HorizontalOptions = LayoutOptions.FillAndExpand,
                                            Placeholder = "in"
                                        };
                                        secondEditor.TextChanged += Editor_TextChanged;

                                        generalInfoHeightStackLayout.Children.Add(editor);
                                        generalInfoHeightStackLayout.Children.Add(secondEditor);

                                        radioButtonGroupView.ClassId = classId;
                                        radioButtonGroupView.Children.Add(generalInfoHeightStackLayout);
                                    }
                                    else if (item.SurveyQuestionId == Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationWeight.ToInt())
                                    {
                                        //<div class="row" style="padding:5px">
                                        //    <div class="form-group">
                                        //        <div class="col-md-2">
                                        //            <input type = "text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                        //        </div>
                                        //        <div class="col-md-2" style="margin-top:5px;font-size: 16px;">lbs</div>
                                        //        <div class="col-md-8"></div>
                                        //    </div>
                                        //</div>
                                        classId = classId + ":T";
                                        Editor editor = new Editor()
                                        {
                                            ClassId = classId,
                                            Text = "",
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        };
                                        editor.TextChanged += Editor_TextChanged; ;
                                        radioButtonGroupView.ClassId = classId;
                                        radioButtonGroupView.Children.Add(editor);
                                    }
                                    else
                                    {
                                        //<div style = "padding-bottom:10px" >
                                        //    < input type="text" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                        //</div>
                                        classId = classId + ":T";
                                        Editor editor = new Editor()
                                        {
                                            ClassId = classId,
                                            Text = "",
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            HorizontalOptions = LayoutOptions.FillAndExpand,
                                        };
                                        editor.TextChanged += Editor_TextChanged; ;
                                        radioButtonGroupView.ClassId = classId;
                                        radioButtonGroupView.Children.Add(editor);
                                    }

                                }

                                if (item2.OptionSuggestion != null)
                                {
                                    //<div class="optionsuggestion" style="display:none">
                                    //    @Html.Raw(item2.OptionSuggestion.Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //</div>
                                }

                            }
                            else
                            {

                                if (item2.DoNotShowNextSetId.HasValue)
                                {
                                    //<input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].DoNotShowNextSetId" type="hidden" value="@item2.DoNotShowNextSetId" />
                                }
                                if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt() || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    classId = classId + ":R";
                                    RadioButton radio = new RadioButton
                                    {
                                        Text = item2.QuestionDetailText,
                                        CircleSize = 32,
                                        TextFontSize = 18,
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
                                        Color = Color.FromHex("#222"),
                                        Padding = new Thickness(10, 0)
                                    };
                                    radioButtonGroupView.Children.Add(radio);
                                    //<div style = "padding:5px" >
                                    //    < input class="magic-radio" data-nextwrapper="@nextwrapper" onclick="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SelectedAnsware" type="radio" value="@item2.SurveyQuestionDetailId" data-oldvalue="@item2.SurveyQuestionDetailId" data-tvalue="@item2.SubmitButtonValue" />
                                    //    <label for="@id">
                                    //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //    </label>
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
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
                                        TextColor = Color.FromHex("#222"),
                                        Color = Color.FromHex("#222"),
                                        //Type = CheckBox.CheckType.Check,
                                        Padding = new Thickness(10, 0)
                                    };
                                    radioButtonGroupView.Children.Add(check);
                                    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                                    radioButtonGroupView.Children.Add(bxCheck);
                                    //<div style = "padding:5px" >
                                    //    < input class="magic-checkbox" id="@id" name="PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected"
                                    //           data-nextwrapper="@nextwrapper" onclick="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" type="checkbox" value="true">
                                    //    <label for="@id">
                                    //        @Html.Raw(item2.QuestionDetailText.Replace("  ", "&nbsp;&nbsp;&nbsp;").Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //    </label>
                                    //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].PatientSurveyQuestionDetailViewModels[@loop_two].IsSelected" type="hidden" value="false">
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                                {
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                    //<div style = "padding-bottom:10px" >
                                    //    < textarea style="resize: none;" data-nextwrapper="@nextwrapper" onfocus="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId"></textarea>
                                    //</div>
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                                {
                                    id = Guid.NewGuid().ToString();
                                    var years = Enumerable.Range(DateTime.Now.Year - 80, 81)
                                        .OrderByDescending(x => x);

                                    if (item.SurveyQuestionId == Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationPneumoniaVaccineYear.ToInt()
                                        || item.SurveyQuestionId == Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationColorectalExaminationYear.ToInt())
                                    {
                                        //<div class="row" style="padding:5px">
                                        //    <div class="form-group">
                                        //        <label class="control-label col-md-4" style="margin-top:5px;font-size: 16px;">@item.SurveyQuestionText</label>
                                        //        <div class="col-md-6">
                                        //            <div class='input-group date dp_SurveyQuestionDetailYear' id='dp_SurveyQuestionDetailYear_@item2.SurveyQuestionDetailId'>
                                        //                <input type = "text" data-nextwrapper="@nextwrapper" onblur="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" maxlength="4" />
                                        //                <span class="input-group-addon">
                                        //                    <span class="glyphicon glyphicon-calendar"></span>
                                        //                </span>
                                        //            </div>
                                        //        </div>
                                        //        <div class="col-md-2"></div>
                                        //    </div>
                                        //</div>


                                        classId = classId + ":YD";
                                        Picker yearPicker = new Picker
                                        {
                                            Title = "Select a Year",
                                            ClassId = classId,
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            Margin = new Thickness(60, 0, 0, 0)
                                        };

                                        foreach (var year in years)
                                        {
                                            yearPicker.Items.Add(year.ToString());
                                        }

                                        radioButtonGroupView.ClassId = classId;
                                        radioButtonGroupView.Children.Add(yearPicker);
                                    }
                                    else
                                    {
                                        //<div class="row" style="padding:5px">
                                        //    <div class="form-group">
                                        //        <label class="control-label col-md-4" style="margin-top:5px;font-size: 16px;">@item.SurveyQuestionText</label>
                                        //        <div class="col-md-6">
                                        //            <div class='input-group date dp_SurveyQuestionDetailDate' id='dp_SurveyQuestionDetailDate_@item2.SurveyQuestionDetailId'>
                                        //                <input type = "text" data-nextwrapper="@nextwrapper" onblur="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                        //                <span class="input-group-addon">
                                        //                    <span class="glyphicon glyphicon-calendar"></span>
                                        //                </span>
                                        //            </div>
                                        //        </div>
                                        //        <div class="col-md-2"></div>
                                        //    </div>
                                        //</div>
                                        //classId = classId + ":D";
                                        //DatePicker datePicker = new DatePicker()
                                        //{
                                        //    ClassId = classId,
                                        //    VerticalOptions = LayoutOptions.CenterAndExpand,
                                        //    HorizontalOptions = LayoutOptions.FillAndExpand,
                                        //    HeightRequest = 45
                                        //};
                                        //datePicker.DateSelected += DatePicker_DateSelected;
                                        classId = classId + ":YD";
                                        Picker yearPicker = new Picker
                                        {
                                            Title = "Select a Year",
                                            ClassId = classId,
                                            VerticalOptions = LayoutOptions.CenterAndExpand,
                                            Margin = new Thickness(60, 0, 0, 0)
                                        };

                                        foreach (var year in years)
                                        {
                                            yearPicker.Items.Add(year.ToString());
                                        }
                                        radioButtonGroupView.ClassId = classId;
                                        //radioButtonGroupView.Children.Add(datePicker);
                                        radioButtonGroupView.Children.Add(yearPicker);
                                    }
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Email.ToInt())
                                {
                                    var surveyQuestionDetailEmailId = ("SurveyQuestionDetailEmail_" + item2.SurveyQuestionDetailId);
                                    //<div style = "padding-bottom:10px" >
                                    //    < input type="text" data-nextwrapper="@nextwrapper" data-validid="@surveyQuestionDetailEmailId" onblur="patientPastMedicalSurvey.ShowHideQuestionEmail(this, 'replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //    <div class="field-validation-valid text-danger survey-email-valid" style="display: none;" id="@surveyQuestionDetailEmailId">Email is not valid.</div>
                                    //</div>
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Phone.ToInt())
                                {
                                    var surveyQuestionDetailPhoneId = ("SurveyQuestionDetailPhone_" + item2.SurveyQuestionDetailId);
                                    var surveyQuestionDetailPhoneCodeId = ("SurveyQuestionDetailPhoneCode_" + item2.SurveyQuestionDetailId);
                                    var surveyQuestionDetailPhoneCountryIdId = ("SurveyQuestionDetailPhoneCountryId_" + item2.SurveyQuestionDetailId);
                                    // <div class="row" style="padding-bottom:10px">
                                    //    <div class="form-group">
                                    //        <div class="col-md-12">
                                    //            <div class="phone-control">
                                    //                <div class="btn-group country-control" data-numbercontrol="@surveyQuestionDetailPhoneId" data-phonecode="@surveyQuestionDetailPhoneCodeId" data-countryid="@surveyQuestionDetailPhoneCountryIdId">
                                    //                    <button class="btn dropdown-toggle btn-sm" type="button" data-toggle="dropdown" aria-expanded="false">
                                    //                        <span class="selected-country-flag flag flag-16 flag-@ViewBag.PhoneCountryIso"></span>
                                    //                        @*<i class="fa fa-angle-down"></i>*@
                                    //                    </button>
                                    //                    @*@{Html.RenderPartial("~/Views/Partial/_CountryCodeDropdownList.cshtml", Model.CountryViewModels);}*@
                                    //                </div>
                                    //                <div class="number-code-control">
                                    //                    @ViewBag.PhoneCode
                                    //                </div>
                                    //                <input type = "hidden" id="@surveyQuestionDetailPhoneCodeId" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnswarePhoneCode", loop_one)" value="@ViewBag.PhoneCode" />
                                    //                <input type = "hidden" id="@surveyQuestionDetailPhoneCountryIdId" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnswarePhoneCountryId", loop_one)" value="@ViewBag.PhoneCountryId" />
                                    //                <input type = "text" data-nextwrapper="@nextwrapper" id="@surveyQuestionDetailPhoneId" onblur="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')"
                                    //                       name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea SurveyQuestionDetailPhone"
                                    //                       style="border: none;" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //            </div>
                                    //        </div>
                                    //    </div>
                                    //</div>
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Keyboard = Keyboard.Telephone,
                                        Placeholder = "+10234567890"
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }
                                else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.TextEncrypt.ToInt())
                                {
                                    //<div style = "padding-bottom:10px" >
                                    //    < input type="text" data-nextwrapper="@nextwrapper" onblur="patientPastMedicalSurvey.ShowHideQuestion('replaceQuestionSet_@Model.NotificationId')" name="@string.Format("PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[{0}].SelectedAnsware", loop_one)" class="form-control survey-textarea" data-isLastqsn="@isLastQsn" data-buttonid="btn_@item.NotificationId" />
                                    //</div>
                                    classId = classId + ":T";
                                    Editor editor = new Editor()
                                    {
                                        ClassId = classId,
                                        Text = "",
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    };
                                    editor.TextChanged += Editor_TextChanged; ;
                                    radioButtonGroupView.ClassId = classId;
                                    radioButtonGroupView.Children.Add(editor);
                                }

                                if (item2.OptionSuggestion != null)
                                {
                                    //<div class="optionsuggestion" style="display:none">
                                    //    @Html.Raw(item2.OptionSuggestion.Replace("Please call our office.", "<span style='color:#ff0000;'>Please call our office.</span>"))
                                    //</div>
                                }

                            }

                            //</div>

                            //if (item.SurveyQuestionTypeId == 1 || item.SurveyQuestionTypeId == 2)
                            //if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Yes_No.ToInt()
                            //    || item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Option.ToInt())
                            //{
                            //    classId = classId + ":R";
                            //    RadioButton radio = new RadioButton
                            //    {
                            //        Text = item2.QuestionDetailText,
                            //        CircleSize = 32,
                            //        TextFontSize = 18,
                            //        VerticalOptions = LayoutOptions.CenterAndExpand,
                            //        HorizontalOptions = LayoutOptions.FillAndExpand,
                            //        Value = classId,
                            //        Spacing = 5,
                            //        ClickCommand = new Command((tag) =>
                            //        {
                            //            RadioButtonClickedCommand(tag, classId, radioButtonGroupView);
                            //        }),
                            //        FontFamily = "Fonts/georgia.ttf#georgia",
                            //        TextColor = Color.FromHex("#222"),
                            //        Color = Color.FromHex("#222"),
                            //        Padding = new Thickness(10, 0)
                            //    };
                            //    radioButtonGroupView.Children.Add(radio);
                            //    if (!string.IsNullOrEmpty(item2.OptionSuggestion))
                            //    {
                            //        HtmlLabel lbl_two = new HtmlLabel
                            //        {
                            //            Text = item2.OptionSuggestion,
                            //            FontFamily = "Fonts/georgia.ttf#georgia",
                            //            FontSize = 13,
                            //            Margin = new Thickness(30, 0, 0, 10),
                            //            IsVisible = false,
                            //            ClassId = classId + "__tim",
                            //            TextColor = Color.FromHex("#222")
                            //        };
                            //        radioButtonGroupView.Children.Add(lbl_two);
                            //    }
                            //    BoxView bxRadio = new BoxView { BackgroundColor = Color.FromHex("#FFFFFF"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                            //    radioButtonGroupView.Children.Add(bxRadio);
                            //}
                            ////else if (item.SurveyQuestionTypeId == 4)
                            //else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                            //{
                            //    //Checkbox
                            //    classId = classId + ":C";
                            //    CheckBox check = new CheckBox
                            //    {
                            //        Text = item2.QuestionDetailText,
                            //        BoxSizeRequest = 26,
                            //        TextFontSize = 20,
                            //        VerticalOptions = LayoutOptions.CenterAndExpand,
                            //        HorizontalOptions = LayoutOptions.FillAndExpand,
                            //        ClassId = classId,
                            //        Spacing = 5,
                            //        IsChecked = false,
                            //        CheckChangedCommand = new Command((tag) => {
                            //            CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                            //        }),
                            //        FontFamily = "Fonts/georgia.ttf#georgia",
                            //        TextColor = Color.FromHex("#222"),
                            //        Color = Color.FromHex("#222"),
                            //        //Type = CheckBox.CheckType.Check,
                            //        Padding = new Thickness(10, 0)
                            //    };
                            //    radioButtonGroupView.Children.Add(check);
                            //    BoxView bxCheck = new BoxView { BackgroundColor = Color.FromHex("#d6f7fe"), HeightRequest = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
                            //    radioButtonGroupView.Children.Add(bxCheck);
                            //}
                            ////else if (item.SurveyQuestionTypeId == 3)
                            //else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                            //{
                            //    //Textbox
                            //    classId = classId + ":T";
                            //    Editor editor = new Editor()
                            //    {
                            //        ClassId = classId,
                            //        Text = "",
                            //        VerticalOptions = LayoutOptions.CenterAndExpand,
                            //        HorizontalOptions = LayoutOptions.FillAndExpand,
                            //        HeightRequest = 100
                            //    };
                            //    editor.TextChanged += Editor_TextChanged; ;
                            //    //radioButtonGroupView.ClassId = classId;
                            //    radioButtonGroupView.Children.Add(editor);
                            //}
                            ////else if (item.SurveyQuestionTypeId == 5)
                            //else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                            //{
                            //    //Date
                            //    classId = classId + ":D";
                            //    DatePicker datePicker = new DatePicker()
                            //    {
                            //        ClassId = classId,
                            //        VerticalOptions = LayoutOptions.CenterAndExpand,
                            //        HorizontalOptions = LayoutOptions.FillAndExpand,
                            //        HeightRequest = 45
                            //    };
                            //    datePicker.DateSelected += DatePicker_DateSelected;
                            //    //radioButtonGroupView.ClassId = classId;
                            //    radioButtonGroupView.Children.Add(datePicker);
                            //}
                        }

                        if (blockTwoClassId == "")
                        {
                            if ((item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyGeneralInformation.ToInt()) && (item.SurveyQuestionId <= Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationPneumoniaVaccine.ToInt()))
                            {
                                blockTwoClassId = "FirstBlock";
                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications.ToInt())
                            {
                                blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt())
                            {
                                blockTwoClassId = "FirstBlock";
                            }
                            else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt())
                            {
                                blockTwoClassId = "FirstBlock";
                            }
                            else
                            {
                                blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            }
                        }


                        StackLayout qsn = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Padding = 10,
                            BackgroundColor = Color.FromHex("#d6f7fe")
                        };

                        StackLayout StackLayoutSlRadio = new StackLayout { ClassId = "slRadio", Padding = 2 };


                        if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Date.ToInt())
                        {
                            if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt())
                            {
                                string classId = "survey_question_" + item.SurveyQuestionId;
                                CheckBox check = new CheckBox
                                {
                                    Text = item.SurveyQuestionText,
                                    BoxSizeRequest = 26,
                                    TextFontSize = 20,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    ClassId = classId,
                                    Spacing = 5,
                                    IsChecked = false,

                                    FontFamily = "Fonts/georgia.ttf#georgia",
                                    TextColor = Color.FromHex("#222"),
                                    Color = Color.FromHex("#222"),
                                    //Type = CheckBox.CheckType.Check,
                                    Padding = new Thickness(10, 0)
                                };
                                check.CheckChangedCommand = new Command((tag) =>
                                {
                                    pmmfCheckBoxCheckChangedCommand(StackLayoutSlRadio, check);
                                });
                                qsn.Children.Add(check);

                            }

                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SurveyQuestionId" type="hidden" value="@item.SurveyQuestionId" />
                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SurveyQuestionTypeId" type="hidden" value="@item.SurveyQuestionTypeId" />
                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].DontAskAgain" type="hidden" value="@item.DontAskAgain" id="@idDontAskAgain" />
                        }
                        else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt())
                        {

                            if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Multiple_Checkbox.ToInt())
                            {
                                //<div style = "padding:0px;" >
                                //    < div style="padding:5px">
                                //        <input class="magic-checkbox" data-showsubmitbutton="@showSubmitButtonPastSurgicalAndMedicalHistory" data-nextwrapper="@nextwrapperPastSurgicalAndMedicalHistoryQuestionDetail"
                                //               data-nextwrapperother="@nextwrapperPastSurgicalAndMedicalHistoryDependQuestionClass" id="@item.SurveyQuestionId" onclick="patientPastMedicalSurvey.ShowHideQuestionPastSurgicalAndMedicalHistory(this, 'replaceQuestionSet_@Model.NotificationId')" type="checkbox" value="true" />
                                //        <label for="@item.SurveyQuestionId">
                                //            @Html.Raw(item.SurveyQuestionText)
                                //        </label>
                                //    </div>
                                //</div>
                                string classId = "survey_question_" + item.SurveyQuestionId;
                                CheckBox check = new CheckBox
                                {
                                    Text = item.SurveyQuestionText,
                                    BoxSizeRequest = 26,
                                    TextFontSize = 20,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    ClassId = classId,
                                    Spacing = 5,
                                    IsChecked = false,

                                    FontFamily = "Fonts/georgia.ttf#georgia",
                                    TextColor = Color.FromHex("#222"),
                                    Color = Color.FromHex("#222"),
                                    //Type = CheckBox.CheckType.Check,
                                    Padding = new Thickness(10, 0)
                                };
                                check.CheckChangedCommand = new Command((tag) =>
                                {
                                    pmmfCheckBoxCheckChangedCommand(StackLayoutSlRadio, check);
                                });
                                qsn.Children.Add(check);

                            }
                            else if (item.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt())
                            {
                                Label lblQsn = new Label
                                {
                                    Text = "",
                                    TextColor = Color.FromHex("#446377"),
                                    FontSize = 18,
                                    FontFamily = "Fonts/georgia.ttf#georgia"
                                };
                                qsn.Children.Add(lblQsn);
                                showBlock = false;
                                blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString();
                            }

                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SurveyQuestionId" type="hidden" value="@item.SurveyQuestionId" />
                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].SurveyQuestionTypeId" type="hidden" value="@item.SurveyQuestionTypeId" />
                            //    <input name = "PatientSurveyQuestionSetViewModels[0].PatientSurveyQuestionViewModels[@loop_one].DontAskAgain" type="hidden" value="@item.DontAskAgain" id="@idDontAskAgain" />
                        }
                        else
                        {
                            if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt())
                            {
                                CheckBox check = new CheckBox
                                {
                                    Text = item.SurveyQuestionText,
                                    BoxSizeRequest = 26,
                                    TextFontSize = 20,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    //ClassId = classId,
                                    Spacing = 5,
                                    IsChecked = false,
                                    CheckChangedCommand = new Command((tag) =>
                                    {
                                        // CheckBoxCheckChangedCommand(classId, radioButtonGroupView);
                                    }),
                                    FontFamily = "Fonts/georgia.ttf#georgia",
                                    TextColor = Color.FromHex("#222"),
                                    Color = Color.FromHex("#222"),
                                    //Type = CheckBox.CheckType.Check,
                                    Padding = new Thickness(10, 0)
                                };
                                qsn.Children.Add(check);

                            }
                            else
                            {

                                Label lblQsn = new Label
                                {
                                    Text = item.SurveyQuestionText,
                                    TextColor = Color.FromHex("#446377"),
                                    FontSize = 18,
                                    FontFamily = "Fonts/georgia.ttf#georgia"
                                };

                                HtmlLabel lblQsnHtml = new HtmlLabel
                                {
                                    Text = item.SurveyQuestionText,
                                    TextColor = Color.FromHex("#446377"),
                                    FontSize = 18,
                                    FontFamily = "Fonts/georgia.ttf#georgia"
                                };
                                if (item.SurveyQuestionText.Contains("</"))
                                {
                                    qsn.Children.Add(lblQsnHtml);
                                }
                                else
                                {
                                    qsn.Children.Add(lblQsn);

                                }

                            }
                        }


                        //if (blockTwoClassId == "")
                        //{
                        //    blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString();
                        //    showBlock = false;
                        //}

                        StackLayout BlockTwo = new StackLayout { ClassId = blockTwoClassId, BackgroundColor = Color.FromHex("#f0fcfe"), IsVisible = showBlock };
                        if (item.SurveyQuestionTypeId == 4)
                        {
                            //BlockTwo.Margin = new Thickness(-15, 0, 0, 0);
                        }
                        BlockTwo.Children.Add(qsn);


                        if ((item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt()
                            && item.SurveyQuestionTypeId == (int)Enums.SurveyQuestionTypeEnum.Multiple_Checkbox)
                            || (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt()
                            && item.SurveyQuestionTypeId == (int)Enums.SurveyQuestionTypeEnum.Date))
                        {
                            StackLayoutSlRadio.IsVisible = false;
                        }
                        StackLayoutSlRadio.Children.Add(radioButtonGroupView);

                        BlockTwo.Children.Add(StackLayoutSlRadio);
                        blockTwoClassId = "";

                        StackLayoutMainBlock.Children.Add(BlockTwo);

                        if ((item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyGeneralInformation.ToInt()) && (item.SurveyQuestionId < Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationPneumoniaVaccine.ToInt()))
                        {
                            //blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            showBlock = true;
                        }
                        else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications.ToInt())
                        {
                            //blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            showBlock = false;
                        }
                        else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt())
                        {
                            //blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            showBlock = true;
                        }
                        else if (item.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt())
                        {
                            //blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            showBlock = true;
                        }
                        else
                        {
                            //blockTwoClassId = "Class_" + item.SurveyQuestionId.ToString(); ;
                            showBlock = false;
                        }

                        loopOne += 1;
                    }

                    showBlock = true;

                    string buttonText = string.Empty;
                    if (patientSurveyQuestionSetViewModel.IsLastQuestionSet)
                    {
                        buttonText = "Submit";
                    }
                    else
                    {
                        buttonText = "Submit and Continue";
                    }

                    StackLayout buttonMain = new StackLayout { ClassId = "Class_Submit", IsVisible = false, Margin = new Thickness(15, 6) };
                    if (patientSurveyQuestionSetViewModel.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedicalAndFamilyHistory.ToInt()
                            || patientSurveyQuestionSetViewModel.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyPastSurgicalHistory.ToInt()
                            || patientSurveyQuestionSetViewModel.SurveyQuestionSetId == Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications.ToInt())
                    {
                        buttonMain.IsVisible = true;
                    }

                    StackLayout buttonMainSlRadio = new StackLayout { ClassId = "slRadio", Padding = 2 };
                    RadioButtonGroupView buttonGroupView = new RadioButtonGroupView();

                    ButtonExtended buttonSubmit = new ButtonExtended { Text = buttonText };

                    buttonSubmit.Clicked += async delegate { await SubmitButtonClickedAsync(); };

                    buttonGroupView.Children.Add(buttonSubmit);
                    buttonMainSlRadio.Children.Add(buttonGroupView);
                    buttonMain.Children.Add(buttonMainSlRadio);
                    StackLayoutMainBlock.Children.Add(buttonMain);
                }
                else
                {
                    string msg = "Today's Check-In Program is now complete.";
                    if (patientSurveyPatientNotificationDetailViewModel.IsLastNotification)
                    {
                        msg = "Thank you for your participation. We wish you the best of health.";
                    }

                    StackLayout stackMsg = new StackLayout { Margin = new Thickness(0, 40), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, BackgroundColor = Color.FromHex("#d6f7fe"), Padding = new Thickness(20) };
                    Label lblMsg = new Label { FontSize = 22, Text = msg, Margin = new Thickness(0, 0, 0, 40) };
                    stackMsg.Children.Add(lblMsg);

                    UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();

                    ButtonExtended buttonGoHome = new ButtonExtended { Text = "Home" };
                    buttonGoHome.Clicked += (s, e) =>
                    {
                        //App.Current.MainPage = new CustomNavigation.CustomNavigationPage(new MainPage());
                        //App.Current.MainPage = new CustomNavigation.CustomNavigationPage(new MainPatientPage());
                        App.Instance.MainPage = new MenuPage(userIdentityModel);
                    };

                    stackMsg.Children.Add(buttonGoHome);

                    StackLayoutMainBlock.Children.Add(stackMsg);
                }
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
                                var stackLayout = StackLayoutMainBlock.Children.Where(x => x.ClassId == t[1]).FirstOrDefault();
                                if (stackLayout != null)
                                {
                                    StackLayout StackLayoutMainBlockFirstChild = stackLayout as StackLayout;
                                    StackLayoutMainBlockFirstChild.IsVisible = true;
                                    labelGroupStackLayout.ClassId = "HasValue";
                                    if (editor != null)
                                    {
                                        IsSubmitVisibled = false;
                                    }
                                    IsSubmitVisibled = true;
                                    ShowNextElementIfEditdor(StackLayoutMainBlockFirstChild);
                                }

                            }
                        }

                    }
                }
                loop++;
            }

        }

        private void ShowNextElementIfEditdor(StackLayout stackLayoutChildBlock)
        {
            var id = stackLayoutChildBlock.ClassId;
            if (stackLayoutChildBlock.Children.FirstOrDefault(x => x.ClassId == "slRadio") is StackLayout radioButtonGroupStackLayoutTwo)
            {
                var radioButtonGroupView = radioButtonGroupStackLayoutTwo.Children.Where(x => x.GetType() == typeof(RadioButtonGroupView)).FirstOrDefault();// as RadioButtonGroupView;
                RadioButtonGroupView view = ((RadioButtonGroupView)radioButtonGroupView);
                Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
                if (editor != null)
                {
                    if (!string.IsNullOrEmpty(editor.ClassId))
                    {
                        var t = editor.ClassId.ToString().Split(':').ToArray();
                        if (t.Length >= 2)
                        {
                            var stackLayout = StackLayoutMainBlock.Children.Where(x => x.ClassId == t[1]).FirstOrDefault();
                            if (stackLayout != null)
                            {
                                var StackLayoutMainBlockFirstChild = stackLayout as StackLayout;
                                StackLayoutMainBlockFirstChild.IsVisible = true;
                                if (editor != null)
                                {
                                    IsSubmitVisibled = false;
                                }
                                IsSubmitVisibled = true;
                                ShowNextElementIfEditdor(StackLayoutMainBlockFirstChild);
                            }
                        }
                    }
                }
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

        private void pmmfCheckBoxCheckChangedCommand(StackLayout StackLayoutSlRadio, CheckBox checkBox)
        {
            if (checkBox.IsChecked)
            {
                StackLayoutSlRadio.IsVisible = true;
            }
            else
            {
                StackLayoutSlRadio.IsVisible = false;
            }
            //var t = value.ToString().Split(':').ToArray();
            //string nextItem = string.Empty;
            //if (t.Length >= 4)
            //{
            //    nextItem = t[1];
            //}
            //// Set Group Selected Value
            //CheckBox ch = parent;

            //bool isCkecked = false;

            //foreach (var item in ch.Children.Where(x => x.GetType() == typeof(CheckBox)))
            //{
            //    CheckBox checkbox = item as CheckBox;
            //    if (checkbox.IsChecked)
            //    {
            //        isCkecked = true;
            //    }
            //}
            //if (isCkecked)
            //{
            //    ch.ClassId = value.ToString();
            //}
            //else
            //{
            //    ch.ClassId = string.Empty;
            //}
            //HideAllNextBock();
            //ShowNextBocks();
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

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (IsSubmitVisibled) return;
            Editor editor = sender as Editor;
            RadioButtonGroupView ch = editor.Parent as RadioButtonGroupView;
            var value = editor.ClassId;
            var t = value != null ? value.ToString().Split(':').ToArray() : null;
            if (t.Length > 3 && int.Parse(t[3]) == Enums.SurveyQuestionIdForEnum.UsedRestRoomIn24Hour.ToInt())
            {

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

                //editor.Focus();
            }
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
        public async Task AddButtonClickedAsync(long? nextQuestionId)
        {
            await Task.Yield();
            var stackLayout = StackLayoutMainBlock.Children.Where(x => x.ClassId == "Class_" + nextQuestionId).FirstOrDefault();
            if (stackLayout != null)
            {
                StackLayout StackLayoutMainBlockFirstChild = stackLayout as StackLayout;
                StackLayoutMainBlockFirstChild.IsVisible = true;
            }
        }


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
                var stackLayoutChildBlockClassId = stackLayoutChildBlock.ClassId;
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
                                    long surveyQuestionId = Convert.ToInt64(t[3]);

                                    if (surveyQuestionId == (int)Enums.SurveyQuestionIdForEnum.PastMedicalSurveyGeneralInformationHeight)
                                    {
                                        StackLayout generalInfoStackLayout = view.Children.Where(x => x.GetType() == typeof(StackLayout)).FirstOrDefault() as StackLayout;
                                        if (generalInfoStackLayout != null)
                                        {
                                            foreach (var item in generalInfoStackLayout.Children.Where(x => x.GetType() == typeof(Editor)))
                                            {
                                                Editor editor = item as Editor;
                                                if (editor.ClassId == "T-FT")
                                                {
                                                    patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = editor.Text;
                                                }
                                                else if (editor.ClassId == "T-IN")
                                                {
                                                    patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnswareTwo = editor.Text;
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        Editor editor = view.Children.Where(x => x.GetType() == typeof(Editor)).FirstOrDefault() as Editor;
                                        if (editor != null)
                                        {
                                            //string selectedAnsware = editor.Text;

                                            long surveyQuestionSetId;
                                            long.TryParse(t[2], out surveyQuestionSetId);
                                            if (surveyQuestionSetId == (int)Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications || surveyQuestionSetId == (int)Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyAllergies)
                                            {
                                                foreach (var item in view.Children.Where(x => x.GetType() == typeof(Editor)))
                                                {
                                                    editor = item as Editor;
                                                    var itemSelected = editor.ClassId.ToString().Split(':').ToArray();
                                                    long surveyQuestionDetailId = Convert.ToInt64(itemSelected[0]);

                                                    var dataItem = patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().PatientSurveyQuestionDetailViewModels
                                                        .FirstOrDefault(d => d.SurveyQuestionDetailId == surveyQuestionDetailId);

                                                    dataItem.DefaultValue = editor.Text;
                                                    dataItem.IsSelected = !string.IsNullOrEmpty(editor.Text);

                                                }
                                            }
                                            else
                                            {
                                                patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = editor.Text;//selectedAnsware;
                                            }


                                        }
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
                                        patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedAnswareDate.ToString("MM/dd/yyyy");//selectedAnsware;
                                    }

                                }
                                else if (t[4].ToString() == "YD")
                                {
                                    Picker yearPicker = view.Children.Where(x => x.GetType() == typeof(Picker)).FirstOrDefault() as Picker;
                                    if (yearPicker != null)
                                    {
                                        string selectedYear = null;
                                        if (yearPicker.SelectedIndex > -1)
                                        {
                                            selectedYear = yearPicker.SelectedItem.ToString();
                                            long surveyQuestionId = Convert.ToInt64(t[3]);
                                            int setId = Convert.ToInt32(t[2]);
                                            patientSurveyQuestionViewModels.Where(x => x.SurveyQuestionId == surveyQuestionId).FirstOrDefault().SelectedAnsware = selectedYear;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            foreach (var item in patientSurveyQuestionViewModels)
            {
                PatientSurveyQuestionCreateModel modelOne = new PatientSurveyQuestionCreateModel();
                modelOne.SeAns = item.SelectedAnsware;
                modelOne.SeAns2 = item.SelectedAnswareTwo;
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
                    else if (modelOne.SqTypeId == Enums.SurveyQuestionTypeEnum.Text.ToInt() && (modelTwo.SetId == (int)Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyMedications || modelTwo.SetId == (int)Enums.SurveyQuestionSetIdForEnum.PastMedicalSurveyAllergies))
                    {
                        modelTwo.Sel = true;
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
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }

        public async Task ReLoadSurveyWithNextQuestionSet()
        {
            var viewModel = PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel;
            var response = await PatientSurveyClient.GetNextPatientSurvey(viewModel.PracticeProfileId.ToString(), viewModel.ProfessionalProfileId.ToString(), viewModel.PatientProcedureDetailId.ToString(), viewModel.NotificationId.ToString());
            if (response.StatusIsSuccessful)
            {
                PatientSurveyQuestionSetViewModel patientSurveyQuestionSetViewModel = response.Data;
                PatientSurveyPageViewModel.PatientSurveyPatientNotificationDetailViewModel.PatientSurveyQuestionSetViewModel = patientSurveyQuestionSetViewModel;
                CreateForm();
            }
        }

        protected override async void OnAppearing()
        {
            App.ShowUserDialogAsync();

            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                //App.Instance.ClearNavigationAndGoToPage(new LoginPage());
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            await PatientSurveyPageViewModel.ExecuteLoadPatientSurveyNotificationDetailCommandAsync();
            CreateForm();
            App.HideUserDialogAsync();
        }

    }
}