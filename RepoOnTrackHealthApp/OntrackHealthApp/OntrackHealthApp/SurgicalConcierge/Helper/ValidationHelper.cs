using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Helper
{
    public class ValidationHelper
    {
        public static ValidationResult ValidateEmailAddress(string emailAddress)
        {
            ValidationResult validationResult = new ValidationResult();
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                validationResult.success = true;
                validationResult.message = "";
            }
            catch (Exception)
            {
                validationResult.success = false;
                validationResult.message = AppConstants.EmailErrorMessage;
            }
            return validationResult;
        }

        public static ValidationResult ValidateEmailAddressMti(string emailAddress)
        {
            ValidationResult validationResult = new ValidationResult();

            if (emailAddress == null || emailAddress.Length <= 0)
            {
                validationResult.success = true;
                validationResult.message = "";
                return validationResult;
            }

            try
            {
                MailAddress m = new MailAddress(emailAddress);
                validationResult.success = true;
                validationResult.message = "";
            }
            catch (Exception)
            {
                validationResult.success = false;
                validationResult.message = AppConstants.EmailErrorMessage;
            }
            return validationResult;
        }

        public static ValidationResult ValidatePhoneNumber(string phoneNumber)
        {
            ValidationResult validationResult = new ValidationResult();
            if ((phoneNumber == null || phoneNumber.Length <= 0) || (phoneNumber != null && phoneNumber.Length > 12))
            {
                validationResult.success = false;
                validationResult.message = AppConstants.PhoneNumberErrorMessage;
            }
            else
            {
                validationResult.success = true;
                validationResult.message = "";
            }
            return validationResult;
        }

        public static ValidationResult ValidatePhoneNumber(string phoneNumber, bool smsSendAllowed)
        {
            ValidationResult validationResult = new ValidationResult();
            if ((smsSendAllowed && (phoneNumber == null || phoneNumber.Length <= 0)) || (phoneNumber != null && phoneNumber.Length > 50))
            {
                validationResult.success = false;
                validationResult.message = AppConstants.PhoneNumberErrorMessage;
            }
            else
            {
                validationResult.success = true;
                validationResult.message = "";
            }
            return validationResult;
        }

        public static ValidationResult ValidatePhoneNumberMti(string phoneNumber, bool smsSendAllowed, bool required = false)
        {
            ValidationResult validationResult = new ValidationResult();

            if (smsSendAllowed && (phoneNumber == null || phoneNumber.Length <= 0))
            {
                validationResult.success = true;
                if (required)
                {
                    validationResult.message = AppConstants.PhoneNumberRequiredMessage;
                    validationResult.success = false;
                }
                else
                {
                    validationResult.message = "";
                }
                return validationResult;
            }
            else if (smsSendAllowed && phoneNumber.Length != 12)
            {
                validationResult.success = false;
                validationResult.message = AppConstants.PhoneNumberErrorMessage;
                return validationResult;
            }
            else
            {
                phoneNumber = phoneNumber.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
                foreach (char c in phoneNumber)
                {
                    if (c < '0' || c > '9')
                    {
                        validationResult.success = false;
                        validationResult.message = AppConstants.PhoneNumberErrorMessage;
                        return validationResult;
                    }
                }
            }

            validationResult.success = true;
            validationResult.message = "";
            return validationResult;
        }

        public static ValidationResult ValidatePhoneNumberMti(string phoneNumber, bool required = false)
        {
            ValidationResult validationResult = new ValidationResult();

            if (phoneNumber == null || phoneNumber.Length <= 0)
            {
                validationResult.success = true;
                if (required)
                {
                    validationResult.message = AppConstants.PhoneNumberRequiredMessage;
                    validationResult.success = false;
                }
                else
                {
                    validationResult.message = "";
                }
                return validationResult;
            }
            else if (phoneNumber.Length != 12)
            {
                validationResult.success = false;
                validationResult.message = AppConstants.PhoneNumberErrorMessage;
                return validationResult;
            }
            else
            {
                phoneNumber = phoneNumber.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
                foreach (char c in phoneNumber)
                {
                    if (c < '0' || c > '9')
                    {
                        validationResult.success = false;
                        validationResult.message = AppConstants.PhoneNumberErrorMessage;
                        return validationResult;
                    }
                }
            }

            validationResult.success = true;
            validationResult.message = "";
            return validationResult;
        }
       
        public static ValidationResult ValidateEmailAddressAndPhoneNumber(string emailAddress, string phoneNumber)
        {
            ValidationResult validationResult = new ValidationResult();

            if ((emailAddress == null || emailAddress.Length <= 0) && (phoneNumber == null || phoneNumber.Length <= 0))
            {
                validationResult.success = false;
                validationResult.message = AppConstants.AtLeastOneErrorMessage;
                return validationResult;
            }

            if ((emailAddress != null && emailAddress.Length > 0))
            {
                try
                {
                    MailAddress m = new MailAddress(emailAddress);
                    validationResult.success = true;
                    validationResult.message = "";
                }
                catch (Exception)
                {
                    validationResult.success = false;
                    validationResult.message = AppConstants.EmailErrorMessage;
                }
                return validationResult;

            }
            else if ((phoneNumber == null || phoneNumber.Length <= 0) || (phoneNumber != null && phoneNumber.Length > 12))
            {
                ValidationResult validationResultPhone = new ValidationResult();
                validationResultPhone = ValidatePhoneNumberMti(phoneNumber);
                if (validationResultPhone.success == false)
                {
                    validationResult.success = false;
                    validationResult.message = validationResultPhone.message;
                    validationResult.IsPhoneNumberInvalid = true;
                    return validationResult;
                }
            }

            validationResult.success = true;
            validationResult.message = "";
            return validationResult;
        }

        public static ValidationResult ValidateEmailAddressOrPhoneNumber(string emailAddress, string phoneNumber)
        {
            ValidationResult validationResult = new ValidationResult();
            bool isEmailEmpty = false;
            bool isPhoneEmpty = false;
            validationResult.success = true;
            validationResult.message = "";

            if ((emailAddress == null || emailAddress.Length <= 0))
            {
                isEmailEmpty = true;
            }
            if ((phoneNumber == null || phoneNumber.Length <= 0) || (phoneNumber != null && phoneNumber.Length > 12))
            {
                isPhoneEmpty = true;
            }

            if (isEmailEmpty == true && isPhoneEmpty == true)
            {
                validationResult.success = false;
                validationResult.EmailAddressAndPhoneNumberBothEmpty = true;
                validationResult.message = AppConstants.EmailAddressOrPhoneNumberRequired;
                return validationResult;
            }
            if (isEmailEmpty == false)
            {
                ValidationResult validationResultEmail = new ValidationResult();
                validationResultEmail = ValidateEmailAddress(emailAddress);
                if (validationResultEmail.success == false)
                {
                    validationResult.success = false;
                    validationResult.message = validationResultEmail.message;
                    validationResult.IsEmailAddressInvalid = true;
                    return validationResult;
                }
            }
            if (isPhoneEmpty == false)
            {
                ValidationResult validationResultPhone = new ValidationResult();
                validationResultPhone = ValidatePhoneNumberMti(phoneNumber);
                if (validationResultPhone.success == false)
                {
                    validationResult.success = false;
                    validationResult.message = validationResultPhone.message;
                    validationResult.IsPhoneNumberInvalid = true;
                    return validationResult;
                }
            }
            return validationResult;
        }

        public static ValidationResult ValidateEmailAddressForAttendee(string emailAddress)
        {
            ValidationResult validationResult = new ValidationResult();
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                validationResult.success = true;
                validationResult.message = "";
            }
            catch (Exception)
            {
                validationResult.success = false;
                validationResult.message = AppConstants.EmailErrorMessage;
            }
            return validationResult;
        }

        public static ValidationResult ValidatePhoneNumberForAttendee(string phoneNumber)
        {
            ValidationResult validationResult = new ValidationResult();
            if ((phoneNumber != null && phoneNumber.Length > 12))
            {
                validationResult.success = false;
                validationResult.message = AppConstants.PhoneNumberErrorMessage;
            }
            else
            {
                validationResult.success = true;
                validationResult.message = "";
            }
            return validationResult;
        }
    }
}
