using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.ProfessionalProfile.ViewModel;
using OntrackHealthApp.ViewModel;
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
    public partial class ProfessionalPatientBphComplianceSearchDetailPage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private ProfessionalProfileRestApiService professionalProfileRestApiService;

        private PatientReportedOutcomePageViewModel PatientReportedOutcomePageViewModel;
        private PatientComplianceSearchDetailPageViewModel PatientComplianceSearchDetailPageViewModel;
        private PatientReportedOutcomePatientViewModel PatientReportedOutcomePatientViewModel;
        private ProfessionalPatientProfileComplianceViewModel ProfessionalPatientProfileComplianceViewModel;

        private bool IsPatientReportedOutcomeSearchPage;

        public ProfessionalPatientBphComplianceSearchDetailPage()
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                //LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public ProfessionalPatientBphComplianceSearchDetailPage(PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel, ProfessionalPatientProfileComplianceViewModel professionalPatientProfileComplianceViewModel, bool isPatientReportedOutcomeSearchPage = false)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                if (patientReportedOutcomePageViewModel != null)
                {
                    PatientReportedOutcomePageViewModel = patientReportedOutcomePageViewModel;
                }

                if (professionalPatientProfileComplianceViewModel != null)
                {
                    ProfessionalPatientProfileComplianceViewModel = professionalPatientProfileComplianceViewModel;
                }

                if (isPatientReportedOutcomeSearchPage)
                {
                    IsPatientReportedOutcomeSearchPage = isPatientReportedOutcomeSearchPage;
                }

                LoadDataAsync(PatientReportedOutcomePageViewModel.PatientProfileId, PatientReportedOutcomePageViewModel.PatientProcedureDetailId);
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public ProfessionalPatientBphComplianceSearchDetailPage(PatientReportedOutcomePageViewModel patientReportedOutcomePageViewModel, PatientReportedOutcomePatientViewModel patientReportedOutcomePatientViewModel, bool isPatientReportedOutcomeSearchPage = false)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);

                professionalProfileRestApiService = new ProfessionalProfileRestApiService();

                if (patientReportedOutcomePageViewModel != null)
                {
                    PatientReportedOutcomePageViewModel = patientReportedOutcomePageViewModel;
                }

                if (patientReportedOutcomePatientViewModel != null)
                {
                    PatientReportedOutcomePatientViewModel = patientReportedOutcomePatientViewModel;
                }

                if (isPatientReportedOutcomeSearchPage)
                {
                    IsPatientReportedOutcomeSearchPage = isPatientReportedOutcomeSearchPage;
                }

                LoadDataAsync();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadDataAsync(long patientProfileId, string patientProcedureDetailId)
        {
            try
            {
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await App.ShowUserDialogDelayAsync();

                    long professionalProfileId = _iTokenContainer.ApiProfessionalProfileId.ToLong();

                    int reportType = PatientReportedOutcomePageViewModel.TypeId;

                    ProfessionalPatientBphComplianceDetailViewModel professionalPatientBphComplianceDetailViewModel = await professionalProfileRestApiService.PatientBphComplianceSearchDetail(patientProfileId, false, reportType);

                    if (professionalPatientBphComplianceDetailViewModel != null)
                    {
                        BuildHeader(professionalPatientBphComplianceDetailViewModel);

                        if (professionalPatientBphComplianceDetailViewModel.PatientCompletedSurveyActivities != null)
                        {
                            BuildCompletedSurveys(professionalPatientBphComplianceDetailViewModel.PatientCompletedSurveyActivities);
                        }

                        if (professionalPatientBphComplianceDetailViewModel.PatientUpcomingSurveyActivities != null)
                        {
                            BuildUpcomingSurveys(professionalPatientBphComplianceDetailViewModel.PatientUpcomingSurveyActivities);
                        }

                        if (professionalPatientBphComplianceDetailViewModel.PatientNotCompletedSurveyActivities != null)
                        {
                            BuildNotCompletedSurveys(professionalPatientBphComplianceDetailViewModel.PatientNotCompletedSurveyActivities);
                        }

                        ContentHeaderStackLayout.IsVisible = true;
                        ContentStackLayout.IsVisible = true;
                        FooterStackLayout.IsVisible = true;
                        PatientNoteStackLayout.IsVisible = true;
                        App.HideUserDialogAsync();

                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        private async void LoadDataAsync()
        {
            try
            {
                if (InternetConnectHelper.DoIHaveInternet())
                {
                    await App.ShowUserDialogDelayAsync();

                    long professionalProfileId = _iTokenContainer.ApiProfessionalProfileId.ToLong();
                    long patientProfileId = PatientReportedOutcomePatientViewModel.PatientProfileId.ToLong();
                    string patientProcedureDetailId = PatientReportedOutcomePatientViewModel.PatientProcedureDetailId.ToString();
                    int reportType = PatientReportedOutcomePageViewModel.TypeId;

                    ProfessionalPatientBphComplianceDetailViewModel professionalPatientBphComplianceDetailViewModel = await professionalProfileRestApiService.PatientBphComplianceSearchDetail(patientProfileId, false, reportType);

                    if (professionalPatientBphComplianceDetailViewModel != null)
                    {
                        BuildHeader(professionalPatientBphComplianceDetailViewModel);

                        if (professionalPatientBphComplianceDetailViewModel.PatientCompletedSurveyActivities != null)
                        {
                            BuildCompletedSurveys(professionalPatientBphComplianceDetailViewModel.PatientCompletedSurveyActivities);
                        }

                        if (professionalPatientBphComplianceDetailViewModel.PatientUpcomingSurveyActivities != null)
                        {
                            BuildUpcomingSurveys(professionalPatientBphComplianceDetailViewModel.PatientUpcomingSurveyActivities);
                        }

                        if (professionalPatientBphComplianceDetailViewModel.PatientNotCompletedSurveyActivities != null)
                        {
                            BuildNotCompletedSurveys(professionalPatientBphComplianceDetailViewModel.PatientNotCompletedSurveyActivities);
                        }

                        ContentHeaderStackLayout.IsVisible = true;
                        ContentStackLayout.IsVisible = true;
                        FooterStackLayout.IsVisible = true;
                        PatientNoteStackLayout.IsVisible = true;
                        App.HideUserDialogAsync();

                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                    }
                }
                else
                {
                    App.HideUserDialogAsync();
                    await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                App.HideUserDialogAsync();
            }
            finally
            {
                App.HideUserDialogAsync();
            }
        }

        private void BuildHeader(ProfessionalPatientBphComplianceDetailViewModel professionalPatientBphComplianceDetailViewModel)
        {
            PatientComplianceSearchDetailPageViewModel = new PatientComplianceSearchDetailPageViewModel();

            Image headerImage = new Image
            {
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center
            };
            if (PatientReportedOutcomePageViewModel.TypeId == Enums.PatientReportedOutComeReportType.AllRegisteredPatient.ToInt())
            {
                headerImage.Source = "mob_patient.png";
            }
            else if (PatientReportedOutcomePageViewModel.TypeId == Enums.PatientReportedOutComeReportType.FlaggedResult.ToInt())
            {
                headerImage.Source = "mob_red_flag.png";
            }
            else if (PatientReportedOutcomePageViewModel.TypeId == Enums.PatientReportedOutComeReportType.PastMonthCompletedSurvey.ToInt())
            {
                headerImage.Source = "mob_surveycompleted.png";
            }
            else if (PatientReportedOutcomePageViewModel.TypeId == Enums.PatientReportedOutComeReportType.PastMonthPendingSurvey.ToInt())
            {
                headerImage.Source = "mob_surveypending.png";
            }

            HeaderLeftContent.Children.Clear();
            HeaderLeftContent.Children.Add(headerImage);

            string bgColor = PatientReportedOutcomePageViewModel.TypeBgColor;
            //HeaderStackLayout.BackgroundColor = Color.FromHex(bgColor);
            FooterStackLayout.BackgroundColor = Color.FromHex("#FFF");

            if (PatientReportedOutcomePageViewModel.TypeId == Enums.PatientReportedOutComeReportType.FlaggedResult.ToInt())
            {
                string fontColor = "#FFF";
                PatientFullNameLabel.TextColor = Color.FromHex(fontColor);
                //DateOfBirthLabel.TextColor = Color.FromHex(fontColor);
                ProcedureNameLabel.TextColor = Color.FromHex(fontColor);
                SurgeryDateLabel.TextColor = Color.FromHex(fontColor);
                //NotificationDateLabel.TextColor = Color.FromHex(fontColor);
                LocationLabel.TextColor = Color.FromHex(fontColor);
            }

            if (professionalPatientBphComplianceDetailViewModel.PatientProfileViewModel != null)
            {
                PatientComplianceSearchDetailPageViewModel.PatientFullName = professionalPatientBphComplianceDetailViewModel.PatientProfileViewModel.PreferredName;
                PatientFullNameLabel.Text = PatientComplianceSearchDetailPageViewModel.PatientFullName;
                PatientComplianceSearchDetailPageViewModel.DateOfBirth = Convert.ToDateTime(professionalPatientBphComplianceDetailViewModel.PatientProfileViewModel.DateOfBirthStr);
                //DateOfBirthLabel.Text = PatientComplianceSearchDetailPageViewModel.DateOfBirthFormated;
            }

            if (professionalPatientBphComplianceDetailViewModel.PatientProcedureDetailViewModel != null)
            {
                PatientComplianceSearchDetailPageViewModel.ProcedureName = professionalPatientBphComplianceDetailViewModel.PatientProcedureDetailViewModel.ProcedureName;
                ProcedureNameLabel.Text = PatientComplianceSearchDetailPageViewModel.ProcedureName;
                PatientComplianceSearchDetailPageViewModel.SurgeryDateTime = Convert.ToDateTime(professionalPatientBphComplianceDetailViewModel.PatientProcedureDetailViewModel.SurgeryDateTime);
                SurgeryDateLabel.Text = PatientComplianceSearchDetailPageViewModel.SurgeryDateFormated;
                PatientComplianceSearchDetailPageViewModel.NotificationDate = (PatientReportedOutcomePatientViewModel != null ? PatientReportedOutcomePatientViewModel.NotificationDate : null);
                //NotificationDateLabel.Text = PatientComplianceSearchDetailPageViewModel.NotificationDateFormated;
                //NotificationDateLabel.IsVisible = (PatientComplianceSearchDetailPageViewModel.NotificationDate != null ? true : false);
                PatientComplianceSearchDetailPageViewModel.LocationName = professionalPatientBphComplianceDetailViewModel.PatientProcedureDetailViewModel.LocationName;
                LocationLabel.Text = PatientComplianceSearchDetailPageViewModel.LocationNameFormated;
            }

            if (professionalPatientBphComplianceDetailViewModel.CriticalPatientSurveyResults.Count() > 0)
            {
                BuildCriticalSurveys(professionalPatientBphComplianceDetailViewModel.CriticalPatientSurveyResults);
            }

            SurveysStackLayout.IsVisible = true;
        }

        private void BuildCriticalSurveys(IEnumerable<SpForCriticalResultsOfPatientSurvey> criticalPatientSurveyResults)
        {
            CriticalSurveysResultStackLayout.Children.Clear();

            if (criticalPatientSurveyResults.Count() > 0)
            {
                CriticalSurveysStackLayout.IsVisible = true;

                //Result
                StackLayout resultStackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    BackgroundColor = Color.FromHex("#D7D7D7")
                };

                Grid resultGrid = new Grid();
                resultGrid.Margin = new Thickness(0, 0, 0, 0);
                resultGrid.Padding = new Thickness(0, 0, 0, 0);
                resultGrid.ColumnSpacing = 0;
                resultGrid.RowSpacing = 0;
                resultGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                resultGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                resultGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                StackLayout resultIconStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Spacing = 0
                };
                Image resultIconImage = new Image
                {
                    Source = PatientReportedOutComeReportTypeName.SurveyCriticalIcon.ToString(),
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                resultIconStackLayout.Children.Add(resultIconImage);
                resultGrid.Children.Add(resultIconStackLayout, 0, 0);

                StackLayout resultTitleFirstStackLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 0, 0, 0),
                    BackgroundColor = Color.FromHex("#ED220B"),
                    Spacing = 0
                };

                foreach (var item in criticalPatientSurveyResults.ToList())
                {
                    #region Data List

                    StackLayout resultTitleStackLayout = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 0),
                        Spacing = 0
                    };

                    string resultTitle = (item.SurveyTitle + ": " + item.SurveyScore);

                    Label resultTitleLabel = new Label
                    {
                        Text = resultTitle,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    resultTitleStackLayout.Children.Add(resultTitleLabel);

                    resultTitleFirstStackLayout.Children.Add(resultTitleStackLayout);

                    #endregion
                }

                resultGrid.Children.Add(resultTitleFirstStackLayout, 1, 0);

                resultStackLayout.Children.Add(resultGrid);
                //Result

                CriticalSurveysStackLayout.Children.Add(resultStackLayout);
            }

        }

        private void BuildCompletedSurveys(IEnumerable<PatientSurveyActivityViewModel> patientCompletedSurveyActivities)
        {
            CompletedSurveysStackLayout.Children.Clear();

            if (patientCompletedSurveyActivities.Count() == 0)
            {
                #region No Data Found

                //No Data Found
                StackLayout noDataFoundStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Label noDataFoundLabel = new Label
                {
                    Text = "No record(s) found.",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                //No Data Found

                CompletedSurveysStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                foreach (var itemProcedure in patientCompletedSurveyActivities.GroupBy(g => new { g.ProcedureId, g.ProcedureName }))
                {
                    //Procedure Name
                    StackLayout procedureNameStackLayout = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    string procedureName = itemProcedure.Key.ProcedureName;

                    Label procedureNameLabel = new Label
                    {
                        Text = procedureName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    procedureNameStackLayout.Children.Add(procedureNameLabel);
                    //Procedure Name

                    foreach (var itemNotification in itemProcedure.GroupBy(g => new { g.NotificationId, g.NotificationTitle }))
                    {
                        //Notification
                        StackLayout notificationStackLayout = new StackLayout
                        {
                            BackgroundColor = Color.FromHex("#ffffff"),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Padding = new Thickness(0, 0, 0, 0),
                            Margin = new Thickness(0, 0, 0, 5)
                        };

                        Grid notificationGrid = new Grid();
                        notificationGrid.Margin = new Thickness(0, 0, 0, 0);
                        notificationGrid.Padding = new Thickness(0, 0, 0, 0);
                        notificationGrid.ColumnSpacing = 0;
                        notificationGrid.RowSpacing = 0;
                        notificationGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                        StackLayout notificationIconStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };
                        Image notificationIconImage = new Image
                        {
                            Source = "patient_outcome_bullet_compliance.png",
                            Aspect = Aspect.AspectFit,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        notificationIconStackLayout.Children.Add(notificationIconImage);
                        notificationGrid.Children.Add(notificationIconStackLayout, 0, 0);

                        StackLayout notificationTitleStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };

                        string notificationTitle = itemNotification.Key.NotificationTitle;

                        Label notificationTitleLabel = new Label
                        {
                            Text = notificationTitle,
                            TextColor = Color.FromHex("#000"),
                            FontSize = 14,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        };
                        notificationTitleStackLayout.Children.Add(notificationTitleLabel);
                        notificationGrid.Children.Add(notificationTitleStackLayout, 1, 0);

                        notificationStackLayout.Children.Add(notificationGrid);
                        //Notification

                        CompletedSurveysStackLayout.Children.Add(procedureNameStackLayout);
                        CompletedSurveysStackLayout.Children.Add(notificationStackLayout);
                    }
                }

                #endregion
            }

        }

        private void BuildUpcomingSurveys(IEnumerable<PatientSurveyActivityViewModel> patientUpcomingSurveyActivities)
        {
            UpcomingSurveysStackLayout.Children.Clear();

            if (patientUpcomingSurveyActivities.Count() == 0)
            {
                #region No Data Found

                //No Data Found
                StackLayout noDataFoundStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Label noDataFoundLabel = new Label
                {
                    Text = "No record(s) found.",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                //No Data Found

                UpcomingSurveysStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                foreach (var itemProcedure in patientUpcomingSurveyActivities.GroupBy(g => new { g.ProcedureId, g.ProcedureName }))
                {
                    //Procedure Name
                    StackLayout procedureNameStackLayout = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    string procedureName = itemProcedure.Key.ProcedureName;

                    Label procedureNameLabel = new Label
                    {
                        Text = procedureName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    procedureNameStackLayout.Children.Add(procedureNameLabel);
                    //Procedure Name

                    foreach (var itemNotification in itemProcedure.GroupBy(g => new { g.NotificationId, g.NotificationTitle }))
                    {
                        //Notification
                        StackLayout notificationStackLayout = new StackLayout
                        {
                            BackgroundColor = Color.FromHex("#ffffff"),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Padding = new Thickness(0, 0, 0, 0),
                            Margin = new Thickness(0, 0, 0, 5)
                        };

                        Grid notificationGrid = new Grid();
                        notificationGrid.Margin = new Thickness(0, 0, 0, 0);
                        notificationGrid.Padding = new Thickness(0, 0, 0, 0);
                        notificationGrid.ColumnSpacing = 0;
                        notificationGrid.RowSpacing = 0;
                        notificationGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                        StackLayout notificationIconStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };
                        Image notificationIconImage = new Image
                        {
                            Source = "patient_outcome_bullet_compliance.png",
                            Aspect = Aspect.AspectFit,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        notificationIconStackLayout.Children.Add(notificationIconImage);
                        notificationGrid.Children.Add(notificationIconStackLayout, 0, 0);

                        StackLayout notificationTitleStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };

                        string notificationTitle = itemNotification.Key.NotificationTitle;

                        Label notificationTitleLabel = new Label
                        {
                            Text = notificationTitle,
                            TextColor = Color.FromHex("#000"),
                            FontSize = 14,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        };
                        notificationTitleStackLayout.Children.Add(notificationTitleLabel);
                        notificationGrid.Children.Add(notificationTitleStackLayout, 1, 0);

                        notificationStackLayout.Children.Add(notificationGrid);
                        //Notification

                        UpcomingSurveysStackLayout.Children.Add(procedureNameStackLayout);
                        UpcomingSurveysStackLayout.Children.Add(notificationStackLayout);
                    }
                }

                #endregion
            }

        }

        private void BuildNotCompletedSurveys(IEnumerable<PatientSurveyActivityViewModel> patientNotCompletedSurveyActivities)
        {
            NotCompletedSurveysStackLayout.Children.Clear();

            if (patientNotCompletedSurveyActivities.Count() == 0)
            {
                #region No Data Found

                //No Data Found
                StackLayout noDataFoundStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Label noDataFoundLabel = new Label
                {
                    Text = "No record(s) found.",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                //No Data Found

                NotCompletedSurveysStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                #region Data List

                foreach (var itemProcedure in patientNotCompletedSurveyActivities.GroupBy(g => new { g.ProcedureId, g.ProcedureName }))
                {
                    //Procedure Name
                    StackLayout procedureNameStackLayout = new StackLayout
                    {
                        Padding = new Thickness(5, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    string procedureName = itemProcedure.Key.ProcedureName;

                    Label procedureNameLabel = new Label
                    {
                        Text = procedureName,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    procedureNameStackLayout.Children.Add(procedureNameLabel);
                    //Procedure Name

                    foreach (var itemNotification in itemProcedure.GroupBy(g => new { g.NotificationId, g.NotificationTitle }))
                    {
                        //Notification
                        StackLayout notificationStackLayout = new StackLayout
                        {
                            BackgroundColor = Color.FromHex("#ffffff"),
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Padding = new Thickness(0, 0, 0, 0),
                            Margin = new Thickness(0, 0, 0, 5)
                        };

                        Grid notificationGrid = new Grid();
                        notificationGrid.Margin = new Thickness(0, 0, 0, 0);
                        notificationGrid.Padding = new Thickness(0, 0, 0, 0);
                        notificationGrid.ColumnSpacing = 0;
                        notificationGrid.RowSpacing = 0;
                        notificationGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                        notificationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                        StackLayout notificationIconStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };
                        Image notificationIconImage = new Image
                        {
                            Source = "patient_outcome_bullet_compliance.png",
                            Aspect = Aspect.AspectFit,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        };
                        notificationIconStackLayout.Children.Add(notificationIconImage);
                        notificationGrid.Children.Add(notificationIconStackLayout, 0, 0);

                        StackLayout notificationTitleStackLayout = new StackLayout
                        {
                            Padding = new Thickness(0, 0, 0, 0),
                            Spacing = 0
                        };

                        string notificationTitle = itemNotification.Key.NotificationTitle;

                        Label notificationTitleLabel = new Label
                        {
                            Text = notificationTitle,
                            TextColor = Color.FromHex("#000"),
                            FontSize = 14,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            VerticalOptions = LayoutOptions.Center
                        };
                        notificationTitleStackLayout.Children.Add(notificationTitleLabel);
                        notificationGrid.Children.Add(notificationTitleStackLayout, 1, 0);

                        notificationStackLayout.Children.Add(notificationGrid);
                        //Notification

                        NotCompletedSurveysStackLayout.Children.Add(procedureNameStackLayout);
                        NotCompletedSurveysStackLayout.Children.Add(notificationStackLayout);
                    }
                }

                #endregion
            }

        }
        private void BuildPatientNotes(IEnumerable<PatientNoteViewModel> PatientNoteViewModels)
        {
            PatientNoteStackLayout.Children.Clear();

            if (PatientNoteViewModels == null || PatientNoteViewModels.Count() == 0)
            {
                #region No Data Found

                //No Data Found
                StackLayout noDataFoundStackLayout = new StackLayout
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Label noDataFoundLabel = new Label
                {
                    Text = "No record(s) found.",
                    TextColor = Color.FromHex("#000"),
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                noDataFoundStackLayout.Children.Add(noDataFoundLabel);
                //No Data Found

                PatientNoteStackLayout.Children.Add(noDataFoundStackLayout);

                #endregion
            }
            else
            {
                foreach (var item in PatientNoteViewModels)
                {
                    #region Data List

                    //Notification
                    StackLayout notestackLayout = new StackLayout
                    {
                        BackgroundColor = Color.FromHex("#ffffff"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    Grid noteGrid = new Grid();
                    noteGrid.Margin = new Thickness(0, 0, 0, 0);
                    noteGrid.Padding = new Thickness(0, 0, 0, 0);
                    noteGrid.ColumnSpacing = 0;
                    noteGrid.RowSpacing = 0;
                    noteGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    noteGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    StackLayout noteDateStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        Spacing = 0
                    };
                    Label noteDateLabel = new Label
                    {
                        Text = item.CreatedDateFormated,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    noteDateStackLayout.Children.Add(noteDateLabel);
                    noteGrid.Children.Add(noteDateStackLayout, 0, 0);
                    notestackLayout.Children.Add(noteGrid);

                    StackLayout noteTextStackLayout = new StackLayout
                    {
                        Padding = new Thickness(0, 0, 0, 0),
                        Spacing = 0
                    };

                    Label noteTextLabel = new Label
                    {
                        Text = item.Notes,
                        TextColor = Color.FromHex("#000"),
                        FontSize = 14,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    };
                    noteTextStackLayout.Children.Add(noteTextLabel);
                    noteGrid.Children.Add(noteTextStackLayout, 0, 0);

                    notestackLayout.Children.Add(noteGrid);
                    //Notification

                    PatientNoteStackLayout.Children.Add(notestackLayout);

                    #endregion
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_iTokenContainer.IsApiToken())
            {
                App.Instance.ClearNavigationAndGoToPage(new LoginPageNew());
            }
            Title = _iTokenContainer.ApiPracticeName;
        }

        private async void BtnOutComeReport_Clicked(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                try
                {
                    App.ShowUserDialogAsync();

                    if (PatientReportedOutcomePatientViewModel != null)
                    {
                        await Navigation.PushAsync(new ProfessionalPatientBphComplianceProcedurePage(PatientReportedOutcomePatientViewModel));
                    }


                    App.HideUserDialogAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }

        }



        #region Bottom Menu Actions

        private async void OnHomeButtonClickedAsync(object sender, EventArgs e)
        {
            if (InternetConnectHelper.DoIHaveInternet())
            {
                HomeButton();
            }
            else
            {
                await DisplayAlert(string.Empty, AppConstant.NoInternetConnectMessage, AppConstant.DisplayAlertErrorButtonText);
            }
        }

        private void HomeButton()
        {
            //App.ShowUserDialogAsync();
            UserIdentityModel userIdentityModel = _iTokenContainer.GetUserIdentityModel();
            App.Instance.MainPage = new MenuPage(userIdentityModel);
            //App.Instance.MainPage = new MenuProfessionalPage();
        }

        #endregion
    }
}