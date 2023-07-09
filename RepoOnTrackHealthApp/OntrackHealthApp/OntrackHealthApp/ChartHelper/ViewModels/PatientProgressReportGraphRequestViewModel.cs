using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ChartHelper.ViewModels
{
    public class PatientProgressReportGraphRequestViewModel
    {
        public long GraphId { set; get; }
        public bool IsPatientProgressReport { set; get; }
        public bool IsPatientSelected { set; get; }
        public bool IsPhysicianPatientSelected { set; get; }
        public Guid PatientProcedureDetailId { set; get; }
        public long PatientProfileId { set; get; }
        public string chartType { set; get; }

    }
}
