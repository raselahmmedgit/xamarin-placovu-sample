using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class RadicalNephrectomyTrendsViewModel
    {
        public RadicalNephrectomyTrendsViewModel()
        {
        }

        public int ProfessionalProcedureTotalPatient { get; set; }
        public int PracticeTotalPatient { get; set; }
        public int SystemProcedureTotalPatient { get; set; }

        public ProfessionalRiskFactorReportViewModel ProfessionalRiskFactorReportViewModel { get; set; }
        public ProfessionalReadmittedHospitalReportViewModel ProfessionalReadmittedHospitalReportViewModel { get; set; }
        public ProfessionalPainReportViewModel ProfessionalPainReportViewModel { get; set; }
        public ProfessionalGeneralReportViewModel ProfessionalGeneralReportViewModel { get; set; }
        public ProfessionalGIReportViewModel ProfessionalGIReportViewModel { get; set; }
    }

    public class ProfessionalRiskFactorReportViewModel
    {
        public ProfessionalRiskFactorReportViewModel()
        {
            Labels = new List<string>();
            Values = new List<decimal>();
        }
        public List<string> Labels { get; set; }
        public List<decimal> Values { get; set; }
    }

    public class ProfessionalReadmittedHospitalReportViewModel
    {
        public ProfessionalReadmittedHospitalReportViewModel()
        {
            Labels = new List<string>();
            SeenElseWhere = new List<decimal>();
            PrimaryCare = new List<decimal>();
            EmergencyRoom = new List<decimal>();
            UrgentCare = new List<decimal>();
            Hospitalized = new List<decimal>();
        }
        public List<string> Labels { get; set; }

        public List<decimal> SeenElseWhere { get; set; }

        public List<decimal> PrimaryCare { get; set; }

        public List<decimal> EmergencyRoom { get; set; }

        public List<decimal> UrgentCare { get; set; }

        public List<decimal> Hospitalized { get; set; }
    }

    public class ProfessionalPainReportViewModel
    {
        public ProfessionalPainReportViewModel()
        {
            Labels = new List<string>();
            Level0_5 = new List<decimal>();
            PainPills = new List<string>();
            ShoulderPain = new List<decimal>();
        }
        public List<string> Labels { get; set; }

        public List<decimal> Level0_5 { get; set; }

        public List<string> PainPills { get; set; }

        public List<decimal> ShoulderPain { get; set; }
    }

    public class ProfessionalGeneralReportViewModel
    {
        public ProfessionalGeneralReportViewModel()
        {
            Labels = new List<string>();
            Fever = new List<decimal>();
            Fever101 = new List<decimal>();
            Bruising = new List<decimal>();
            IncisionalDrainage = new List<decimal>();
        }
        public List<string> Labels { get; set; }

        public List<decimal> Fever { get; set; }

        public List<decimal> Fever101 { get; set; }

        public List<decimal> Bruising { get; set; }

        public List<decimal> IncisionalDrainage { get; set; }
    }

    public class ProfessionalGIReportViewModel
    {
        public ProfessionalGIReportViewModel()
        {
            Labels = new List<string>();
            Flatus = new List<decimal>();
            BM = new List<decimal>();
            Nausea = new List<decimal>();
            Vomiting = new List<decimal>();
        }
        public List<string> Labels { get; set; }

        public List<decimal> Flatus { get; set; }

        public List<decimal> BM { get; set; }

        public List<decimal> Nausea { get; set; }

        public List<decimal> Vomiting { get; set; }
    }

    public class RadicalNephrectomyTabularReportViewModel
    {
        public long NotificationId { get; set; }
        public long SurveyQuestionId { get; set; }
        public int NotificationOrder { get; set; }
        public string SurveyQuestionShortText { get; set; }
        public string NotificationShortTitle { get; set; }
        public decimal? PatientSurveyScore { get; set; }
        public decimal? ProfessionalSurveyScore { get; set; }
        public decimal? PracticeSurveyScore { get; set; }
        public decimal? AllSurveyScore { get; set; }

    }
}
