using OntrackHealthApp.ApiHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }

        //[StringLength(128)]
        public string CountryName { get; set; }

        //[StringLength(128)]
        public string CountryDisplayName { get; set; }

        //[StringLength(5)]
        public string CountryIso { get; set; }

        //[StringLength(5)]
        public string CountryIso3 { get; set; }

        //[StringLength(8)]
        public string NumberCode { get; set; }

        //[StringLength(8)]
        public string PhoneCode { get; set; }

        public int PhoneCodeNumeric
        {
            get
            {
                if (!string.IsNullOrEmpty(PhoneCode))
                {
                    return PhoneCode.ToInt();
                }
                return 0;
            }
        }

        public bool IsPublished { get; set; }
    }
}
