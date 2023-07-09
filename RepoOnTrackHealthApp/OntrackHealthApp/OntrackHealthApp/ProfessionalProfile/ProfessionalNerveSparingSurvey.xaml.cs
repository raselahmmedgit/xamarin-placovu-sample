using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.Model;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalNerveSparingSurvey : CustomModalContentPage
    {
        private SurgicalConciergePatientViewModel SurgicalConciergePatientViewModel;
        private List<SurveyQuestionDetail> SurveyQuestionDetails;

        public ProfessionalNerveSparingSurvey(List<SurveyQuestionDetail> surveyQuestionDetails, SurgicalConciergePatientViewModel surgicalConciergePatientViewModel = null)
        {
            InitializeComponent();
            SurgicalConciergePatientViewModel = surgicalConciergePatientViewModel;
            SurveyQuestionDetails = surveyQuestionDetails;
            CreateForm();
        }

        private void CreateForm()
        {
            StackLayoutMainBlock.Children.Clear();

            var surveyQuestionDetailViewModels = SurveyQuestionDetails;
            if (surveyQuestionDetailViewModels != null)
            {

                #region SurveyQuestionDetail

                int loopOne = 0;

                StackLayout surveyQuestionLayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                //#region Header

                //StackLayout practiceHeaderStackLayout = new StackLayout
                //{
                //    VerticalOptions = LayoutOptions.StartAndExpand,
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //    Padding = new Thickness(10, 5)
                //};

                //HtmlLabel labelHeaderTitle = new HtmlLabel()
                //{
                //    FontSize = 21,
                //    Text = "Hospital",
                //    FontFamily = "Fonts/georgia.ttf#georgia",
                //    TextColor = Color.FromHex("#222")
                //};
                //practiceHeaderStackLayout.Children.Add(labelHeaderTitle);

                //practiceStackLayout.Children.Add(practiceHeaderStackLayout);

                //#endregion

                #region Body

                StackLayout surveyBodyStackLayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(15, 10),
                    BackgroundColor = Color.FromHex("#d6f7fe")
                };

                RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                foreach (var item in surveyQuestionDetailViewModels)
                {
                    string classId = item.SurveyQuestionDetailId.ToString();

                    //Radiobutton
                    classId = classId + "_" + item.QuestionDetailText;
                    CheckBox check = new CheckBox
                    {
                        Text = item.QuestionDetailText,
                        BoxSizeRequest = 24,
                        TextFontSize = 19,
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
                        Padding = new Thickness(10, 0)
                    };

                    radioButtonGroupView.Children.Add(check);
                    BoxView bxCheck = new BoxView
                    {
                        BackgroundColor = Color.FromHex("#d6f7fe"),
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    radioButtonGroupView.Children.Add(bxCheck);

                    loopOne += 1;
                }//

                surveyBodyStackLayout.Children.Add(radioButtonGroupView);

                surveyQuestionLayout.Children.Add(surveyBodyStackLayout);

                #endregion

                StackLayoutMainBlock.Children.Add(surveyQuestionLayout);

                #endregion
            }
        }

        //private async void PatientNerveSparingSumbitButtonClicked(object sender, EventArgs e)
        //{
        //    SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
        //    ApiExecutionResult<NerveSparingSelectionViewModel> apiExecutionResult = new ApiExecutionResult<NerveSparingSelectionViewModel>();
        //    NerveSparingSelectionViewModel nerveSparingSelectionViewModel = new NerveSparingSelectionViewModel
        //    {
        //        PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId,
        //        PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId,
        //        ProfessionalProfileId = SurgicalConciergePatientViewModel.ProfessionalProfileId ?? 0,
        //        PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid(),
        //        //SurveyQuestionDetailId =
        //        //SurveyQuestionAnswareText =
        //    }; 
        //    apiExecutionResult = await restService.SubmitNerveSparingSelection(nerveSparingSelectionViewModel);
        //    await Navigation.PopModalAsync();
        //}

        private async void PatientNerveSparingCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        private async void CheckBoxCheckChangedCommand(object value, RadioButtonGroupView parent)
        {
            var classIdArr = value.ToString().Split('_').ToArray();

            if (classIdArr != null)
            {
                long surveyQuestionDetailId = classIdArr[0].ToLong();
                string surveyQuestionAnswareText = classIdArr[1].ToString();

                SurgicalConciergeRestApiService restService = new SurgicalConciergeRestApiService();
                ApiExecutionResult<NerveSparingSelectionViewModel> apiExecutionResult = new ApiExecutionResult<NerveSparingSelectionViewModel>();
                NerveSparingSelectionViewModel nerveSparingSelectionViewModel = new NerveSparingSelectionViewModel
                {
                    PatientProfileId = SurgicalConciergePatientViewModel.PatientProfileId,
                    PracticeProfileId = SurgicalConciergePatientViewModel.PracticeProfileId,
                    ProfessionalProfileId = SurgicalConciergePatientViewModel.ProfessionalProfileId ?? 0,
                    PatientProcedureDetailId = SurgicalConciergePatientViewModel.PatientProcedureDetailId.ToGuid(),
                    SurveyQuestionDetailId = surveyQuestionDetailId,
                    SurveyQuestionAnswareText = surveyQuestionAnswareText
                };
                apiExecutionResult = await restService.SubmitNerveSparingSelection(nerveSparingSelectionViewModel);
                await Navigation.PopModalAsync();
            }
        }
    }
}