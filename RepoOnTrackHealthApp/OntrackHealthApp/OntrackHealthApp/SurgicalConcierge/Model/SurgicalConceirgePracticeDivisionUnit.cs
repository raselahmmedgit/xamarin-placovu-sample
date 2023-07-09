using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class SurgicalConceirgePracticeDivisionUnit
    {
        public long DivisionUnitId { get; set; }
        public string DivisionUnitName { get; set; }
        public string DivisionUnitName2 { get; set; }
        public long PracticeId { get; set; }
        public long SurgicalConceirgeDivisionId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPracticeAccess { get; set; }
        public bool IsProfessionalAccess { get; set; }
        public bool IsOperatingRoomSurgeonAccess { get; set; }
        public bool IsOperaingRoomNurseAccess { get; set; }
        public bool IsPatientAccess { get; set; }
        public bool IsSchedulerAccess { get; set; }
        public int DisplayOrder { get; set; }
        public string DivisionUnitStyle { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string DivisionUnitBgColor { get; set; }
        public double FontSize { get; set; } = 24;
        public string DivisionUnitDescription { get; set; }
    }
}
