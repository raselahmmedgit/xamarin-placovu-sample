using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergePacuViewModel
    {
        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public string PatientFullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public string PrimaryPhone { get; set; }

        public string OtherPhone { get; set; }

        public string EmailAddress { get; set; }

        public bool IsDeleted { get; set; }

        public string ProfilePictureId { get; set; }

        public string BirthMonth { get; set; }

        public string BirthDay { get; set; }

        public string BirthYear { get; set; }
        public string DateOfBirthString => BirthDay + "-" + BirthMonth + "-" + BirthYear + " 00:00:00";
        public string DateOfBirthStr { get; set; }

        public int PatientActiveProcedureCount { get; set; }

        public string ProfessionalName { get; set; }

        public string ProcedureName { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public string SurgeryTime { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public virtual IEnumerable<ScgPacuCommentViewModel> ScgPacuCommentViewModels { get; set; }
    }

    public class ScgPacuCommentViewModel
    {
        public long ScgPacuCommentId { get; set; }

        public string ScgPacuCommentTitle { get; set; }

        public string ScgPacuCommentText { get; set; }

        public string ScgPacuAdditionalComment { get; set; }

        public string ScgPacuCommentImageUrl { get; set; }

        public string ScgPacuCommentImageName { get; set; }

        public bool IsSelected { set; get; }

        public long PatientProfileId { get; set; }

        public string PatientProcedureDetailId { get; set; }

        public long ProcedureId { get; set; }

        public string ScgPacuCommentTextForSms { get; set; }

        public int DisplayOrder { get; set; }

        public bool HasFreeTextBox { get; set; }

        public string FreeTextBoxLabel { get; set; }

        public bool IsAdditionalComment { get; set; }
    }
}
