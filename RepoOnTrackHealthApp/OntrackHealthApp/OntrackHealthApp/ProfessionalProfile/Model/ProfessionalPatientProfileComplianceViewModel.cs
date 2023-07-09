using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class ProfessionalPatientProfileComplianceViewModel
    {
        public long PatientProfileId { get; set; }

        public long ProcedureId { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string PatientName { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public DateTime? SurgeryDateTime { get; set; }

        public string ProcedureName { get; set; }

        public string CompletedStatusClass { get; set; }

        public string CompletedStatusName { get; set; }

        public string SurgeryMonth { get; set; }

        public string SurgeryYear { get; set; }

        public bool IsBphProcedure { get; set; }

        public string SurgeryTime
        {
            get
            {
                if (SurgeryDateTime != null)
                { return SurgeryDateTime.Value.ToString("hh:mm tt"); }
                return string.Empty;
            }
        }
    }

    public class ProfessionalPatientReportedOutcomeSearchViewModel
    {
        public int Id { get; set; }

        public string SurgeryMonthYear { get; set; }

        public string BackgroundColor { get; set; }

        public string BorderColor { get; set; }

        public List<ProfessionalPatientReportedOutcomeSearchDetailViewModel> ProfessionalPatientReportedOutcomeSearchDetailViewModels { get; set; }
    }

    public class ProfessionalPatientReportedOutcomeSearchDetailViewModel
    {
        public long PatientProfileId { get; set; }

        public long ProcedureId { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public string PatientName { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public DateTime? SurgeryDateTime { get; set; }

        public string ProcedureName { get; set; }

        public string CompletedStatusClass { get; set; }

        public string CompletedStatusName { get; set; }

        public string SurgeryMonth { get; set; }

        public string SurgeryYear { get; set; }

        public string SurgeryTime
        {
            get
            {
                if (SurgeryDateTime != null)
                { return SurgeryDateTime.Value.ToString("hh:mm tt"); }
                return string.Empty;
            }
        }

        public string PatientFullNameFormated
        {
            get
            {
                if (!string.IsNullOrEmpty(PatientName))
                {
                    return "Name : " + PatientName;
                }
                else if (!string.IsNullOrEmpty(PatientName))
                {
                    return "Name : " + PatientName;
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
                        return "Procedure : " + ProcedureName;
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
                { return "Surgery : " + SurgeryDate.Value.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }

        public bool IsBphProcedure { get; set; }

        public string IdFormated
        {
            get
            {
                if (PatientProcedureDetailId != null)
                { return (ProcedureId + "_" + PatientProfileId + "_" + PatientProcedureDetailId + "_" + IsBphProcedure); }
                return string.Empty;
            }
        }
    }
}
