using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.PatientOutComeReport.ViewModel
{
    public class GraphDataProperty
    {
        public GraphDataProperty()
        {
            PatientSurveyScore = new List<decimal>();
            ProfessionalSurveyScore = new List<decimal>();
            PracticeSurveyScore = new List<decimal>();
            AllSurveyScore = new List<decimal>();

            ToolTipPatientServeyScore = new List<List<decimal>>();
            ToolTipProfessionalServeyScore = new List<List<decimal>>();
            ToolTipPracticeServeyScore = new List<List<decimal>>();
            ToolTipAllServeyScore = new List<List<decimal>>();
        }        
        public List<decimal> PatientSurveyScore { set; get; }
        public List<decimal> ProfessionalSurveyScore { set; get; }
        public List<decimal> PracticeSurveyScore { set; get; }
        public List<decimal> AllSurveyScore { set; get; }
        public List<List<decimal>> ToolTipPatientServeyScore { set; get; }
        public List<List<decimal>> ToolTipProfessionalServeyScore { set; get; }
        public List<List<decimal>> ToolTipPracticeServeyScore { set; get; }
        public List<List<decimal>> ToolTipAllServeyScore { set; get; }
    }
}
