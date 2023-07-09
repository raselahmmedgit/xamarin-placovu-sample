using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class PatientDischargeNotificationModel
    {
        public string ProcedureName { get; set; }
        public string PatientProcedureDetailId { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public IEnumerable<PatientSurveyPatientNotificationDetailViewModel> PatientSurveyPatientNotificationDetailViewModels { get; set; }
        public List<SurgicalResourcePatientProstatectomyLibraryPageViewModel> SurgicalResourcePatientProstatectomyLibraryPageViewModelList { get; set; }
    }
}
