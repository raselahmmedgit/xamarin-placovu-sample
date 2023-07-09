namespace OntrackHealthApp.ApiService.Model
{
    using System;

    public partial class PatientShortProfileView
    {

        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public string PrimaryPhone { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public int? PrimaryPhoneCountryId { get; set; }

        public string OtherPhone { get; set; }

        public string OtherPhoneCode { get; set; }

        public int? OtherPhoneCountryId { get; set; }

        public string EmailAddress { get; set; }

        public string AppUserId { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string DateOfBirthStr { get; set; }

        public string ProfilePictureId { get; set; }

        public string PatientPassword { get; set; }

        public bool? RemoveFromGDB { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsArchived { get; set; }

        public bool IsDeleted { get; set; }

        public bool EnableSmsNotification { get; set; }

        public bool IsFromScgPatientEntry { get; set; }

        public bool EnablePathologySmsNotification { get; set; }

        public bool IsDeactivated { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
