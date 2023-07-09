using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalDashboardDivisionUnitViewModel
    {
        public long DivisionUnitId { get; set; }
        public string DivisionUnitName { get; set; }
        public long PracticeId { get; set; }
        public long DivisionId { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string DivisionUnitStyle { get; set; }
        public string DivisionUnitBgColor { get; set; }
        public string DivisionUnitDescription { get; set; }
        public string MobileDivisionUnitDescription { get; set; }
    }
}
