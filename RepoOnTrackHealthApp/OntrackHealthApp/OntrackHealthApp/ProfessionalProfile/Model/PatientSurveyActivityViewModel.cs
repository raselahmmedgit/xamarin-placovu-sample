using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class PatientSurveyActivityViewModel
    {
        public long ProcedureId { get; set; }
        public string ProcedureName { get; set; }

        public long NotificationId { get; set; }
        public string NotificationTitle { get; set; }
    }
}
