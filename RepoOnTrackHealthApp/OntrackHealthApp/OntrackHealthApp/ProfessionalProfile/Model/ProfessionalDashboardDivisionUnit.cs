using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.Model
{
    public class ProfessionalDashboardDivisionUnit
    {
        public long DivisionUnitId { get; set; }

        public string DivisionUnitName { get; set; }

        public string MobileDivisionUnitName { get; set; }

        public long PracticeProfileId { get; set; }

        public long DivisionId { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string DivisionUnitStyle { get; set; }

        public string DivisionUnitDescription { get; set; }

        public string DivisionUnitBgColor { get; set; }

        public string DivisionUnitStyleIcon { get; set; }

        public string DivisionUnitStyleIconBgColor { get; set; }

        public string MobileDivisionUnitDescription { get; set; }

        public string GetDivisionUnitStyleIcon()
        {
            if (DivisionUnitId == 1 && DivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Hospital)
            {
                return "pro_operation_room_unit_icon.png";
            }
            if (DivisionUnitId == 2 && DivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Hospital)
            {
                return "pro_pacu_unit_icon.png";
            }
            if (DivisionUnitId == 13 && DivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Hospital)
            {
                return "pro_floor_unit_icon.png";
            }
            if (DivisionUnitId == 5 && DivisionId == (int)AppCore.Enums.ProfessionPracticeDivision.Hospital)
            {
                return "pro_discharge_unit_icon.png";
            }
            return "";
        }
    }
}