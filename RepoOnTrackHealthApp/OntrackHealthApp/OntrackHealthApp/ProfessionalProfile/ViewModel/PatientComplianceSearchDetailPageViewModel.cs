using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientComplianceSearchDetailPageViewModel
    {
        public string PatientFullName { get; set; }

        public string PatientFullNameFormated
        {
            get
            {
                if (!string.IsNullOrEmpty(PatientFullName))
                {
                    return "Name : " + PatientFullName;
                }

                return string.Empty;
            }
        }

        public string ProcedureName { get; set; }

        public string ProcedureNameFormated
        {
            get
            {
                if (ProcedureName != null)
                {
                    if (!string.IsNullOrEmpty(ProcedureName))
                    {
                        return "Procedure : " + ProcedureName;
                    }
                }

                return string.Empty;
            }
        }

        public DateTime SurgeryDateTime { get; set; }

        public string SurgeryDateTimeFormated
        {
            get
            {
                if (SurgeryDateTime != null)
                { return "Surgery : " + SurgeryDateTime.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }
        public string SurgeryDateFormated
        {
            get
            {
                if (SurgeryDateTime != null)
                { return "Surgery : " + SurgeryDateTime.ToString("MM/dd/yyyy"); }
                return string.Empty;
            }
        }

        public DateTime? NotificationDate { get; set; }

        public string NotificationDateFormated
        {
            get
            {
                if (NotificationDate != null)
                { return "Notification : " + NotificationDate.Value.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }

        public DateTime DateOfBirth { get; set; }

        public string DateOfBirthFormated
        {
            get
            {
                if (DateOfBirth != null)
                { return "Date Of Birth : " + DateOfBirth.ToString("MM/dd/yyyy - hh:mm tt"); }
                return string.Empty;
            }
        }
        public string DateOfBirthShortFormated
        {
            get
            {
                if (DateOfBirth != null)
                { return DateOfBirth.ToString("MM/dd/yyyy"); }
                return string.Empty;
            }
        }
        public string LocationName { get; set; }

        public string LocationNameFormated
        {
            get
            {
                if (LocationName != null)
                {
                    if (!string.IsNullOrEmpty(LocationName))
                    {
                        return "Location : " + LocationName;
                    }
                }

                return string.Empty;
            }
        }
    }
}
