using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.ProfessionalProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientReportedOutcomeSearchCompliancePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private StackLayout notificationButtonContainer;
        private ProfessionalOutcomeRestApiService _restApi;
        private PatientReportedOutcomePatientViewModel _patientReportedOutcomePatientViewModel;
        private PatientAggregateSurveyReportModel _patientAggregateSurveyReportModel;

        public PatientReportedOutcomeSearchCompliancePage(PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _restApi = new ProfessionalOutcomeRestApiService();
                _patientReportedOutcomePatientViewModel = patientReportedOutcomePatientViewModel;
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public PatientReportedOutcomeSearchCompliancePage(ProfessionalPatientBphComplianceProcedureViewModel professionalPatientBphComplianceProcedureViewModel)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                _restApi = new ProfessionalOutcomeRestApiService();
                if (professionalPatientBphComplianceProcedureViewModel != null)
                {
                    _patientReportedOutcomePatientViewModel = new PatientReportedOutcomePatientViewModel();
                    _patientReportedOutcomePatientViewModel.PatientProfileId = professionalPatientBphComplianceProcedureViewModel.PatientProfileId;
                    _patientReportedOutcomePatientViewModel.PatientProcedureDetailId = professionalPatientBphComplianceProcedureViewModel.PatientProcedureDetailId.Value;
                    _patientReportedOutcomePatientViewModel.ProcedureId = professionalPatientBphComplianceProcedureViewModel.ProcedureId;
                }
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void GenerateSurveyActivityView()
        {
            _patientAggregateSurveyReportModel = await _restApi.GetPatientSurveyActivity(_patientReportedOutcomePatientViewModel.PatientProfileId, _patientReportedOutcomePatientViewModel.PatientProcedureDetailId.ToString(), false);
            BuildNotificationButtons();
        }
        private void BuildNotificationButtons()
        {
            notificationButtonContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var latestNotification = _patientAggregateSurveyReportModel.PatientAggregateSurveyReportViewList.Where(d => d.NotificationDate < DateTime.UtcNow).OrderByDescending(d => d.NotificationOrder).FirstOrDefault();

            foreach (var item in _patientAggregateSurveyReportModel.PatientAggregateSurveyReportViewList.OrderBy(m => m.NotificationOrder).GroupBy(c => c.NotificationId))
            {
                Button notificationButton = new Button
                {
                    Text = item.FirstOrDefault().NotificationAbbreviateTitle,
                    Margin = new Thickness(5, 0, 0, 0),
                    BackgroundColor = latestNotification.NotificationId == item.Key ? Color.FromHex("#00FF1A") : Color.FromHex("#DDD"),
                    TextColor = latestNotification.NotificationId == item.Key ? Color.FromHex("#000") : Color.FromHex("#000"),
                    WidthRequest = 70,
                    HeightRequest = 70,
                    CornerRadius = 35,
                    BindingContext = item.FirstOrDefault()
                };
                notificationButton.Clicked += OnNotificationButton_Clicked;

                notificationButtonContainer.Children.Add(notificationButton);
            }

            notificationButtonsStackLayout.Children.Add(notificationButtonContainer);
            GenerateSurveyDataView(latestNotification);
        }

        private void GenerateSurveyDataView(PatientAggregateSurveyReportView selectedNotificationItem)
        {
            patientSurveyActivityGrid.Children.Clear();
            long firstNotificationId = 0;
            var firstItem = _patientAggregateSurveyReportModel.PatientAggregateSurveyReportViewList.OrderBy(c => c.NotificationOrder).FirstOrDefault();
            if (firstItem != null)
                firstNotificationId = firstItem.NotificationId;
            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();

            RowDefinition rowDefinition = new RowDefinition
            {
                Height = GridLength.Auto
            };
            rowDefinitions.Add(rowDefinition);
            var questionList = _patientAggregateSurveyReportModel.PatientAggregateSurveyReportViewList.Where(c => c.NotificationId == firstNotificationId || c.NotificationId == selectedNotificationItem.NotificationId);
            foreach (var SurveyQuestionSet in questionList.GroupBy(c => c.SurveyQuestionSetId))
            {
                foreach (var SurveyQuestionGroup in SurveyQuestionSet.GroupBy(c => c.SurveyQuestionGroupId))
                {
                    rowDefinition = new RowDefinition
                    {
                        Height = GridLength.Auto
                    };
                    rowDefinitions.Add(rowDefinition);

                    foreach (var SurveyQuestion in SurveyQuestionGroup.GroupBy(c => c.SurveyQuestionId))
                    {
                        rowDefinition = new RowDefinition
                        {
                            Height = GridLength.Auto
                        };
                        rowDefinitions.Add(rowDefinition);
                    }
                }
            }
            patientSurveyActivityGrid.RowDefinitions = rowDefinitions;


            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition columnDefinition = new ColumnDefinition
            {
                Width = new GridLength(0.65, GridUnitType.Star)
            };
            columnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition
            {
                Width = new GridLength(0.175, GridUnitType.Star)
            };
            columnDefinitions.Add(columnDefinition);

            columnDefinition = new ColumnDefinition
            {
                Width = new GridLength(0.175, GridUnitType.Star)
            };

            columnDefinitions.Add(columnDefinition);

            patientSurveyActivityGrid.ColumnDefinitions = columnDefinitions;

            StackLayout activityRow = new StackLayout
            {
                BackgroundColor = Color.FromHex("#909090"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            FontSize = 12,
                            Text = "Survey",
                            TextColor = Color.FromHex("#000")
                        }
                    }
            };
            GenerateSurveyNotificationTitleRow(firstItem, selectedNotificationItem, 0);

            int rowCount = 0;
            foreach (var SurveyQuestionSet in questionList.Where(c => c.NotificationId == selectedNotificationItem.NotificationId).GroupBy(c => c.SurveyQuestionSetId))
            {
                foreach (var surveyQuestionGroup in SurveyQuestionSet.GroupBy(c => c.SurveyQuestionGroupId))
                {
                    var surveyQuestionGroupName = surveyQuestionGroup.FirstOrDefault().SurveyQuestionGroupName;
                    if (string.IsNullOrEmpty(surveyQuestionGroupName))
                        surveyQuestionGroupName = SurveyQuestionSet.FirstOrDefault().SurveyQuestionSetName;

                    rowCount++;
                    GenerateSurveyQuestionGroupRow(surveyQuestionGroupName, firstItem, selectedNotificationItem,rowCount);

                    foreach (var surveyQuestionList in surveyQuestionGroup.GroupBy(c => c.SurveyQuestionId))
                    {
                        rowCount++;
                        if (surveyQuestionList != null)
                        {
                            var surveyQuestion = surveyQuestionList.FirstOrDefault();
                            if (surveyQuestion != null)
                            {
                                var surveyQuestionName = !string.IsNullOrEmpty(surveyQuestion.SurveyQuestionReportText) ? surveyQuestion.SurveyQuestionReportText : surveyQuestion.SurveyQuestionShortText;
                                var firstAnswerList = surveyQuestionList.Where(c => c.NotificationId == firstNotificationId).ToList();
                                PatientAggregateSurveyReportView firstAnswer = null;
                                if (firstAnswerList != null && firstAnswerList.Any())
                                    firstAnswer = firstAnswerList.FirstOrDefault();
                                var secondAnswerList = surveyQuestionList.Where(c => c.NotificationId == selectedNotificationItem.NotificationId).ToList();
                                PatientAggregateSurveyReportView secondAnswer = null;
                                if (secondAnswerList != null && secondAnswerList.Any())
                                    secondAnswer = secondAnswerList.FirstOrDefault();

                                GenerateSurveyQuestionRow(firstAnswer, secondAnswer, rowCount);
                            }
                        }
                        
                    }
                }
            }




        }
        private void GenerateSurveyQuestionGroupRow(string surveyQuestionGroupName,PatientAggregateSurveyReportView firstTitle, PatientAggregateSurveyReportView selectedTitle, int rowId)
        {
            StackLayout activityRow = new StackLayout
            {
                BackgroundColor = Color.FromHex("#DDD"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5,0,0,0),
                            FontSize = 12,
                            LineBreakMode = LineBreakMode.WordWrap,
                            Text = surveyQuestionGroupName,
                            TextColor = Color.FromHex("#000")
                        }
                    }
            };
            patientSurveyActivityGrid.Children.Add(activityRow, 0, rowId);
            if (firstTitle != null)
            {
                StackLayout firstAnswerLayout = new StackLayout
                {
                    BackgroundColor = selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId ? Color.FromHex("#00FF1A") : Color.FromHex("#DDD"),
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 12,
                            Text = string.Empty,
                            TextColor = Color.FromHex("#000")
                        }
                    }
                };

                patientSurveyActivityGrid.Children.Add(firstAnswerLayout, 1, rowId);
            }

            if (selectedTitle != null)
            {
                StackLayout firstAnswerLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#00FF1A"),
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 12,
                            Text = string.Empty,
                            TextColor = Color.FromHex("#000")
                        }
                    }
                };
                if (!(firstTitle != null && selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId))
                {
                    patientSurveyActivityGrid.Children.Add(firstAnswerLayout, 2, rowId);
                }

            }
        }

        private string GetSurveyQuestionAnswerFormatedText(PatientAggregateSurveyReportView answer)
        {
            if (answer == null || string.IsNullOrEmpty(answer.SurveyQuestionAnswareText))
                return "";
            if (answer.SurveyQuestionTypeId == (int)Enums.SurveyQuestionTypeEnum.Yes_No)
            {
                return (answer.SurveyQuestionAnswareText.Equals("0") || answer.SurveyQuestionAnswareText.Equals("False")) ? "No" : "Yes";
            }
            else
            {
                if(answer.SurveyQuestionAnswareText.Contains("</span>"))
                {
                    answer.SurveyQuestionAnswareText = answer.SurveyQuestionAnswareText.Replace("<span class='check-circle-red'>", "");
                    answer.SurveyQuestionAnswareText = answer.SurveyQuestionAnswareText.Replace("<span class=\"check-circle-red\">", "");
                    answer.SurveyQuestionAnswareText = answer.SurveyQuestionAnswareText.Replace("<span class='check-circle-black'>", "");
                    answer.SurveyQuestionAnswareText = answer.SurveyQuestionAnswareText.Replace("<span class=\"check-circle-black\">", "");
                    answer.SurveyQuestionAnswareText = answer.SurveyQuestionAnswareText.Replace("</span>", "");
                }
                
                return answer.SurveyQuestionAnswareText;
            }
        }

        private void GenerateSurveyQuestionRow(PatientAggregateSurveyReportView firstAnswer, PatientAggregateSurveyReportView selectedAnswer, int rowId)
        {
            var surveyQuestionText = "";
            if (firstAnswer != null)
            {
                surveyQuestionText = string.IsNullOrEmpty(firstAnswer.SurveyQuestionReportText) ? firstAnswer.SurveyQuestionShortText : firstAnswer.SurveyQuestionReportText;
            }
            else if (selectedAnswer != null)
            {
                surveyQuestionText = string.IsNullOrEmpty(selectedAnswer.SurveyQuestionReportText) ? selectedAnswer.SurveyQuestionShortText : selectedAnswer.SurveyQuestionReportText;
            }
            StackLayout activityRow = new StackLayout
            {
                BackgroundColor = Color.FromHex("#FFF"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5,0,0,0),
                            FontSize = 12,
                            LineBreakMode = LineBreakMode.WordWrap,
                            Text = surveyQuestionText,
                            TextColor = Color.FromHex("#000")
                        }
                    }
            };
            patientSurveyActivityGrid.Children.Add(activityRow, 0, rowId);

            StackLayout firstAnswerLayout = new StackLayout
            {
                BackgroundColor = firstAnswer!=null && selectedAnswer != null && selectedAnswer.NotificationId == firstAnswer.NotificationId ? Color.FromHex("#00FF1A") : Color.FromHex("#FFF"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 12,
                            FontAttributes = firstAnswer!=null && selectedAnswer != null && selectedAnswer.NotificationId == firstAnswer.NotificationId ? FontAttributes.Bold : FontAttributes.None,
                            Text = GetSurveyQuestionAnswerFormatedText(firstAnswer),
                            TextColor = firstAnswer!=null && selectedAnswer != null && selectedAnswer.NotificationId == firstAnswer.NotificationId ? Color.FromHex("#000") : Color.FromHex("#000")
                        }
                    }
            };

            patientSurveyActivityGrid.Children.Add(firstAnswerLayout, 1, rowId);

            StackLayout selectedAnswerLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("#00FF1A"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 12,
                            FontAttributes = firstAnswer!=null && selectedAnswer != null && selectedAnswer.NotificationId == firstAnswer.NotificationId ? FontAttributes.Bold : FontAttributes.None,
                            Text = GetSurveyQuestionAnswerFormatedText(selectedAnswer),
                            TextColor = Color.FromHex("#000")
                        }
                    }
            };
            if (!(firstAnswer != null && selectedAnswer != null && firstAnswer.NotificationId == selectedAnswer.NotificationId))
            {
                patientSurveyActivityGrid.Children.Add(selectedAnswerLayout, 2, rowId);
            }

        }

        private void GenerateSurveyNotificationTitleRow(PatientAggregateSurveyReportView firstTitle, PatientAggregateSurveyReportView selectedTitle, int rowId)
        {
            StackLayout activityRow = new StackLayout
            {
                BackgroundColor = Color.FromHex("#909090"),
                Padding = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5,0,0,0),
                            FontSize = 12,
                            LineBreakMode = LineBreakMode.WordWrap,
                            Text = "Survey",
                            TextColor = Color.FromHex("#000")
                        }
                    }
            };
            patientSurveyActivityGrid.Children.Add(activityRow, 0, rowId);

            if (firstTitle != null)
            {
                StackLayout firstAnswerLayout = new StackLayout
                {
                    BackgroundColor = selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId ? Color.FromHex("#00FF1A") : Color.FromHex("#909090"),
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontAttributes = selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId ? FontAttributes.Bold : FontAttributes.None,
                            FontSize = 12,
                            Text = string.IsNullOrEmpty(firstTitle.NotificationAbbreviateTitle) ? firstTitle.NotificationShortTitle : firstTitle.NotificationAbbreviateTitle,
                            TextColor = Color.FromHex("#000")
                        }
                    }
                };

                patientSurveyActivityGrid.Children.Add(firstAnswerLayout, 1, rowId);
            }

            if (selectedTitle != null)
            {
                StackLayout firstAnswerLayout = new StackLayout
                {
                    BackgroundColor = Color.FromHex("#00FF1A"),
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Margin = new Thickness(5, 0, 0, 0),
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = 12,
                            FontAttributes = selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId ? FontAttributes.Bold : FontAttributes.None,
                            Text = string.IsNullOrEmpty(selectedTitle.NotificationAbbreviateTitle) ? selectedTitle.NotificationShortTitle : selectedTitle.NotificationAbbreviateTitle,
                            TextColor = Color.FromHex("#000")
                        }
                    }
                };
                if (!(firstTitle != null && selectedTitle != null && firstTitle.NotificationId == selectedTitle.NotificationId))
                {
                    patientSurveyActivityGrid.Children.Add(firstAnswerLayout, 2, rowId);
                }

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GenerateSurveyActivityView();
        }

        private void OnNotificationButton_Clicked(object sender, EventArgs e)
        {
            var selectedNotificationButton = sender as Button;
            if (selectedNotificationButton != null)
            {
                var selectedNotification = selectedNotificationButton.BindingContext as PatientAggregateSurveyReportView;
                if (selectedNotificationButton.BackgroundColor == Color.FromHex("#DDD"))
                {
                    var notificationButtonList = notificationButtonContainer.Children.Where(c => c.GetType() == typeof(Button));
                    foreach (var item in notificationButtonList)
                    {
                        var button = item as Button;
                        button.BackgroundColor = Color.FromHex("#DDD");
                        button.TextColor = Color.FromHex("#000");
                    }
                    selectedNotificationButton.BackgroundColor = Color.FromHex("#00FF1A");
                    selectedNotificationButton.TextColor = Color.FromHex("#000");
                    GenerateSurveyDataView(selectedNotification);
                }

            }
        }
    }
}