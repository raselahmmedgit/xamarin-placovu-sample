using OntrackHealthApp.ApiService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeNursePatientSurveyViewModel
    {
        public ReportTabularData<WeekOneTwoPostOpGraphReport> PatientSurvey { get; set; }
        public PatientProfileWithProfessionalProcedureView PatientProcedureDetailInfo { get; set; }
        public List<ProfessionaNotesToNurse> ProfessionaNotesToNurseItems { get; set; }

    }
}
