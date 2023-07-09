using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.PatientOutComeReport.ViewModel;
using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ProfessionalProfile.RestApiService;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalComparativeAnalyticsPageViewModel : ViewModelBase
    {
        public event EventHandler AfterProcedureDataLoad;
        public event EventHandler AfterSurveyReportMenuDataLoad;
        
        public void OnAfterProcedureDataLoad()
        {
            AfterProcedureDataLoad(this, EventArgs.Empty);
        }
        public void OnAfterSurveyReportMenuDataLoad()
        {
            AfterSurveyReportMenuDataLoad(this, EventArgs.Empty);
        }
        public long ProcedureId { get; set; }
        public ObservableCollection<ProcedureShortViewModel> ProcedureShortViewModels { get; set; }
        public ObservableCollection<ProfessionalSurveyReportMobileMenu> ProfessionalSurveyReportMenuShortViews { get; set; }
        public Command LoadProcedureShortViewsCommand { get; set; }
        public Command LoadSurveyReportMenuCommand { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private readonly ProfessionalProfileRestApiService _restApiService;

        public ProfessionalComparativeAnalyticsPageViewModel()
		{
            _iTokenContainer = new TokenContainer();
            _restApiService = new ProfessionalProfileRestApiService();
            LoadProcedureShortViewsCommand = new Command(async () => await ExecuteLoadProcedureShortViewsCommandAsync());
            LoadSurveyReportMenuCommand = new Command(async () => await ExecuteLoadSurveyReportMenuCommandAsync());
        }

        public async Task ExecuteLoadProcedureShortViewsCommandAsync()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }            
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                ProcedureShortViewModels = new ObservableCollection<ProcedureShortViewModel>();
                var data = await _restApiService.GetProfessionalSurveyReportProcedure();
                ProcedureShortViewModels.Clear();
                foreach (var item in data)
                {
                    ProcedureShortViewModels.Add(item);
                }               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                OnAfterProcedureDataLoad();
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadSurveyReportMenuCommandAsync()
        {
            if (!InternetConnectHelper.CheckConnection())
            {
                UtilHelper.ShowToastMessage(AppConstant.NoInternetConnectMessage);
                return;
            }
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                ProfessionalSurveyReportMenuShortViews = new ObservableCollection<ProfessionalSurveyReportMobileMenu>();
                var data = await _restApiService.GetProfessionalSurveyReportMenus(ProcedureId);
                foreach (var item in data)
                {
                    ProfessionalSurveyReportMenuShortViews.Add(item);
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                OnAfterSurveyReportMenuDataLoad();
                IsBusy = false;
            }
        }
    }
}
