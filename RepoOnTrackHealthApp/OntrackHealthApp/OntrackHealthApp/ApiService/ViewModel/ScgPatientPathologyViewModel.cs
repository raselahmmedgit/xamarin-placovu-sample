using OntrackHealthApp.ApiHelper.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ScgPatientPathologyViewModel : ApiModel
    {
        public ScgPatientPathologyViewModel()
        {
            ScgPatientPathologyId = Guid.NewGuid();
        }
        public Guid ScgPatientPathologyId { get; set; }

        public long PatientProfileId { get; set; }

        public long PracticeProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public long ProcedureId { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public string PreopPsa { get; set; }

        public string GleasonScore { get; set; }

        public int GleasonScoreOne { get; set; }

        public int GleasonScoreTwo { get; set; }

        public int GleasonScoreSum
        {
            get
            {
                var retVal = 0;
                if (!string.IsNullOrEmpty(GleasonScore))
                {
                    if (GleasonScore.Length == 1)
                        retVal = Convert.ToInt32( GleasonScore.Trim());

                    if (GleasonScore.Length > 1 && GleasonScore.Contains("+"))
                    {
                        var data = GleasonScore.Split('+');

                        if (data.Length >= 2)
                        {
                            retVal += Convert.ToInt32(data[0].Trim());
                            retVal += Convert.ToInt32(data[1].Trim());
                        }
                    }
                    if (GleasonScore.Length > 1 && GleasonScore.Contains(","))
                    {
                        var data = GleasonScore.Split(',');
                        retVal = retVal + Convert.ToInt32(data[0].Trim()) + Convert.ToInt32(data[1].Trim()) + Convert.ToInt32(data[2].Trim());
                    }
                }
                return retVal;
            }
        }

        public string StageScore { get; set; }

        public string MarginScore { get; set; }

        public string NodeStatus { get; set; }

        public string Metastasis { get; set; }

        public decimal? CancerInvolvement { get; set; }

        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthStr { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public string PrimaryPhone { get; set; }

        public string EmailAddress { get; set; }

        public string ProfessionalName { get; set; }

        public string ProcedureName { get; set; }

        public Guid ScgPathologyEmailSMSHistoryId { get; set; }


    }
}
