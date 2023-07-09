using Acr.UserDialogs;
using LabelHtml.Forms.Plugin.Abstractions;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.RestApiService;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using Plugin.InputKit.Shared.Controls;
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
	public partial class OutcomeSearchFilterView : ContentPage
	{
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeSidebarSearchViewModel SurgicalConciergeSidebarSearchViewModel { get; set; }

        private List<string> SelectedProcedure = new List<string>();
        PatientReportedOutcomePatientListPage _patientReportedOutcomePatientListPage;
        PatientReportedOutcomeMonthPage _patientReportedOutcomeMonthPage;
        PatientReportedOutcomePage _patientReportedOutcomePage;

        public OutcomeSearchFilterView(PatientReportedOutcomePatientListPage patientReportedOutcomePatientListPage)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                this._patientReportedOutcomePatientListPage = patientReportedOutcomePatientListPage;

                this.SelectedProcedure = patientReportedOutcomePatientListPage.SelectedProcedure;

                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public OutcomeSearchFilterView(PatientReportedOutcomeMonthPage patientReportedOutcomeMonthPage)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                this._patientReportedOutcomeMonthPage = patientReportedOutcomeMonthPage;

                this.SelectedProcedure = patientReportedOutcomeMonthPage.SelectedProcedure;

                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        public OutcomeSearchFilterView(PatientReportedOutcomePage patientReportedOutcomePage)
        {
            if (InternetConnectHelper.CheckConnection())
            {
                InitializeComponent();
                _iTokenContainer = new TokenContainer();
                Title = _iTokenContainer.ApiPracticeName;

                this._patientReportedOutcomePage = patientReportedOutcomePage;

                this.SelectedProcedure = patientReportedOutcomePage.SelectedProcedure;

                LoadDataAsyc();
            }
            else
            {
                App.Instance.ClearNavigationAndGoToPage(new InternetConnectPage());
            }
        }

        private async void LoadDataAsyc()
        {
            App.ShowUserDialog();
            using (UserDialogs.Instance.Loading(""))
            {
                try
                {
                    var restApiService = new SurgicalConciergeRestApiService();
                    var _surgicalConciergeSidebarSearchViewModels = await restApiService.GetOutcomeSidebarSearchViewModels();

                    if (_surgicalConciergeSidebarSearchViewModels != null)
                    {
                        SurgicalConciergeSidebarSearchViewModel = _surgicalConciergeSidebarSearchViewModels;
                    }
                    else
                    {
                        await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
                    }

                    CreateForm();

                    App.HideUserDialog();
                }
                catch (Exception)
                {
                    await DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.DisplayAlertErrorMessage, AppConstant.DisplayAlertErrorButtonText);
                }
            }

        }

        private void CreateForm()
        {
            StackLayoutMainBlock.Children.Clear();

            var surgicalConciergeSidebarSearchViewModel = SurgicalConciergeSidebarSearchViewModel;
            if (surgicalConciergeSidebarSearchViewModel != null)
            {
                //Procedure
                var procedureCheckBoxListViewModels = surgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews;
                if (procedureCheckBoxListViewModels != null)
                {
                    #region Procedure

                    int loopOne = 0;

                    StackLayout procedureStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    #region Header

                    StackLayout procedureHeaderStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(10, 5)
                    };

                    HtmlLabel labelHeaderTitle = new HtmlLabel()
                    {
                        FontSize = 21,
                        Text = "Procedure",
                        FontFamily = "Fonts/georgia.ttf#georgia",
                        TextColor = Color.FromHex("#222")
                    };
                    procedureHeaderStackLayout.Children.Add(labelHeaderTitle);

                    procedureStackLayout.Children.Add(procedureHeaderStackLayout);

                    #endregion

                    #region Body

                    StackLayout procedureBodyStackLayout = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(15, 10),
                        BackgroundColor = Color.FromHex("#d6f7fe")
                    };

                    RadioButtonGroupView radioButtonGroupView = new RadioButtonGroupView();

                    foreach (var item in procedureCheckBoxListViewModels)
                    {
                        string classId = item.ProcedureId.ToString();
                        bool procedureIsChecked = SelectedProcedure.Where(x => x.Equals(classId)).Any();
                        SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.Where(x => x.ProcedureId == Convert.ToInt64(classId)).FirstOrDefault().IsChecked = procedureIsChecked;

                        //Checkbox
                        classId = "ProcedureId_" + classId;
                        CheckBox check = new CheckBox
                        {
                            Text = item.ProcedureName,
                            BoxSizeRequest = 24,
                            TextFontSize = 19,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            ClassId = classId,
                            Spacing = 5,
                            IsChecked = procedureIsChecked,
                            CheckChangedCommand = new Command((tag) => {
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

                    procedureBodyStackLayout.Children.Add(radioButtonGroupView);

                    procedureStackLayout.Children.Add(procedureBodyStackLayout);

                    #endregion

                    StackLayoutMainBlock.Children.Add(procedureStackLayout);

                    #endregion
                }

            }
            else
            {
                DisplayAlert(AppConstant.DisplayAlertErrorTitle, AppConstant.NotFound, AppConstant.DisplayAlertErrorButtonText);
            }

            App.HideUserDialog();
        }


        private void CheckBoxCheckChangedCommand(object value, RadioButtonGroupView parent)
        {
            var classIdArr = value.ToString().Split('_').ToArray();
            string id = string.Empty;
            string idValue = string.Empty;
            if (classIdArr != null)
            {
                id = classIdArr[0].ToString();
                idValue = classIdArr[1].ToString();
            }

            if (id == "ProcedureId")
            {
                SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.Where(s => s.ProcedureId == Convert.ToInt64(idValue)).ToList().ForEach(c =>
                {
                    if (c.IsChecked == true)
                    {
                        c.IsChecked = false;
                    }
                    else
                    {
                        c.IsChecked = true;
                    }
                });
            }

        }// Data


        private async void ReLoadData()
        {
            if (SurgicalConciergeSidebarSearchViewModel != null)
            {

                if (SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews != null)
                {
                    var procedureCheckBoxListViewModels = SurgicalConciergeSidebarSearchViewModel.ProcedureCheckBoxListViews.ToList().Where(w => w.IsChecked == true).Select(n => n.ProcedureId.ToString()).ToList();

                    //foreach (var procedure in procedureCheckBoxListViewModels)
                    //{
                    //    if(!SelectedProcedure.Contains(procedure))
                    //    {
                    //        SelectedProcedure.Add(procedure);
                    //    }
                    //}

                    SelectedProcedure = procedureCheckBoxListViewModels;
                }
            }

            if (_patientReportedOutcomePatientListPage != null)
            {
                _patientReportedOutcomePatientListPage.SelectedProcedure = SelectedProcedure;
                await Navigation.PopModalAsync();
                _patientReportedOutcomePatientListPage.LoadDataAsyc();
            }
            if (_patientReportedOutcomeMonthPage != null)
            {
                _patientReportedOutcomeMonthPage.SelectedProcedure = SelectedProcedure;
                await Navigation.PopModalAsync();
            }
            if (_patientReportedOutcomePage != null)
            {
                _patientReportedOutcomePage.SelectedProcedure = SelectedProcedure;
                await Navigation.PopModalAsync();
            }
        }

        private void OutcomeSearchFilterSumbitButtonClicked(object sender, EventArgs e)
        {
            ReLoadData();
        }


        private async void OutcomeSearchFilterCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}