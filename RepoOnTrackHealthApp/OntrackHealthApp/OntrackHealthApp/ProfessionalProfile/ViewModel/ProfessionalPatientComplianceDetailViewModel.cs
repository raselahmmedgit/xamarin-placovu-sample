using OntrackHealthApp.ProfessionalProfile.Model;
using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalPatientComplianceDetailViewModel
    {
        public string PatientProfileLastLoginDateTime { get; set; }
        public CompliancePatientProfileModel PatientProfileViewModel { get; set; }

        public virtual CompliancePatientProcedureDetailModel PatientProcedureDetailViewModel { set; get; }

        public string PatientProfileAge
        {
            get
            {
                string patientAge = "";
                if (PatientProfileViewModel != null)
                {
                    if (PatientProfileViewModel.DateOfBirth.GetValueOrDefault() != null && PatientProfileViewModel.DateOfBirth.GetValueOrDefault().ToString().Trim().Length > 0)
                    {
                        DateTime currentDateTime = DateTime.UtcNow;
                        DateTime dateOfBirth = Convert.ToDateTime(PatientProfileViewModel.DateOfBirth);
                        try
                        {
                            int years = Convert.ToInt32(new DateTime(DateTime.UtcNow.Subtract(dateOfBirth).Ticks).Year - 1);
                            patientAge = years + " Years";
                        }
                        catch (Exception) { }
                    }
                }
                return patientAge;
            }
        }

        public bool IsSurgeryDateSinceDay { get; set; }

        public string SurgeryDateSinceDays
        {
            get
            {
                if (PatientProcedureDetailViewModel != null)
                {
                    string sinceDays = "";
                    if (PatientProcedureDetailViewModel.SurgeryDateTime != null)
                    {
                        DateTime currentDateTime = DateTime.UtcNow;

                        if (PatientProcedureDetailViewModel.SurgeryDateTime < currentDateTime)
                        {
                            DateTime surgeryDateTime = PatientProcedureDetailViewModel.SurgeryDateTime;

                            int totalDays = Convert.ToInt32((currentDateTime - surgeryDateTime).TotalDays);

                            sinceDays = totalDays + " Days";

                            IsSurgeryDateSinceDay = true;
                        }
                        else
                        {
                            sinceDays = "Surgery is pending.";

                            IsSurgeryDateSinceDay = false;
                        }
                    }
                    else
                    {
                    }
                    return sinceDays;
                }
                return "";
            }
        }

        public virtual IEnumerable<PatientSurveyActivityViewModel> PatientCompletedSurveyActivities { set; get; }

        public virtual IEnumerable<PatientSurveyActivityViewModel> PatientNotCompletedSurveyActivities { set; get; }

        public virtual IEnumerable<PatientSurveyActivityViewModel> PatientUpcomingSurveyActivities { set; get; }

        public virtual IEnumerable<PatientNoteViewModel> PatientNoteViewModels { set; get; }
        public virtual IEnumerable<ProfessionalPatientProfileComplianceDetailPainViewModel> ProfessionalPatientProfileComplianceDetailPainViewModels { set; get; }

        public virtual IEnumerable<SpForCriticalResultsOfPatientSurvey> CriticalPatientSurveyResults { set; get; }
    }
}
