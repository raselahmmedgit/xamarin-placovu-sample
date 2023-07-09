using OntrackHealthApp.ApiHelper.Extensions;
using System;
using System.Collections.Generic;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConciergePatientViewModel
    {
        public SurgicalConciergePatientViewModel()
        {
            this.PracticeProfileDropDownViewModel = new PracticeProfileDropDownViewModel();
            this.ProfessionalProfileDropDownViewModel = new ProfessionalProfileDropDownViewModel();
            ProfessionalProcedureDropDownViewModel = new ProfessionalProcedureDropDownViewModel
            {
                SelectOptions = new List<SelectListItem>()
            };
            ProfessionalProcedureLocationDropDownViewModel = new PracticeProcedureLocationDropDownViewModel
            {
                PracticeLocationId = new Guid(),
                SelectOptions = new List<SelectListItem>()
            };
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

        public string PrimaryPhoneWithCountryCode
        {
            get
            {
                if (!string.IsNullOrEmpty(PrimaryPhone) && PrimaryPhone != "")
                {
                    return PrimaryPhoneCode + " " + PrimaryPhone.ToFormatedPhoneNumber();
                }
                return string.Empty;
            }
        }

        public string PrimaryPhoneWithCountryCodeFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(PrimaryPhone) && PrimaryPhone != "")
                {
                    return "Phone: " + PrimaryPhoneCode + " " + PrimaryPhone.ToFormatedPhoneNumber();
                }
                return "Phone: ";
            }
        }

        public string EmailAddress { get; set; }

        public string DateOfBirthStr { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? SurgeryDate { get; set; }

        public string SurgeryDateString { get; set; }

        public string SurgeryDateInString { get; set; }

        public DateTime? SurgeryDateTime { get; set; }

        public string SurgeryTime { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public Guid? PracticeLocationId { get; set; }

        public string PreferredName { get; set; }

        public string PatientFullName 
        {
            get
            {
                if (!string.IsNullOrEmpty(PreferredName))
                {
                    return PreferredName;
                }
                else if (!string.IsNullOrEmpty(PatientName))
                {
                    return PatientName;
                }
                else if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                {
                    return FirstName?.Trim() + " " + LastName?.Trim();
                }

                return string.Empty;
            }
        }

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

        public string SurgeryDateFormated
        {
            get
            {
                if (SurgeryDateTime != null)
                    return SurgeryDateTime.Value.ToString("MM/dd/yyyy");
                return string.Empty;
            }
        }

        public string SurgeryTimeFormated
        {
            get
            {
                if (SurgeryDateTime != null)
                    return SurgeryDateTime.Value.ToString("hh:mm tt");
                return string.Empty;
            }
        }

        public string SurgicalConceirgeRoom { get; set; }

        public int TotalRecord { get; set; }

        public bool IsSecondCommandVisible { get; set; }

        public bool IsFirstCommandVisible { get; set; }
        public bool EnablePathologySmsNotification { set; get; }
        public bool IsPatientContactProvided
        {
            get
            {
                return !string.IsNullOrEmpty(EmailAddress) || !string.IsNullOrEmpty(PrimaryPhone);
            }
        }

        public long? PatientDivisionId { get; set; }

        public PracticeProfileDropDownViewModel PracticeProfileDropDownViewModel { get; set; }
        public ProfessionalProfileDropDownViewModel ProfessionalProfileDropDownViewModel { get; set; }
        public ProfessionalProcedureDropDownViewModel ProfessionalProcedureDropDownViewModel { get; set; }
        public PracticeProcedureLocationDropDownViewModel ProfessionalProcedureLocationDropDownViewModel { get; set; }
        public IEnumerable<PatientAttendeeProfileViewModel> PatientAttendeeProfileViewModels { get; set; }
        public SurgicalConciergePatientViewModel SelectedSurgicalConciergePatientViewModel { get; set; }
        public IEnumerable<PracticeScgDivisionView> Departments { get; set; }
        public string ItemBackgroundColor { get; set; }
    }

    public class PracticeScgDivisionView
    {
        public Guid Id { get; set; }
        public long PracticeProfileId { get; set; }
        public long ScgDivisionId { get; set; }
        public string DivisionName { get; set; }
        public bool IsDefault { set; get; }
        public int ProcedureGenderTypeId { set; get; }
        public bool IsPatientDivision { get; set; }
    }
}
