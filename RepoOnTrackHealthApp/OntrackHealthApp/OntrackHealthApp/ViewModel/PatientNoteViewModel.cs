using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class PatientNoteViewModel
    {
        public long PatientNoteId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public long? ProcedureId { get; set; }

        public bool? IsBphProcedure { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public long PatientProfileId { get; set; }

        public string Notes { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormated
        {
            get
            {
                if (CreatedDate != null)
                { return CreatedDate.ToString("MM/dd/yyyy hh:mm tt"); }
                return string.Empty;
            }
        }
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}