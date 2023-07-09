using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ValidationResult
    {
        public bool success;
        public bool EmailAddressAndPhoneNumberBothEmpty { get; set; }
        public bool IsEmailAddressInvalid { get; set; }
        public bool IsPhoneNumberInvalid { get; set; }
        public string message;
    }
}
