using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ChartHelper.Models
{
    public static class ChartExtension
    {
        public static string ToRoundedDecimalString(this decimal data)
        {            
            return Math.Round(data, 0).ToString();
        }
    }
}
