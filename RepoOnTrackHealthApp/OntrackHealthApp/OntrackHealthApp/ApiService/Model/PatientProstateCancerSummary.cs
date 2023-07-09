using System;

namespace OntrackHealthApp.ApiService.Model
{
    public class PatientProstateCancerSummary
    {
        public Guid CancerSummaryId { get; set; }

        public Guid? PatientProcedureDetailId { get; set; }

        public long PracticeProfileId { get; set; }

        public long PatientProfileId { get; set; }

        public long ProfessionalProfileId { get; set; }

        public string PreopPsa { get; set; }

        public string StageScore { get; set; }

        public string GleasonScore { get; set; }

        public int? GleasonScoreOne { get; set; }

        public int? GleasonScoreTwo { get; set; }

        public int? GleasonScoreSum { get; set; }

        public string MarginScore { get; set; }

        public string NodeStatus { get; set; }

        public string NodeStatusN
        {
            get
            {
                if (NodeStatus == "+")
                    return "N1";
                return "N0";
            }
        }

        public decimal? CancerInvolvement { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public string FiveYearsFactorScore { get; set; }

        public string TenYearsFactorScore { get; set; }

        public string Metastasis { get; set; }

        public string MetastasisN
        {
            get
            {
                if (Metastasis == "+")
                    return "M1";
                return "M0";
            }
        }
    }
}
