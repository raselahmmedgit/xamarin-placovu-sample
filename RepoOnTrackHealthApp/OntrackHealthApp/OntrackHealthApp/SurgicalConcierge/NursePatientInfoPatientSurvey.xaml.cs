using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Model;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NursePatientInfoPatientSurvey : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        SurgicalConciergeRestApiService restApiService;

        public NursePatientInfoPatientSurvey(Guid patientProcedureDetailId, SurgicalConciergePatientViewModel selectedPatient)
        {
            _iTokenContainer = new TokenContainer();
            restApiService = new SurgicalConciergeRestApiService();
            InitializeComponent();
            showPatientInfo(selectedPatient);
            loadPatientSurvey(patientProcedureDetailId);
        }
        private void showPatientInfo(SurgicalConciergePatientViewModel selectedPatient)
        {
            Title = _iTokenContainer.ApiPracticeName;
            PatientFullName.Text = "Patient : " + selectedPatient.PatientFullName;
            ProcedureName.Text = "Procedure : " + selectedPatient.ProcedureName;
            ProfessionalName.Text = "Professional : " + selectedPatient.ProfessionalName;

        }
        private async void loadPatientSurvey(Guid patientProcedureDetailId)
        {
            SurgicalConciergeNursePatientSurveyViewModel patientSurveyViewModel = await restApiService.GetNursePatientInfoPatientSurvey(patientProcedureDetailId);
            if (patientSurveyViewModel == null)
                return;

            if (patientSurveyViewModel.PatientSurvey != null && patientSurveyViewModel.PatientSurvey.DataList != null && patientSurveyViewModel.PatientSurvey.DataList.Any())
            {
                //define row
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = GridLength.Auto}
                };
                foreach (var x in patientSurveyViewModel.PatientSurvey.DataList)
                {
                    rowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }

                PatientServeyViewData.RowDefinitions = rowDefinitions;

                //define colum
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=GridLength.Star},
                    new ColumnDefinition{Width=GridLength.Star},
                    new ColumnDefinition{Width=GridLength.Star},
                    new ColumnDefinition{Width=GridLength.Star},
                    new ColumnDefinition{Width=GridLength.Star},
                };
                PatientServeyViewData.ColumnDefinitions = colDefitions;

                //define header
                var item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#ddd")

                };
                item.Children.Add(new Label { Text = "Title", FontSize = 14, TextColor = Color.FromHex("#333") });
                PatientServeyViewData.Children.Add(item, 0, 0);

                item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#00a2ff")
                };
                item.Children.Add(new Label { Text = "2 Days", FontSize = 14, TextColor = Color.FromHex("#ffffff") });
                PatientServeyViewData.Children.Add(item, 1, 0);
                item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#00a2ff")
                };
                item.Children.Add(new Label { Text = "4 Days", FontSize = 14, TextColor = Color.FromHex("#ffffff") });
                PatientServeyViewData.Children.Add(item, 2, 0);
                item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#00a2ff")

                };
                item.Children.Add(new Label { Text = "6 Days", FontSize = 14, TextColor = Color.FromHex("#ffffff") });
                PatientServeyViewData.Children.Add(item, 3, 0);
                item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#61d836")

                };
                item.Children.Add(new Label { Text = "My Patients", FontSize = 14, TextColor = Color.FromHex("#ffffff") });
                PatientServeyViewData.Children.Add(item, 4, 0);

                var rowCount = 0;
                foreach (var data in patientSurveyViewModel.PatientSurvey.DataList)
                {
                    rowCount++;
                    var rowBgColor = "#ddd";
                    if (rowCount % 2 == 0)
                        rowBgColor = "#ddd";
                    else
                        rowBgColor = "#fff";

                    item = new StackLayout
                    {
                        Padding = 12,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex(rowBgColor)
                    };
                    item.Children.Add(new Label { Text = data.SurveyQuestionShortText, FontSize = 14, TextColor = Color.FromHex("#333") });
                    PatientServeyViewData.Children.Add(item, 0, rowCount);

                    item = new StackLayout
                    {
                        Padding = 12,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex(rowBgColor)
                    };
                    item.Children.Add(new HtmlLabel { Text = data.TwoDayPatientSurvey, FontSize = 14, TextColor = Color.FromHex("#333") });
                    PatientServeyViewData.Children.Add(item, 1, rowCount);
                    item = new StackLayout
                    {
                        Padding = 12,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex(rowBgColor)
                    };
                    item.Children.Add(new HtmlLabel { Text = data.FourDayPatientSurvey, FontSize = 14, TextColor = Color.FromHex("#333") });
                    PatientServeyViewData.Children.Add(item, 2, rowCount);
                    item = new StackLayout
                    {
                        Padding = 12,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex(rowBgColor)
                    };
                    item.Children.Add(new HtmlLabel { Text = data.SixDayPatientSurvey, FontSize = 14, TextColor = Color.FromHex("#333") });
                    PatientServeyViewData.Children.Add(item, 3, rowCount);
                    item = new StackLayout
                    {
                        Padding = 12,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.FromHex(rowBgColor)
                    };
                    item.Children.Add(new Label { Text = data.ProfessionalSurvey, FontSize = 14, TextColor = Color.FromHex("#333") });
                    PatientServeyViewData.Children.Add(item, 4, rowCount);
                }
            }
            else
            {
                var rowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition{ Height = GridLength.Auto}
                };
                var colDefitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition{Width=GridLength.Star}
                };
                PatientServeyViewData.RowDefinitions = rowDefinitions;
                PatientServeyViewData.ColumnDefinitions = colDefitions;
                var item = new StackLayout
                {
                    Padding = 12,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHex("#ddd")
                };
                item.Children.Add(new Label { Text = "No survey data found", FontSize = 14, TextColor = Color.FromHex("#333") });
                PatientServeyViewData.Children.Add(item, 0, 0);

            }

            if (patientSurveyViewModel.ProfessionaNotesToNurseItems != null && patientSurveyViewModel.ProfessionaNotesToNurseItems.Any())
            {
                NurseNotesListView.ItemsSource = patientSurveyViewModel.ProfessionaNotesToNurseItems;
            }
        }
        
    }
}