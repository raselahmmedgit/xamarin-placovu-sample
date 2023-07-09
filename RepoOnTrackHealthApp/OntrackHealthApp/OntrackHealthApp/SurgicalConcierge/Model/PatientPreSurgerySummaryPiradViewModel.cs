using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class PatientPreSurgerySummaryPiradViewModel
    {
        public PatientPreSurgerySummaryPiradViewModel()
        {
            PiradLesionId = Guid.NewGuid();
        }
        public Guid PiradLesionId { get; set; }

        public Guid PreSurgerySummaryId { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public decimal? PiradScore { get; set; }

        public decimal? PiradVolume { get; set; }

        public string LesionLocation { get; set; }

        public string LesionZone { get; set; }

        public string LesionGrade { get; set; }
        public string DisplayLesionGrade
        {
            get
            {
                string lesionGrade = null;
                if (LesionGradeOne != 0 && LesionGradeTwo != 0)
                {
                    lesionGrade = LesionGradeOne + " + " + LesionGradeTwo;
                }
                return lesionGrade;
            }
        }

        public int LesionGradeOne { get; set; }

        public int LesionGradeTwo { get; set; }

        public string CapsularInvolvement { get; set; }

        public int RowIndex { get; set; }

        public bool IsDeleted { get; set; }
    }
}
