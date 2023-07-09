using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientScheduleHomePageViewModel
    {
        public string ProcedureName { get; set; }

        public string PatientProcedureDetailId { get; set; }

        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public IEnumerable<PatientSurveyPatientNotificationDetailViewModel> PatientSurveyPatientNotificationDetailViewModels { get; set; }
    }
}
