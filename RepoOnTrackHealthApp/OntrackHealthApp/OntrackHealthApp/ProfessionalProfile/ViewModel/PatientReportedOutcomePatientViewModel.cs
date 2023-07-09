using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientReportedOutcomePatientViewModel
    {
        public long PracticeProfileId { get; set; }

        public long ProcedureId { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public string ProcedureName { get; set; }

        public long PatientProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public DateTime? NotificationDate { get; set; }

        public DateTime SurgeryDate { get; set; }

        public int PatientProcedureStatusId { get; set; }

        public bool IsBphProcedure { get; set; }

        public bool HasPainLevelAboveTen { get; set; }

        public bool IsServeySubmited { get; set; }

        public int TotalRow { get; set; }

        public bool IsLinkVisitedByProfessional { get; set; }

        public string PatientPreferredName { get; set; }

        public int TotalRecord { get; set; }

        public int SurgeryDateYearMonth { get; set; }

        public int SurgeryYear { get; set; }

        public string SurgeryMonthName { get; set; }

        public long RowNumber { get; set; }

        public string ProfessionalProfileName { get; set; }

        public string PatientName { get; set; }

        public string SurgeryTime { get; set; }

        public DateTime? SurgeryDateTime => Convert.ToDateTime(SurgeryDate.ToString("MM/dd/yyyy") + " " + SurgeryTime);

        public DateTime LatestNotificationDate { get; set; }

        public string NotificationMonth { get; set; }

        public string NotificationYear { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public int IsViewByUser { get; set; }

        public string NotificationMonthName { get; set; }

        public string PatientFullName { get; set; }

        public string PatientFullNameFormated
        {
            get
            {
                if (!string.IsNullOrEmpty(PatientFullName))
                {
                    return PatientFullName;
                }
                else if (!string.IsNullOrEmpty(PatientName))
                {
                    return PatientName;
                }
                else if (!string.IsNullOrEmpty(PatientPreferredName))
                {
                    return PatientPreferredName;
                }

                return string.Empty;
            }
        }

        public string ProcedureNameFormated
        {
            get
            {
                if (ProcedureName != null)
                {
                    if (!string.IsNullOrEmpty(ProcedureName))
                    {
                        return ProcedureName;
                    }
                }

                return string.Empty;
            }
        }

        public string SurgeryDateTimeFormated
        {
            get
            {
                if (SurgeryDate != null)
                { return SurgeryDate.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }

        public string NotificationDateFormated
        {
            get
            {
                if (NotificationDate != null)
                { return NotificationDate.Value.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }

        public string ProfessionalLinkVisitStatus
        {
            get
            {
                if (IsLinkVisitedByProfessional)
                {
                    return "patient_outcome_tick.png";
                }
                else
                {
                    return "patient_outcome_minus.png"; ;
                }
            }
        }

    }
}
