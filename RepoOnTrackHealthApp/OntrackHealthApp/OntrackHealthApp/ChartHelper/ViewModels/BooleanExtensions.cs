﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ChartHelper.ViewModels
{
    public static class BooleanExtensions
    {
        public static string ToBooleanString(this bool b)
        {
            if (b == true)
                return "true";
            return "false";
        }
    }
}
