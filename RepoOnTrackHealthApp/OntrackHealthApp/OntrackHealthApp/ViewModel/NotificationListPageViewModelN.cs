using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.ApiHelper.Client;
using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.ApiService.Client;
using OntrackHealthApp.ApiService.ViewModel;
using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OntrackHealthApp.ViewModel
{
    public class NotificationListPageViewModelN : ViewModelBase
    {
        public ObservableCollection<PatientNotificationShortView> PatientNotificationShortViews { get; set; }
        private List<PatientNotificationShortViewListViewModel> _patientNotificationShortViewListViewModelList;
        public List<PatientNotificationShortViewListViewModel> PatientNotificationShortViewListViewModelList { get { return _patientNotificationShortViewListViewModelList; } set { _patientNotificationShortViewListViewModelList = value; base.OnPropertyChanged(); } }
        public List<CalendarMonthModel> CalendarMonthModels { get; set; }
        public Command LoadPatientNotificationCommand { get; set; }
        private readonly ITokenContainer _iTokenContainer;
        private readonly IScheduleClient _iScheduleClient;

        public NotificationListPageViewModelN()
        {
            PatientNotificationShortViews = new ObservableCollection<PatientNotificationShortView>();
            CalendarMonthModels = new List<CalendarMonthModel>();
            _iTokenContainer = new TokenContainer();
            var apiClient = new ApiClient(HttpClientInstance.Instance, _iTokenContainer);
            _iScheduleClient = new ScheduleClient(apiClient);
            LoadPatientNotificationCommand = new Command( async ()=> { await ExecuteLoadPatientNotificationCommandAsync(); });
        }

        public async Task ExecuteLoadPatientNotificationCommandAsync()
        {
            if (IsBusy) { return; }
            if (PatientNotificationShortViews.Count > 0) { return; }
            App.ShowUserDialogAsync();
            try
            {
                IsBusy = true;
                //PatientNotificationShortViews = new ObservableCollection<PatientNotificationShortView>();
                //CalendarMonthModels = new List<CalendarMonthModel>();
                List<DistinctYearMonth> DistinctYearMonths = new List<DistinctYearMonth>();

                var response = await _iScheduleClient.GetPatientSchedulesUpToDate(_iTokenContainer.CurrentPatientProcedureDetailId);
                
                if (response.StatusIsSuccessful)
                {
                    var data = response.DataList;

                    if (data != null)
                    {                        
                        foreach (PatientNotificationShortViewFromApi item in data)
                        {
                            var pnsv = new PatientNotificationShortView
                            {
                                NotificationId = item.NoId,
                                NotificationOrder = item.Nor,
                                NotificationDate = item.Nd,
                                NotificationShortTitle = item.Nstl,
                                NotificationStatusId = item.NstId,
                                NotificationTitle = item.Ntl,
                                NotificationTypeId = item.NtId,
                                PatientNotificationDetailId = item.PndId,
                                NotificationMonthName = item.Nd.ToString("MMM").ToUpper(),
                                NotificationYearMonthName = item.Nd.ToString("MMM").ToUpper() + " " + item.Nd.Year.ToString(),
                                NotificationMonth = item.Nd.Month,
                                NotificationDay = item.Nd.Day,
                                NotificationYear = item.Nd.Year
                            };
                            PatientNotificationShortViews.Add(pnsv);
                            if (DistinctYearMonths.Any(x => x.Year == pnsv.NotificationYear && x.Month == pnsv.NotificationMonth) == false)
                            {
                                DistinctYearMonths.Add(new DistinctYearMonth { Month = pnsv.NotificationMonth, Year = pnsv.NotificationYear, MonthName = pnsv.NotificationMonthName });
                            }
                        }                        

                        foreach(var item in DistinctYearMonths)
                        {
                            var md = new CalendarMonthModel();
                            md.Month = item.Month;
                            md.Year = item.Year;
                            md.MonthName = item.MonthName;
                            var daysData =  PatientNotificationShortViews.Where(x => x.NotificationMonth == item.Month && x.NotificationYear == item.Year).ToList();
                            int orderBy = 0;
                            foreach (var day in daysData)
                            {
                                if( day.NotificationId == Enums.SurgeryDatePatientNotificationEnum.NotificationId.ToInt())
                                {
                                    md.SecondaryEvents.Add(new CalendarEventModel { EventDay = day.NotificationDay, EventId = day.NotificationId, OrderBy = orderBy, EventTitle = day.NotificationTitle, MonthName = day.NotificationMonthName });
                                }
                                else
                                {
                                    md.PrimaryEvents.Add(new CalendarEventModel { EventDay = day.NotificationDay, EventId = day.NotificationId, OrderBy = orderBy, EventTitle = day.NotificationTitle, MonthName = day.NotificationMonthName });
                                }
                                
                                orderBy++;
                            }
                            CalendarMonthModels.Add(md);
                        }


                        #region Group

                        List<PatientNotificationShortViewListViewModel> patientNotificationShortViewListViewModelList = new List<PatientNotificationShortViewListViewModel>();

                        var patientNotificationShortViewGroupByList = PatientNotificationShortViews.GroupBy(item => item.NotificationYearMonthName).ToList();

                        foreach (var patientNotificationShortViewGroupBy in patientNotificationShortViewGroupByList)
                        {
                            PatientNotificationShortViewListViewModel patientNotificationShortViewListViewModel = new PatientNotificationShortViewListViewModel();

                            string scheduleMonthName = patientNotificationShortViewGroupBy.Key.ToString();

                            patientNotificationShortViewListViewModel.GroupHeader = scheduleMonthName;

                            foreach (var patientNotificationShortView in patientNotificationShortViewGroupBy)
                            {
                                if (patientNotificationShortView.NotificationId == 999999)
                                {
                                    patientNotificationShortView.NotificationDayBackgroundColor = "#f7a50f";
                                    patientNotificationShortView.NotificationDayBackgroundImage = "sun_yellow_fill.png";
                                }
                                else
                                {
                                   patientNotificationShortView.NotificationDayBackgroundColor = "#00A79B";
                                    patientNotificationShortView.NotificationDayBackgroundImage = "rounded_list_icon.png";
                                }

                                patientNotificationShortViewListViewModel.Add(patientNotificationShortView);

                            } // inner group by


                            patientNotificationShortViewListViewModelList.Add(patientNotificationShortViewListViewModel);
                        }

                        PatientNotificationShortViewListViewModelList = patientNotificationShortViewListViewModelList;

                        #endregion

                        App.HideUserDialogAsync();
                    }
                }
                else
                {
                    IsSuccess = false;
                    ErrorMessage = AppConstant.ErrorCommon;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                ErrorMessage = AppConstant.ErrorCommon;
                App.HideUserDialogAsync();
                var d = ex;
            }
            finally
            {
                IsBusy = false;               
            }
        }


        internal class DistinctYearMonth {
            public int Month { get; set; }
            public int Year { get; set; }
            public string MonthName { get; set; }
        }
    }    
}
