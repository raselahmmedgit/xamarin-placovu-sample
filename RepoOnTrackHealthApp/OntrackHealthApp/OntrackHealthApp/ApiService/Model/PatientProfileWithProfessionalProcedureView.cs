using OntrackHealthApp.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class PatientProfileWithProfessionalProcedureView : ApiModel
    {
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

        public DateTime? SurgeryDateTime { get; set; }

        public string SurgeryTime { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public string PatientFullName => FirstName + " " + LastName;
    }
}
