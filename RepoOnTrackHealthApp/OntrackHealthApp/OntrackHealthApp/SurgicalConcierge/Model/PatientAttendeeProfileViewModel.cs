using OntrackHealthApp.ApiHelper.Extensions;
using OntrackHealthApp.SurgicalConcierge.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PatientAttendeeProfileViewModel
    {
        public PatientAttendeeProfileViewModel()
        {
            this.CountryViewModels = new List<CountryViewModel>();
        }

        public long AttendeeProfileId { get; set; }

        //[Display(Name = "First Name")]
        //[StringLength(250)]
        public string FirstName { get; set; }

        //[Display(Name = "Last Name")]
        //[StringLength(250)]
        public string LastName { get; set; }

        //[Display(Name = "Preferred Name")]
        //[StringLength(250)]
        public string PreferredName { get; set; }

        //[Display(Name = "Mobile Phone Code")]
        //[StringLength(10)]
        public string PrimaryPhoneCode { get; set; }

        //[Required]
        //[Display(Name = "Mobile Phone")]
        //[RegularExpression(@"1?\W*([2-9][0-8][0-9])\W*([2-9][0-9]{2})\W*([0-9]{4})(\se?x?t?(\d*))?", ErrorMessage = "The Primary Phone field is not a valid phone number.")]
        //[StringLength(50)]
        public string PrimaryPhone { get; set; }

        public int? PrimaryPhoneCountryId { get; set; }

        public string PrimaryPhoneCountryIso { get; set; }

        //[Display(Name = "Phone Code")]
        //[StringLength(10)]
        //public string OtherPhoneCode { get; set; }

        //[StringLength(50)]
        ////[RegularExpression(@"1?\W*([2-9][0-8][0-9])\W*([2-9][0-9]{2})\W*([0-9]{4})(\se?x?t?(\d*))?", ErrorMessage = "The Other Phone field is not a valid phone number.")]
        //[Display(Name = "Other Phone")]
        //public string OtherPhone { get; set; }

        //public int? OtherPhoneCountryId { get; set; }

        //public string OtherPhoneCountryIso { get; set; }

        //[Required]
        //[StringLength(250)]
        //[EmailAddress]
        //[Display(Name = "Email Address")]
        //Source: https://www.w3.org/TR/2012/WD-html-markup-20120320/input.email.html#input.email.attrs.value.single
        //[RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        public string EmailAddress { get; set; }

        public bool IsDeleted { get; set; }

        //[Display(Name = "Email send allowed")]
        public bool EmailAllowed { get; set; }

        //[Display(Name = "SMS send allowed")]
        public bool SmsAllowed { get; set; }

        public long PatientProfileId { get; set; }

        public virtual CountryViewModel PrimaryPhoneCountryViewModel { get; set; }

        public virtual CountryViewModel OtherPhoneCountryViewModel { get; set; }

        //public virtual PatientProfileViewModel PatientProfileViewModel { get; set; }

        public virtual IEnumerable<CountryViewModel> CountryViewModels { get; set; }
        public string EmailStatus
        {
            get
            {
                if (EmailAllowed)
                    return "Enabled";
                return "Disabled";
            }
        }
        public string SmsStatus
        {
            get
            {
                if (SmsAllowed)
                    return "Enabled";
                return "Disabled";
            }
        }
        public string MobilePhoneWithCountryCode
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

        public string MobilePhoneWithCountryCodeFormatted
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

        public string EmailAddressFormatted
        {
            get
            {
                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    return "Email: " + EmailAddress;
                }
                return "Email: ";
            }
        }

        public bool IsEmailProvided
        {
            get
            {
                return EmailAddress != null;
            }
        }
        public bool IsPhoneProvided
        {
            get
            {
                return PrimaryPhone != null;
            }
        }

        public int? AttendeeProfileTypeId { get; set; }

        public string AttendeeProfileTypeName { get; set; }

        public virtual PatientAttendeeProfileTypeViewModel PatientAttendeeProfileTypeViewModel { get; set; }
    }
}
