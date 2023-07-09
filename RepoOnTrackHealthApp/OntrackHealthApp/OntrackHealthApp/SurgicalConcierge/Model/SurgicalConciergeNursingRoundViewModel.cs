using System;
using System.Collections.Generic;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeNursingRoundViewModel
    {
        public SurgicalConciergeNursingRoundViewModel()
        {
            CountryViewModels = new List<CountryViewModel>();
            ScgNursingRoundTemplateCategoryApiViewModels = new List<ScgNursingRoundTemplateCategoryApiViewModel>();
        }
        public string PracticeLocationName { get; set; }
        public Guid? PracticeLocationId { get; set; }
        public Guid? PatientProcedureDetailId { get; set; }
        public long PatientProfileId { get; set; }
        public string SalutationMessage { get; set; }
        public string ProgressUpdateMessage { get; set; }
        public string TodaysPlanMessage { get; set; }
        public string FloorPhone { get; set; }
        public string CustomMessage { get; set; }
        public string FloorPhoneCode { set; get; }
        public string HeaderTitle { set; get; }

        public virtual List<CountryViewModel> CountryViewModels { set; get; }
        public virtual List<ScgNursingRoundTemplateCategoryApiViewModel> ScgNursingRoundTemplateCategoryApiViewModels { get; set; }
    }
}
