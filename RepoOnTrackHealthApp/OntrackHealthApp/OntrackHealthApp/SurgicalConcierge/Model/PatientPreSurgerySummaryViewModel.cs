using OntrackHealthApp.ApiHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PatientPreSurgerySummaryViewModel
    {
        public PatientPreSurgerySummaryViewModel()
        {
            PatientPreSurgerySummaryPiradViewModels = new List<PatientPreSurgerySummaryPiradViewModel>();
            CancerLocationViewModels = new List<CancerLocationViewModel>();
            PreSurgerySummaryId = Guid.NewGuid();
        }

        public Guid PreSurgerySummaryId { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public long PracticeProfieId { get; set; }

        public long PatientProfileId { get; set; }

        public decimal? PreopPsa { get; set; }

        public string DisplayPreopPsa
        {
            get
            {
                return PreopPsa == null ? "N/A" : PreopPsa.ToRound().ToString();
            }
        }

        public string GleasonScore { get; set; }

        public string DisplayGleasonScore
        {
            get
            {
                string gleasonScore = "N/A";
                if (GleasonScoreOne != 0 && GleasonScoreTwo != 0)
                {
                    gleasonScore = GleasonScoreOne + " + " + GleasonScoreTwo;
                }
                return gleasonScore;
            }
        }

        public int GleasonScoreOne { get; set; }

        public int GleasonScoreTwo { get; set; }

        public string StageScore { get; set; }

        public string DisplayStageScore
        {
            get
            {
                return string.IsNullOrEmpty(StageScore) ? "N/A" : StageScore;
            }
        }

        public decimal? Volume { get; set; }

        public string DisplayVolume
        {
            get
            {
                return Volume == null ? "N/A" : Volume.ToRound().ToString();
            }
        }

        public decimal? IntIndexErectileFunction5 { get; set; }

        public string DisplayIntIndexErectileFunction5
        {
            get
            {
                return IntIndexErectileFunction5 == null ? "N/A" : IntIndexErectileFunction5.ToRound().ToString();
            }
        }

        public decimal? IntProstateSymptomScore { get; set; }

        public string DisplayIntProstateSymptomScore
        {
            get
            {
                return IntProstateSymptomScore == null ? "N/A" : IntProstateSymptomScore.ToRound().ToString();
            }
        }

        public string PracticeProfileName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PreferredName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthStr { get; set; }

        public string PrimaryPhoneCode { get; set; }

        public string PrimaryPhone { get; set; }

        public string EmailAddress { get; set; }

        public virtual List<PatientPreSurgerySummaryPiradViewModel> PatientPreSurgerySummaryPiradViewModels { get; set; }

        public virtual List<CancerLocationViewModel> CancerLocationViewModels { get; set; }

        public long ProfessionalProfileId { get; set; }

        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string ProfessionalName { get; set; }

        public bool IsAlreadyInsertedSummary { set; get; }

        public int? CancerLocationTemplateType { set; get; }

        public string Notes { get; set; }
        public bool HasNotes => !string.IsNullOrEmpty(Notes);

    }
}
