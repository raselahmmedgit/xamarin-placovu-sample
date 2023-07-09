using OntrackHealthApp.ProfessionalProfile.Model;
using System.Collections.Generic;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ProfessionalDashboardDivisionViewModel
    {
        public long DivisionId { get; set; }

        public string DivisionName { get; set; }

        public string DivisionNameHtml { get; set; }

        public string ShortDescription { get; set; }

        public long PracticeId { get; set; }

        public int DisplayOrder { get; set; }

        public string DivisionStyleBgColor { get; set; }

        public string DivisionStyleIcon { get; set; }

        public string DivisionStyleIconBgColor { get; set; }

        public IEnumerable<ProfessionalDashboardDivisionUnit> ProfessionalDashboardDivisionUnits { set; get; }

        public string GetDivisionUnitStyleIcon()
        {
            if (DivisionId == 1)
            {
                return "pro_dash_division_icon_one.png";
            }
            if (DivisionId == 2)
            {
                return "pro_dash_division_icon_two.png";
            }
            if (DivisionId == 3)
            {
                return "pro_dash_division_icon_three.png";
            }
            if (DivisionId == 4)
            {
                return "pro_dash_division_icon_four.png";
            }
            return "";
        }

        public string GetDivisionUnitStyleBgColor()
        {
            if (DivisionId == 1)
            {
                return "#28a745";
            }
            if (DivisionId == 2)
            {
                return "#dc3545";
            }
            if (DivisionId == 3)
            {
                return "#007bff";
            }
            if (DivisionId == 4)
            {
                return "#6610f2";
            }
            return "";
        }
    }
}
