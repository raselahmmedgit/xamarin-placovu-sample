using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientOutComeReport;
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
    public partial class ProfessionalComparativeAnalyticsPage : ContentPage
    {
        private ProfessionalComparativeAnalyticsPageViewModel professionalComparativeAnalyticsPageViewModel;
        public ProfessionalComparativeAnalyticsPage()
        {
            InitializeComponent();
            professionalComparativeAnalyticsPageViewModel = new ProfessionalComparativeAnalyticsPageViewModel();
            BindingContext = professionalComparativeAnalyticsPageViewModel;
            professionalComparativeAnalyticsPageViewModel.AfterProcedureDataLoad += ProfessionalComparativeAnalyticsPageViewModel_AfterProcedureDataLoad;
            professionalComparativeAnalyticsPageViewModel.AfterSurveyReportMenuDataLoad += ProfessionalComparativeAnalyticsPageViewModel_AfterSurveyReportMenuDataLoad;
            ProcedurePicker.SelectedIndexChanged += ProcedurePicker_SelectedIndexChanged;

            PlotPicker.SelectedIndexChanged += PlotPicker_SelectedIndexChanged;
            // Load Procedure
            professionalComparativeAnalyticsPageViewModel.LoadProcedureShortViewsCommand.Execute(null);
        }
        private void ProfessionalComparativeAnalyticsPageViewModel_AfterProcedureDataLoad(object sender, EventArgs e)
        {
            App.ShowUserDialogAsync();
            ProcedurePicker.Items.Clear();
            var lst = professionalComparativeAnalyticsPageViewModel.ProcedureShortViewModels;
            foreach (var item in lst)
            {
                ProcedurePicker.Items.Add(item.ProcedureName);
            }
            App.HideUserDialogAsync();
        }

        private void ProcedurePicker_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (ProcedurePicker.SelectedItem == null)
            {
                return;
            }
            PlotPicker.Items.Clear();
            PlotPicker.SelectedItem = null;
            App.ShowUserDialogAsync();
            var item = professionalComparativeAnalyticsPageViewModel.ProcedureShortViewModels.FirstOrDefault(x => ProcedurePicker.SelectedItem.ToString() == x.ProcedureName);
            professionalComparativeAnalyticsPageViewModel.ProcedureId = item.ProcedureId;

            if (item.ProcedureId == (int)Enums.ProcedureIdEnum.RoboticPartialNephrectomy
               || item.ProcedureId == (int)Enums.ProcedureIdEnum.RoboticRadicalNephrectomy)
            {
                var page = new NerhrectomyPatientReportPage();
                page.ProcedureId = item.ProcedureId;
                Navigation.PushAsync(page);
            }
            else
            {
                professionalComparativeAnalyticsPageViewModel.LoadSurveyReportMenuCommand.Execute(null);
            }
        }

        private void ShowComparativeAnalysisGraphPage()
        {
            if (PlotPicker.SelectedItem != null)
            {
                App.ShowUserDialogAsync();
                var menuItem = professionalComparativeAnalyticsPageViewModel.ProfessionalSurveyReportMenuShortViews.FirstOrDefault(x => PlotPicker.SelectedItem.ToString() == x.MenuTitle);
                var controllerName = menuItem.GraphReportController == null ? menuItem.ControllerName : menuItem.GraphReportController;
                var actionName = menuItem.GraphReportAction == null ? menuItem.ActionName : menuItem.GraphReportAction;
                string url = $"{controllerName}/{actionName}?chartType=bar";
                
                if (professionalComparativeAnalyticsPageViewModel.ProcedureId == (int)Enums.ProcedureIdEnum.Robotic)
                {
                    var page = new PatientOutComeChartPage(menuItem.MenuId);
                    page.DefaultGraphUrl = url;
                    page.IsPhysicianPatientSelected = MyPatient.IsChecked;
                    page.IsPracticePatientSelected = PracticePatient.IsChecked;
                    Navigation.PushAsync(page);
                }
                else
                {
                    var page = new ComparativeAnalysisGraphPage();
                    page.DefaultGraphUrl = url;
                    page.IsPhysicianPatientSelected = MyPatient.IsChecked;
                    page.IsPracticePatientSelected = PracticePatient.IsChecked;
                    Navigation.PushAsync(page);
                }                
            }
        }

        private void ProfessionalComparativeAnalyticsPageViewModel_AfterSurveyReportMenuDataLoad(object sender, EventArgs e)
        {
            var lst = professionalComparativeAnalyticsPageViewModel.ProfessionalSurveyReportMenuShortViews;
            PlotPicker.Items.Clear();
            foreach (var item in lst)
            {
                PlotPicker.Items.Add(item.MenuTitle);
            }
            //ChooseFilterGroup.IsVisible = true;
            App.HideUserDialogAsync();
        }
        private void PlotPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowComparativeAnalysisGraphPage();
        }

        private void ButtonExtended_Clicked(object sender, EventArgs e)
        {
            //int chk = 0;
            //if(MyPatient.IsChecked)
            //{
            //    chk++;
            //}
            //if (PracticePatient.IsChecked)
            //{
            //    chk++;
            //}
            //if (chk > 0)
            //{
            //    if (ProcedurePicker.SelectedItem == null)
            //    {
            //        PlotFilterGroup.IsVisible = false;
            //    }
            //    else
            //    {
            //        PlotFilterGroup.IsVisible = true;
            //    }
            //}
            //else
            //{
            //    PlotFilterGroup.IsVisible = false;
            //}
            
        }

        private async void ButtonShowGraph_ClickedAsync(object sender, EventArgs e)
        {
            if (ProcedurePicker.SelectedItem == null)
            {
                await DisplayAlert(string.Empty, "Please select comparetive analytics ...", AppConstant.DisplayAlertErrorButtonText);
                return;
            }
            if (IsFilterChecked() == false)
            {
                await DisplayAlert(string.Empty, "Please choose filter ...", AppConstant.DisplayAlertErrorButtonText);
                return;
            }
            if (PlotPicker.SelectedItem == null)
            {
                await DisplayAlert(string.Empty, "Please select plot ...", AppConstant.DisplayAlertErrorButtonText);
                return;
            }           
            ShowComparativeAnalysisGraphPage();
        }        

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        }

        private bool IsFilterChecked()
        {
            int chk = 0;
            if (MyPatient.IsChecked)
            {
                chk++;
            }
            if (PracticePatient.IsChecked)
            {
                chk++;
            }
            return chk > 0;
        }
        
    }
}