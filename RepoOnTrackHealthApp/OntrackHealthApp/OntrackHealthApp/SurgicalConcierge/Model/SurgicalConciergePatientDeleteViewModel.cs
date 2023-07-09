using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergePatientDeleteViewModel
    {
        public SurgicalConciergePatientDeleteViewModel()
        {
            PatientProcedureDetailId = Guid.NewGuid();
        }

        public long AutoId { get; set; }

        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeName { get; set; }

        public long? ProfessionalProfileId { get; set; }

        public string ProfessionalName { get; set; }

        public long? ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PatientName { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public string PrimaryPhone { get; set; }

        public string EmailAddress { get; set; }

        public string DateOfBirthStr { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? SurgeryDate { get; set; }
        public string SurgeryDateInString { get; set; }

        public DateTime? SurgeryDateTime { get; set; }

        public string SurgeryTime { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public string PatientFullName => FirstName + " " + LastName;

        public string DateOfBirthFormated
        {
            get
            {
                if (string.IsNullOrEmpty(DateOfBirthStr))
                {
                    return string.Empty;
                }
                return DateTime.Parse(DateOfBirthStr).ToString("MM/dd/yyyy");
            }
        }

        public string SurgeryDateTimeFormated
        {
            get
            {
                if (SurgeryDateTime != null)
                    return SurgeryDateTime.Value.ToString("MM/dd/yyyy - hh:mm tt");
                return string.Empty;
            }
        }
    }
}
