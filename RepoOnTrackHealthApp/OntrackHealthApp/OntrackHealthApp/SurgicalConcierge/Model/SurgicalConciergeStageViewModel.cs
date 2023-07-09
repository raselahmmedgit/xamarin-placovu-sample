using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergeStageViewModel
    {
        public long ScgStageProfessionalProcedureId { get; set; }

        public long? PracticeProfileId { get; set; }

        public string PracticeProfileName { get; set; }

        public long? PatientProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public long ProcedureId { get; set; }

        public long StageId { get; set; }

        public string ProfessionalName { get; set; }

        public string ProcedureName { get; set; }

        public string StageName { get; set; }

        public DateTime? SchuduleSurgeryDateTime { get; set; }

        public DateTime? ActualSurgeryDateTime { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? StartCSTDateTime { get; set; }

        public DateTime? EndCSTDateTime { get; set; }

        public bool HasStarted { get; set; }

        public bool HasEnded { get; set; }

        public bool IsActive { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public int DisplayOrder { get; set; }

        public Guid PatientProcedureDetailId { get; set; }
        public bool EndButtonVisibility
        {
            get
            {
                if (HasStarted && !HasEnded)
                    return true;
                return false;
            }
        }
        public bool StartButtonVisibility
        {
            get
            {
                if (!HasStarted && !HasEnded)
                    return true;
                return false;
            }
        }

        public string StartedTime
        {
            get
            {
                if (StartCSTDateTime != null)
                    return $"Started: {StartCSTDateTime.Value.ToString("hh:mm:ss tt")}";
                return string.Empty;
            }
        }
        public string EndedTime
        {
            get
            {
                if (EndCSTDateTime != null)
                    return $"Ended: {EndCSTDateTime.Value.ToString("hh:mm:ss tt")}";
                return string.Empty;
            }
        }
        public bool StartTimeVisibility
        {
            get
            {
                if (HasStarted || IsCompleted)
                    return true;
                return false;
            }
        }
        

    }

    
}
    
