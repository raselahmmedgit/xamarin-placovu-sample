using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientNotificationShortViewFromApi
    {
        /// <summary>
        /// NotificationId
        /// </summary>
        public long NoId { get; set; }
        /// <summary>
        /// PatientNotificationDetailId
        /// </summary>
        public long PndId { get; set; }
        /// <summary>
        /// NotificationDate
        /// </summary>
        public DateTime Nd { get; set; }
        /// <summary>
        /// NotificationStatusId
        /// </summary>
        public int NstId { set; get; }
        /// <summary>
        /// NotificationTitle
        /// </summary>
        public string Ntl { get; set; }
        /// <summary>
        /// NotificationShortTitle
        /// </summary>
        public string Nstl { get; set; }
        /// <summary>
        /// NotificationTypeId
        /// </summary>
        public int NtId { get; set; }
        /// <summary>
        /// NotificationOrder
        /// </summary>
        public int Nor { get; set; }
        /// <summary>
        /// NotificationHeader
        /// </summary>
        public string Nhd { get; set; }
        /// <summary>
        /// HasSurveyQuestions
        /// </summary>
        public bool Hsq { get; set; }
    }

    public class PatientNotificationShortView
    {
        public long NotificationId { get; set; }

        public long PatientNotificationDetailId { get; set; }

        public DateTime? NotificationSchedule { get; set; }

        public DateTime? NotificationDate { get; set; }

        public int NotificationMonth { get; set; }

        public string NotificationMonthName { get; set; }

        public string NotificationYearMonthName { get; set; }

        public int NotificationDay { get; set; }

        public int NotificationYear { get; set; }

        public int NotificationStatusId { set; get; }

        public string NotificationTitle { get; set; }

        public string NotificationShortTitle { get; set; }

        public int NotificationTypeId { get; set; }

        public int NotificationOrder { get; set; }

        public string NotificationDayBackgroundColor { get; set; }

        public string NotificationDayBackgroundImage { get; set; }
    }

    public class PatientNotificationShortViewListViewModel : List<PatientNotificationShortView>
    {
        public string GroupHeader { get; set; }
        public List<PatientNotificationShortView> Items => this;
    }

    public class PatientNotificationShortViewGroupedModel<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public PatientNotificationShortViewGroupedModel(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }

    public class CalendarMonthModel {
        public CalendarMonthModel()
        {
            PrimaryEvents = new List<CalendarEventModel>();
            SecondaryEvents = new List<CalendarEventModel>();
        }        
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public List<CalendarEventModel> PrimaryEvents { get; set; }
        public List<CalendarEventModel> SecondaryEvents { get; set; }
    }

    public class CalendarEventModel
    {
        public int OrderBy { get; set; }
        public long EventId { get; set; }
        public int EventDay { get; set; }
        public string MonthName { get; set; }
        public object EventDetailItem{ get; set; }
        public string EventTitle{ get; set; }
    }
}
